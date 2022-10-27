using System;
using System.Collections.Generic;

public class PathQuery
{
    Dictionary<Type, int> m_Filter = new Dictionary<Type, int>();

    public PathQuery()
    {
        m_Filter = new Dictionary<Type, int>();

        AddFilter<FireplaceTile>(50);
    }

    public PathQuery(PathQuery rhs)
    {
        m_Filter = new Dictionary<Type, int>(rhs.m_Filter);
    }

    public void AddFilter<T>(int costModifier) where T : PathfindingTile
    {
        m_Filter.Add(typeof(T), costModifier);
    }

    public void RemoveFilter<T>() where T : PathfindingTile
    {
        m_Filter.Remove(typeof(T));
    }

    public int GetCostModifier(PathfindingTile tile)
    {
        if(m_Filter.ContainsKey(tile.GetType()) == false)
        {
            return 1;
        }

        return m_Filter[tile.GetType()];
    }

    public void Clear()
    {
        m_Filter.Clear();
    }
}