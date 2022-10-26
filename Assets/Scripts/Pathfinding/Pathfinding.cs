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

        private PathfindingTile m_Tile;
        public PathfindingTile Tile => m_Tile;

        public void SetHCost(int hCost) => m_HCost = hCost;
        public void SetGCost(int gCost) => m_GCost = gCost;

        public void SetParent(Node parent) => m_Parent = parent;

        public int HeapIndex
        {
            get { return m_HeapIndex; }
            set { m_HeapIndex = value; }
        }

        public int FCost => m_GCost + m_HCost;

        public Node(Vector3Int position, PathfindingTile tile)
        {
            m_Position = position;
            m_Tile = tile;
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

    public Path FindPath(Vector3 startPosition, Vector3 endPosition, PathQuery pathQuery, Action<Agent> pathCompleteAction)
    {
        Vector3Int startCellPosition = m_Tilemap.WorldToCell(startPosition);
        Vector3Int endCellPosition = m_Tilemap.WorldToCell(endPosition);

        PathfindingTile startTile = m_Tilemap.GetTile(startCellPosition) as PathfindingTile;
        PathfindingTile endTile = m_Tilemap.GetTile(endCellPosition) as PathfindingTile;

        Debug.Assert(startTile != null, "Start tile is invalid!");
        Debug.Assert(endTile != null, "End tile is invalid!");

        TileBase[] allTiles = m_Tilemap.GetTilesBlock(m_Tilemap.cellBounds);

        Dictionary<Vector3Int, int> costTable = new Dictionary<Vector3Int, int>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
        Heap<Node> openList = new Heap<Node>(allTiles.Length);

        Node startNode = new Node(startCellPosition, startTile);
        Node targetNode = new Node(endCellPosition, endTile);
        Path path = null;

        costTable[startCellPosition] = 0;
        openList.Add(startNode);

        while(openList.Count > 0)
        {
            Node currentNode = openList.RemoveFirst();

            if(currentNode.Position == targetNode.Position)
            {
                // Build path
                path = BuildPath(currentNode, targetNode.Position, pathQuery, pathCompleteAction);
                break;
            }

            closedList.Add(currentNode.Position);

            int currentNodeGCost = costTable[currentNode.Position];

            foreach(Node neighbourNode in GetNeighbours(currentNode, pathQuery))
            {
                if(closedList.Contains(neighbourNode.Position))
                {
                    continue;
                }

                Vector3Int neighbourPosition = neighbourNode.Position;

                int newGCost = 
                    currentNodeGCost + GetHeuristic(currentNode, neighbourNode);
                int currentNeighbourGCost = 
                    costTable.ContainsKey(neighbourPosition) ? costTable[neighbourPosition] : int.MaxValue;

                if (newGCost < currentNeighbourGCost)
                {
                    costTable[neighbourPosition] = newGCost;

                    neighbourNode.SetGCost(newGCost);
                    neighbourNode.SetHCost(GetHeuristic(neighbourNode, targetNode));
                    openList.Add(neighbourNode);
                }
            }
        }

        return path;
    }

    public Path.Request GeneratePathRequestToClosestTileOfType<T>(
        Vector3 currentPosition, 
        PathQuery pathQuery, 
        Action<Agent> pathCompleteAction)
    {
        Vector3Int tileStartPos = m_Tilemap.WorldToCell(currentPosition);
        PathfindingTile startTile = m_Tilemap.GetTile(tileStartPos) as PathfindingTile;

        Debug.Assert(startTile != null, "Start tile is invalid!");

        Queue<Node> openList = new Queue<Node>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();

        Node startNode = new Node(tileStartPos, startTile);
        Node bestNode = null;

        openList.Enqueue(startNode);

        while(openList.Count > 0)
        {
            Node currentNode = openList.Dequeue();

            closedList.Add(currentNode.Position);

            foreach (Node neighbourNode in GetNeighbours(currentNode, pathQuery))
            {
                if(closedList.Contains(neighbourNode.Position))
                {
                    continue;
                }

                if(neighbourNode.Tile.GetType() == typeof(T))
                {
                    bestNode = neighbourNode;
                    break;
                }
                else
                {
                    openList.Enqueue(neighbourNode);
                }
            }
        }

        Path.Request pathRequest = null;

        if(bestNode != null)
        {
            pathRequest = new Path.Request(bestNode.Position, pathQuery, pathCompleteAction);
        }

        return pathRequest;
    }

    private Path BuildPath(Node node, Vector3 targetPosition, PathQuery pathQuery, Action<Agent> pathCompleteAction)
    {
        List<Path.Node> pathNodes = new List<Path.Node>();
        float pathCost = node.FCost;

        do
        {
            pathNodes.Add(new Path.Node(m_Tilemap.GetCellCenterWorld(node.Position)));
            node = node.Parent;
        } while (node != null);

        pathNodes.Reverse();

        return new Path(pathNodes, targetPosition, pathCost, pathQuery, pathCompleteAction);
    }

    private List<Node> GetNeighbours(Node node, PathQuery pathQuery)
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

                if (neighbourTile != null && neighbourTile.IsWalkable && pathQuery.PassFilter(neighbourTile))
                {
                    Node neighbourNode = new Node(neighbourPos, neighbourTile);

                    // Debug.Log($"Type of tile : {neighbourTile.GetType()}");

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