using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [field: SerializeField] public ItemData data { get; private set; }
    private GameObject _childObject;

    private void OnEnable()
    {
        _childObject = Instantiate(data.itemPrefab, transform);
    }

    private void OnDisable()
    {
        Destroy(_childObject);
    }
}
