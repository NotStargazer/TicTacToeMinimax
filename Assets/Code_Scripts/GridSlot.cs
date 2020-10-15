using System;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    //Public
    public Grid.SlotContents CurrentContents;
    public bool m_CanBeClicked = true;

    //Private
    private bool m_SlotFilled = false;
    private Vector2Int m_SlotPosition;
    private event Action<Vector2Int> m_SlotDelegate;

    //Components
    [SerializeField] private SpriteRenderer m_Slot;

    public void PlaySlot(Sprite sprite, bool player)
    {
        m_Slot.sprite = sprite;

        if (player)
        {
            CurrentContents = Grid.SlotContents.Cross;
        }
        else
        {
            CurrentContents = Grid.SlotContents.Circle;
        }
    }

    public void InitializeSlot(Vector2Int pos, Action<Vector2Int> slot)
    {
        m_SlotPosition = pos;
        transform.position = pos * 2 + new Vector2(-2, -2);
        m_SlotDelegate = slot;
    }

    private void OnMouseDown()
    {
        if (!m_SlotFilled && m_CanBeClicked)
        {
            m_SlotDelegate.Invoke(m_SlotPosition);
            m_SlotFilled = true;
        }
    }
}
