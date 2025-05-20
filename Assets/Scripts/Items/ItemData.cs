using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string name;
    [TextArea] public string Description;
    public Sprite icon;
    public GameObject itemPrefab;

    // 사용한다는 추상메서드
    public abstract void Use(PlayerController controller);

}
