using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string name;
    [TextArea] public string Description;
    public Sprite icon;
    public GameObject itemPrefab;

    // ����Ѵٴ� �߻�޼���
    public abstract void Use(PlayerController controller);

}
