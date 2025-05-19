using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ReferenceProvider : MonoBehaviour
{
    [SerializeField] Component _component;

    private void Awake()
    {
        ReferenceRegistry.Register(this);
    }

    private void OnDestroy()
    {
        ReferenceRegistry.UnRegister(this);
    }

    public T GetAs<T>() where T : Component
    {
        // ����ó�� �ʿ�
        return _component as T;
    }
}
