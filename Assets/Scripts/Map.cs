using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map
{
    private int[,] grid;
    private Vector2Int size;

    public Map(int lenth, int width)
    {
        size.x = lenth;
        size.y = width;
        grid = new int[lenth, width];
    }

    public void Clear()
    {
        grid = new int[size.x, size.y];
    }

    public void ForcePlace(int x, int y, int length, int width)
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[x + i, y + j] = 1;
            }
        }
    }

    public bool Place(int x, int y, int length, int width)
    {
        //copy map grid
        int[,] _grid = (int[,])grid.Clone();

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //check if already occupied
                if (_grid[x + i, y + j] == 1) return false;

                //occupie space in grid
                _grid[x + i, y + j] = 1;
            }
        }

        //overwrite map grid
        grid = _grid;


        return true;
    }

    public bool IsFree(int x, int y)
    {
        if (grid[x, y] == 0) return true;

        return false;

    }
}


/*
         for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {

            }
        }
 */