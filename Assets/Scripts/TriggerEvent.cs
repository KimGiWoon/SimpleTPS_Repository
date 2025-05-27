using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private bool _isReuse;
    [SerializeField] private UnityEvent _onTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayer == (_targetLayer | (1 << other.gameObject.layer)))
        {
            _onTrigger?.Invoke();

            if (!_isReuse) Destroy(gameObject);
        }
    }
}
