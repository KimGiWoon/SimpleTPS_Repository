using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    bool IsControlActivate { get; set; } = true;

    PlayerStatus _status;
    PlayerMovement _movement;
    Animator _animator;
    Image _image;
    InputAction _aimInputAction;
    InputAction _shootInputAction;
    Inventory _inventory;

    [SerializeField] CinemachineVirtualCamera _aimCamera;
    [SerializeField] GunController _gunController;
    [SerializeField] Animator _aimAnimator;
    [SerializeField] HpGuageUI _hpGuageUI;

    [SerializeField] KeyCode _aimKey = KeyCode.Mouse1;
    [SerializeField] KeyCode _shootKey = KeyCode.Mouse0;

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
        ItemUse();
        
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
        _image = _aimAnimator.GetComponent<Image>();
        _inventory = GetComponent<Inventory>();
        _aimInputAction = GetComponent<PlayerInput>().actions["Aim"];
        _shootInputAction = GetComponent<PlayerInput>().actions["Shoot"];

        _hpGuageUI.SetImageFillAmount(1);
        _status.CurrentHp.Value = _status.MaxHp;

    }

    private void HandlePlayerControl()
    {
        if (!IsControlActivate)
        {
            return;
        }

        HandleMovement();
        //HandleAiming();
        HandleShooting();
       
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

        // Aim Mode 
        if (_status.IsAiming.Value)
        {
            //Vector3 input = _movement.GetInputDirection();
            //_animator.SetFloat("X", input.x);
            //_animator.SetFloat("Z", input.z);

            _animator.SetFloat("X", _movement.InputDirection.x);
            _animator.SetFloat("Z", _movement.InputDirection.y);

        }
    }

    private void HandleAiming(InputAction.CallbackContext ctx)
    {
        // _status.IsAiming.Value = Input.GetKey(_aimKey);
        _status.IsAiming.Value = ctx.started;
        if(_status.IsAiming.Value)
        {
            _hpGuageUI.gameObject.SetActive(false);
        }
        else
        {
            _hpGuageUI.gameObject.SetActive(true);
        }

    }

    // Player Take Damage
    public void TakeDamage(int value)
    {
        _status.CurrentHp.Value -= value;
        Debug.Log($"플레이어가 {value} 데미지 받음");

        if (_status.CurrentHp.Value <= 0)
        {
            PlayerDie();
        } 
    }
    
    // Player Health Recovery
    public void RecoveryHp(int value)   // TODO : 아이템과의 상호작용 구현 예정
    {
        int hp = _status.CurrentHp.Value + value;
        Debug.Log($"플레이어의 체력이 {value} 회복 되었습니다.");

        // 회복된 체력의 최소 0, 최대 Max HP 세팅
        _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
    }

    // Player collider Item
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한게 아이템이 맞는지 체크 (필수)
        if(other.gameObject.layer == 7)
        {
            _inventory.GetItem(other.GetComponent<ItemObject>().data);
            other.gameObject.SetActive(false);
        }
        
    }

    // Player Item Use
    private void ItemUse()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _inventory.UseItem(0);
        }
    }

    // Player Die
    public void PlayerDie()     // TODO : 플레이어 사망 구현 예정
    {
        Debug.Log("You Die");
        gameObject.SetActive(false);
        // 게임 오버 씬 전환
    }

    //public void OnShoot()
    private void HandleShooting()
    {
        //if (Input.GetKey(_shootKey) && _status.IsAiming.Value)
        //if(_status.IsAiming.Value)
        if(_status.IsAiming.Value && _shootInputAction.IsPressed())
        {
            _status.IsAttacking.Value = _gunController.Shoot();
        }
        else
        {
            _status.IsAttacking.Value = false;
        }
    }

    public void SubscribeEvents()
    {
        // Current HP
        _status.CurrentHp.Subscribe(SetHpUIGuage);

        // IsMoving
        _status.IsMoving.Subscribe(SetRunAnimation);

        // IsAimiing
        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);

        // ISAttack
        _status.IsAttacking.Subscribe(SetAttackAnimation);

        // Input
        _aimInputAction.Enable();
        _aimInputAction.started += HandleAiming;
        _aimInputAction.canceled += HandleAiming;
    }

    public void CancelsubscribeEvents()
    {
        // Current HP
        _status.CurrentHp.Cancelsubscribe(SetHpUIGuage);

        // IsMoving
        _status.IsMoving.Cancelsubscribe(SetRunAnimation);

        // IsAiming
        _status.IsAiming.Cancelsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Cancelsubscribe(SetAimAnimation);

        // IsAttack
        _status.IsAttacking.Cancelsubscribe(SetAttackAnimation);

        // Input
        _aimInputAction.Disable();
        _aimInputAction.started -= HandleAiming;
        _aimInputAction.canceled -= HandleAiming;
    }

    private void SetAimAnimation(bool value)
    {
        if (!_image.enabled)
        {
            _image.enabled = true;
        }
        _animator.SetBool("IsAim", value);
        _aimAnimator.SetBool("IsAim", value);
    }

    private void SetRunAnimation(bool value)
    {
        _animator.SetBool("IsRun", value);
    }

    private void SetAttackAnimation(bool value)
    {
        _animator.SetBool("IsAttack", value);
    }

    private void SetHpUIGuage(int currentHp)
    {
        float hp = currentHp / (float)_status.MaxHp;
        _hpGuageUI.SetImageFillAmount(hp);
    }

}














