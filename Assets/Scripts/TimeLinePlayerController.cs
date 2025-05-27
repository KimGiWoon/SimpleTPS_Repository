using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeLinePlayerController : MonoBehaviour
{
    [SerializeField] [Range(0, 20)] private float _moveSpeed;
    
    private Rigidbody _rigidbody;
    public bool IsActivateControl { get; set; } = true;

    private void Awake() => Init();
    private void Update() => HandlePlayerControl();
    
    private void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();    
    }

    private void HandlePlayerControl()
    {
        if (!IsActivateControl) return;
        
        _rigidbody.velocity = GetMoveDireciton() * _moveSpeed;
    }

    private Vector3 GetMoveDireciton()
    {
        Vector3 direction = new()
        {
            x = Input.GetAxisRaw("Horizontal"),
            z = Input.GetAxisRaw("Vertical")
        };
        
        return direction.normalized;
    }
}
