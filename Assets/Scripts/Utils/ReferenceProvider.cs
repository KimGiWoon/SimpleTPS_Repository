using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetComponent 우회 사용법

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
        // 예외처리 필요
        return _component as T;
    }
}
