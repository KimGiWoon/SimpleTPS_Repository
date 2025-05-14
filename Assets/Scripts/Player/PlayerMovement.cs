using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _avatar;
    [SerializeField] Transform _aim;

    Rigidbody _playerRigid;
    Playerstatus _playerStatus;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] float _minPitch;    // ī�޶� �ּ� ����
    [SerializeField][Range(0, 90)] float _maxPitch;     // ī�޶� �ִ� ����
    [SerializeField][Range(0, 5)] float _mouseSensitivity = 1;

    private void Awake()
    {
        Initial();
    }

    private void Update()
    {
        SetMove(_playerStatus.WalkSpeed);
        SetAimRotation();
        SetBodyRotation();
    }

    private void Initial()
    {
        _playerRigid = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<Playerstatus>();
    }

    public Vector3 SetMove(float moveSpeed)
    {
        Vector3 moveDirection = GetMoveDirection();
        Vector3 velocity = _playerRigid.velocity;

        velocity.x = moveDirection.x * moveSpeed * Time.deltaTime;
        velocity.z = moveDirection.z * moveSpeed * Time.deltaTime;

        _playerRigid.velocity = velocity;

        return moveDirection;
    }

    public Vector3 SetAimRotation()
    {
        Vector2 mouseDir = GetMouseDirection();

        Vector2 currentRotation = new()     // ���� ���� ����
        {
            x = transform.rotation.eulerAngles.x,
            y = transform.rotation.eulerAngles.y
        };

        currentRotation.x = mouseDir.x;     // x���� ���� ������ �� �ʿ䰡 ����
        currentRotation.y = Mathf.Clamp(currentRotation.y + mouseDir.y, _minPitch, _maxPitch);  // Clamp�� min, max ������ ������ ����

        // ĳ���� ������Ʈ�� Y�� ȸ���� �ݿ�
        transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);

        // ������ ��ȭ ȸ�� �ݿ�
        Vector3 currentEuler = _aim.localEulerAngles;
        _aim.localEulerAngles = new Vector3(currentRotation.x, currentEuler.y, currentEuler.z);

        // ȸ�� ���� ���� ��ȯ
        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;
    }

    public void SetBodyRotation()
    {

    }

    public Vector3 GetMoveDirection()
    {
        Vector3 input = GetInputDirection();

        Vector3 direction = (transform.right * input.x) + (transform.forward * input.z);

        return direction.normalized;
    }

    // Player Move Input
    public Vector3 GetInputDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        return new Vector3(x, 0, z);
    }

    // Mouse move Input
    private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

        return new Vector2(mouseX, mouseY);
    }
}
