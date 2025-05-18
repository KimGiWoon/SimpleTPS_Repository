using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    bool IsControlActivate { get; set; } = true;

    PlayerStatus _status;
    PlayerMovement _movement;
    Animator _animator;
    Image _image;

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
        HandleAiming();
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

        // SetAnimation Parameter
        if (_status.IsAiming.Value)
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

    // Player Take Damage
    public void TakeDamage(int value)
    {
        _status.CurrentHp.Value -= value;

        if (_status.CurrentHp.Value <= 0)
        {
            PlayerDie();
        } 
    }
    
    // Player Health Recovery
    public void RecoveryHp(int value)
    {
        int hp = _status.CurrentHp.Value + value;

        // 회복된 체력의 최소 0, 최대 Max HP 세팅
        _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
    }

    // Player Die
    public void PlayerDie()
    {
        Debug.Log("You Die");
        gameObject.SetActive(false);
        // 게임 오버 씬 전환
    }


    private void HandleShooting()
    {
        if (Input.GetKey(_shootKey) && _status.IsAiming.Value)
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














