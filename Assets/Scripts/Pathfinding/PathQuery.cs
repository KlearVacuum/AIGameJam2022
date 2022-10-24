﻿using System;
using System.Collections.Generic;

public class PathQuery
{
    HashSet<Type> m_Filter = new HashSet<Type>();

    public void AddFilter<T>() where T : PathfindingTile
    {
        m_Filter.Add(typeof(T));
    }

    public bool PassFilter(PathfindingTile tile)
    {
        return m_Filter.Contains(tile.GetType()) == false;
    }

    public void Clear()
    {
        m_Filter.Clear();
    }
}