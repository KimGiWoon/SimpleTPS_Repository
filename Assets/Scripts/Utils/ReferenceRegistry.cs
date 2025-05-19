using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetComponent ��ȸ ����

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
        // ��ųʸ��� �߰� �Ǿ� ���� ������ NULL ��ȯ
        if (!_providers.ContainsKey(gameObject))
        {
            return null;
        }
        return _providers[gameObject];
    }

}
