using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum SlotContents
    {
        Empty,
        Cross,
        Circle
    }

    //Public
    public bool PlayerTurn = true;

    //Settings
    [SerializeField] private GridSlot m_SlotPrefab;
    [SerializeField] private Sprite m_Cross;
    [SerializeField] private Sprite m_Circle;

    //Private
    private GridSlot[,] m_Grid;
    private List<Vector2Int> m_OpenSlots;
    private event Action<SlotContents> EndTurn;


    private void Awake()
    {
        m_Grid = new GridSlot[3, 3];
        m_OpenSlots = new List<Vector2Int>();
    }

    public void Initialize(Action<SlotContents> endTurnEvent)
    {
        Action<Vector2Int> slotUpdated = UpdateSlot;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GridSlot slot = Instantiate(m_SlotPrefab);
                slot.InitializeSlot(new Vector2Int(x, y), slotUpdated);
                m_Grid[x, y] = slot;
                m_OpenSlots.Add(new Vector2Int(x, y));
            }
        }

        EndTurn = endTurnEvent;
    }

    public void UpdateSlot(Vector2Int slotPos)
    {
        if (PlayerTurn)
        {
            m_Grid[slotPos.x, slotPos.y].PlaySlot(m_Cross, PlayerTurn);
        }
        else
        {
            m_Grid[slotPos.x, slotPos.y].PlaySlot(m_Circle, PlayerTurn);
        }

        foreach (GridSlot slot in m_Grid)
        {
            slot.m_CanBeClicked = !slot.m_CanBeClicked;
        }

        m_OpenSlots.Remove(slotPos);

        SlotContents win = CheckWin(slotPos);
        EndTurn.Invoke(win);
    }

    private SlotContents CheckWin(Vector2Int slotPos)
    {
        SlotContents currentSlot = m_Grid[slotPos.x, slotPos.y].CurrentContents;

        int yWin = -1;
        int xWin = -1;
        int xyWin = -1;
        int yxWin = -1;

        for (int i = 0; i < 3; i++)
        {
            if (m_Grid[i, i].CurrentContents == currentSlot)
            {
                xyWin++;
            }
            if (m_Grid[i, 2 - i].CurrentContents == currentSlot)
            {
                yxWin++;
            }
            if (m_Grid[i, slotPos.y].CurrentContents == currentSlot)
            {
                yWin++;
            }
            if (m_Grid[slotPos.x, i].CurrentContents == currentSlot)
            {
                xWin++;
            }

            if (yWin == 2 || xWin == 2 || xyWin == 2 || yxWin == 2)
            {
                return currentSlot;
            }
        }

        return SlotContents.Empty;
    }

    public List<Vector2Int> GetOpenSlots()
    {
        return m_OpenSlots;
    }

    public DummyGrid CreateDummy(Vector2Int testSlot, bool maximizingPlayer)
    {
        return new DummyGrid(m_Grid, testSlot, maximizingPlayer);
    }
}
