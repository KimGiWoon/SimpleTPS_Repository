using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemData> _slots = new();
    PlayerController _controller;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _controller = GetComponent<PlayerController>();
    }

    public void GetItem(ItemData itemData)
    {
        _slots.Add(itemData);
    }

    public void UseItem(int index)
    {
        if (_slots.Count == 0)
        {
            Debug.Log("인벤토리가 비어있습니다.");
            return;
        }
        _slots[index].Use(_controller);
        _slots[index] = null;
        _slots.RemoveAt(index);
    }
}
