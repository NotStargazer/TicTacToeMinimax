using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGrid
{
    public Grid.SlotContents[,] DGrid;
    public List<Vector2Int> EmptySlots = new List<Vector2Int>();

    public DummyGrid(GridSlot[,] grid, Vector2Int pos, bool playerTurn)
    {
        DGrid = new Grid.SlotContents[3, 3];

        for (int x = 0; x <= 2; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                if (new Vector2Int(x, y) == pos)
                {
                    if (playerTurn)
                    {
                        DGrid[x, y] = Grid.SlotContents.Cross;
                    }
                    else
                    {
                        DGrid[x, y] = Grid.SlotContents.Circle;
                    }
                }
                else
                {
                    DGrid[x, y] = grid[x, y].CurrentContents;
                }

                if (DGrid[x, y] == Grid.SlotContents.Empty)
                {
                    EmptySlots.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    public DummyGrid(Grid.SlotContents[,] grid, Vector2Int pos, bool playerTurn)
    {
        DGrid = new Grid.SlotContents[3, 3];

        for (int x = 0; x <= 2; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                if (new Vector2Int(x, y) == pos)
                {
                    if (playerTurn)
                    {
                        DGrid[x, y] = Grid.SlotContents.Cross;
                    }
                    else
                    {
                        DGrid[x, y] = Grid.SlotContents.Circle;
                    }
                }
                else
                {
                    DGrid[x, y] = grid[x, y];
                }

                if (DGrid[x, y] == Grid.SlotContents.Empty)
                {
                    EmptySlots.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    public int GetMax(Vector2Int slotPos)
    {
        Grid.SlotContents currentSlot = DGrid[slotPos.x, slotPos.y];

        int yWin = -1;
        int xWin = -1;
        int xyWin = -1;
        int yxWin = -1;

        for (int i = 0; i < 3; i++)
        {
            if (DGrid[i, i] == currentSlot)
            {
                xyWin++;
            }
            if (DGrid[i, 2 - i] == currentSlot)
            {
                yxWin++;
            }
            if (DGrid[i, slotPos.y]== currentSlot)
            {
                yWin++;
            }
            if (DGrid[slotPos.x, i]== currentSlot)
            {
                xWin++;
            }
        }

        int max1 = Math.Max(yWin, xWin);
        int max2 = Math.Max(xyWin, yxWin);
        int result = Math.Max(max1, max2);
        return result;
    }
}
