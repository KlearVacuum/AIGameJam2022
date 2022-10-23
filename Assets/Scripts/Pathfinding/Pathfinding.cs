using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

class Pathfinding
{
    private class Node : IHeapItem<Node>
    {
        private Vector3Int m_Position;
        public Vector3Int Position => m_Position;

        private Node m_Parent = null;

        public Node Parent => m_Parent;

        private int m_HeapIndex;

        private int m_GCost = 0;
        private int m_HCost = 0;

        public int HCost => m_HCost;

        public void SetHCost(int hCost) => m_HCost = hCost;
        public void SetGCost(int gCost) => m_GCost = gCost;

        public void SetParent(Node parent) => m_Parent = parent;

        public int HeapIndex
        {
            get { return m_HeapIndex; }
            set { m_HeapIndex = value; }
        }

        public int FCost => m_GCost + m_HCost;

        public Node(Vector3Int position)
        {
            m_Position = position;
        }

        public int CompareTo(Node otherNode)
        {
            int compare = FCost.CompareTo(otherNode.FCost);

            if(compare == 0)
            {
                compare = m_HCost.CompareTo(otherNode.m_HCost);
            }

            return -compare;
        }
    }

    private Tilemap m_Tilemap;

    public void Initialize(Tilemap tilemap)
    {
        m_Tilemap = tilemap;
    }

    //public void Start()
    //{
    //    Vector3 startPosition = new Vector3(-3.63f, 1.51f, 10f);
    //    Vector3 endPosition = new Vector3(-2.34f, 0.42f, -10f);

    //    m_Tilemap.CompressBounds();

    //    Path path = FindPath(startPosition, endPosition);
    //}

    //public void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        PathfindingTile tile = m_Tilemap.GetTile(m_Tilemap.WorldToCell(mouseWorldPos)) as PathfindingTile;

    //        if (tile != null)
    //        {
    //            Debug.Log("TilePos = " + mouseWorldPos);
    //        }
    //    }
    //}

    public Path FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3Int startCellPosition = m_Tilemap.WorldToCell(startPosition);
        Vector3Int endCellPosition = m_Tilemap.WorldToCell(endPosition);

        Debug.Assert(m_Tilemap.GetTile(startCellPosition) != null, "Start tile is invalid!");
        Debug.Assert(m_Tilemap.GetTile(endCellPosition) != null, "End tile is invalid!");

        TileBase[] allTiles = m_Tilemap.GetTilesBlock(m_Tilemap.cellBounds);

        Dictionary<Vector3Int, int> costTable = new Dictionary<Vector3Int, int>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
        Heap<Node> openList = new Heap<Node>(allTiles.Length);

        Node startNode = new Node(startCellPosition);
        Node targetNode = new Node(endCellPosition); 
        Path path = null;

        costTable[startCellPosition] = 0;
        openList.Add(startNode);

        while(openList.Count > 0)
        {
            Node currentNode = openList.RemoveFirst();

            if(currentNode.Position == targetNode.Position)
            {
                // Build path
                path = BuildPath(currentNode);
                break;
            }

            closedList.Add(currentNode.Position);

            int currentNodeGCost = costTable[currentNode.Position];

            foreach(Node neighbourNode in GetNeighbours(currentNode))
            {
                if(closedList.Contains(neighbourNode.Position))
                {
                    continue;
                }

                Vector3Int neighbourPosition = neighbourNode.Position;
                int newGCost = currentNodeGCost + GetHeuristic(currentNode, neighbourNode);
                int currentNeighbourGCost = 
                    costTable.ContainsKey(neighbourPosition) ? costTable[neighbourPosition] : int.MaxValue;

                if (newGCost < currentNeighbourGCost || !openList.Contains(neighbourNode))
                {
                    costTable[neighbourPosition] = newGCost;

                    neighbourNode.SetGCost(newGCost);
                    neighbourNode.SetHCost(GetHeuristic(neighbourNode, targetNode));
                    
                    if(openList.Contains(neighbourNode))
                    {
                        openList.UpdateItem(neighbourNode);
                    }
                    else
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return path;
    }

    private Path BuildPath(Node node)
    {
        List<Path.Node> pathNodes = new List<Path.Node>();

        do
        {
            m_Tilemap.SetTileFlags(node.Position, TileFlags.None);
            m_Tilemap.SetColor(node.Position, Color.green);
            pathNodes.Add(new Path.Node(m_Tilemap.GetCellCenterWorld(node.Position)));
            node = node.Parent;
        } while (node.Parent != null);

        Debug.Assert(node != null);

        // Add last node
        pathNodes.Add(new Path.Node(node.Position));
        pathNodes.Reverse();

        return new Path(pathNodes);
    }

    private List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; ++x)
        {
            for(int y = -1; y <= 1; ++y)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                if(Math.Abs(x) == 1 && Math.Abs(y) == 1)
                {
                    continue;
                }

                Vector3Int neighbourPos = new Vector3Int(node.Position.x + x, node.Position.y + y);
                PathfindingTile neighbourTile = m_Tilemap.GetTile(neighbourPos) as PathfindingTile;

                if (neighbourTile != null && neighbourTile.IsWalkable)
                {
                    Node neighbourNode = new Node(neighbourPos);

                    neighbourNode.SetParent(node);
                    neighbours.Add(neighbourNode);
                }
            }
        }

        return neighbours;
    }

    private int GetHeuristic(Node nodeA, Node nodeB)
    {
        Vector3Int distance = nodeA.Position - nodeB.Position;

        return (Math.Abs(distance.x) + Math.Abs(distance.y)) * 10;
    }
}