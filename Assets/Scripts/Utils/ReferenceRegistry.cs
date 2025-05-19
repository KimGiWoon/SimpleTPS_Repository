using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetComponent 우회 사용법

public static class ReferenceRegistry
{
    private static Dictionary<GameObject, ReferenceProvider> _providers = new();

    public static void Register(ReferenceProvider referenceProvider)
    {
        if(_providers.ContainsKey(referenceProvider.gameObject))
        {
            return;
        }
        _providers.Add(referenceProvider.gameObject, referenceProvider);
    }

    public static void UnRegister(ReferenceProvider referenceProvider)
    {
        if (!_providers.ContainsKey(referenceProvider.gameObject))
        {
            return;
        }
        _providers.Remove(referenceProvider.gameObject);
    }

    public static void Clear()
    {
        _providers.Clear();
    }

    public static ReferenceProvider GetProvider(GameObject gameObject)
    {
        // 딕셔너리에 추가 되어 있지 않으면 NULL 반환
        if (!_providers.ContainsKey(gameObject))
        {
            return null;
        }
        return _providers[gameObject];
    }

}
