using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    bool IsControlActivate { get; set; } = true;

    PlayerStatus _status;
    PlayerMovement _movement;
    Animator _animator;

    [SerializeField] CinemachineVirtualCamera _aimCamera;

    [SerializeField] KeyCode _aimKey = KeyCode.Mouse1;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Update()
    {
        HandlePlayerControl();
    }

    private void OnDisable()
    {
        CancelsubscribeEvents();
    }

    /// <summary>
    /// 초기화용 함수, 객체 생성시 필요한 초기화 작업이 있다면 여기서 수행한다.
    /// </summary>
    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void HandlePlayerControl()
    {
        if (!IsControlActivate)
        {
            return;
        } 

        HandleMovement();
        HandleAiming();
    }

    private void HandleMovement()
    {
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAiming.Value)
        {
            moveSpeed = _status.WalkSpeed;
        }
        else
        {
            moveSpeed = _status.RunSpeed;
        }

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        _status.IsMoving.Value = (moveDir != Vector3.zero);

        Vector3 avatarDir;
        if (_status.IsAiming.Value)
        {
            avatarDir = camRotateDir;
        }
        else
        {
            avatarDir = moveDir;
        }

        _movement.SetAvatarRotation(avatarDir);

        // SetAnimation Parameter
        if(_status.IsAiming.Value)
        {
            Vector3 input = _movement.GetInputDirection();
            _animator.SetFloat("X", input.x);
            _animator.SetFloat("Z", input.z);
        }
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
    }

    public void SubscribeEvents()
    {
        // IsMoving
        _status.IsMoving.Subscribe(SetRunAnimation);

        // IsAimiing
        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);
    }

    public void CancelsubscribeEvents()
    {
        // IsMoving
        _status.IsMoving.Subscribe(SetRunAnimation);

        // IsAiming
        _status.IsAiming.Cancelsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Cancelsubscribe(SetAimAnimation);
    }

    private void SetAimAnimation(bool value)
    {
        _animator.SetBool("IsAim", value);
    }

    private void SetRunAnimation(bool value)
    {
        _animator.SetBool("IsRun", value);
    }
}














