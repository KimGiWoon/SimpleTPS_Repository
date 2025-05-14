using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement _movement;
    public PlayerStatus _status;

    private void Update()
    {
        MoveTest();

        // IsAiming 변경용 테스트 코드
        _status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1);
    }

    public void MoveTest()
    {
        // 좌우 회전에 대한 벡터 반환
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
        _status.IsMoving.Value = (moveDir != Vector3.zero);     // 움직이는지 확인

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
    }
}
