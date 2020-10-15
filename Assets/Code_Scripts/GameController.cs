using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Private
    private bool m_PlayerTurn = true;

    //Components
    [SerializeField] private Grid m_Grid;

    private void Start()
    {
        Action<Grid.SlotContents> endTurn = ChangeTurn;
        m_Grid.Initialize(endTurn);
    }

    private void ChangeTurn(Grid.SlotContents winner)
    {
        if (winner == Grid.SlotContents.Empty)
        {
            m_PlayerTurn = !m_PlayerTurn;
            m_Grid.PlayerTurn = m_PlayerTurn;
        }
        else
        {
            print(winner.ToString() + " Wins!");
        }

        if (!m_PlayerTurn)
        {
            StartCoroutine(WaitForAI(1));
        }
    }

    IEnumerator WaitForAI(float seconds)
    {
        Dictionary<Vector2Int, int> slots = new Dictionary<Vector2Int, int>();

        foreach (Vector2Int slot in m_Grid.GetOpenSlots())
        {
            DummyGrid dummy = m_Grid.CreateDummy(slot, false);

            int value = MiniMax(slot, dummy, 1, true);
            if (slots.Count == 0)
            {
                slots.Add(slot, value);
            }
            else if (slots.ContainsValue(value))
            {
                slots.Add(slot, value);
            }
            else if (value < slots.Values.First())
            {
                slots.Clear();
                slots.Add(slot, value);
            }
        }

        yield return new WaitForSeconds(seconds);
        
        int randNum = UnityEngine.Random.Range(0, slots.Count);

        int loops = 0;
        foreach (var item in slots)
        {
            if (loops == randNum)
            {
                m_Grid.UpdateSlot(item.Key);
                break;
            }
            loops++;
        }

    }

    private int MiniMax(Vector2Int position, DummyGrid grid, int depth, bool maximizingPlayer)
    {
        if (depth == 0)
        {
            return grid.GetMax(position);
        }

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;

            foreach (Vector2Int child in grid.EmptySlots)
            {
                DummyGrid dummy = new DummyGrid(grid.DGrid, child, true);

                int eval = MiniMax(child, dummy, depth - 1, true);
                maxEval = Math.Max(maxEval, eval);
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;

            foreach (Vector2Int child in grid.EmptySlots)
            {
                DummyGrid dummy = new DummyGrid(grid.DGrid, child, false);

                int eval = MiniMax(child, dummy, depth - 1, false);
                minEval = Math.Min(minEval, eval);
            }
            return minEval;
        }
    }
}
