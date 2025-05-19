using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;
using TMPro;

public class NormalMonster : Monster, IDamagable
{
    bool _isActivateControl = true;
    bool _canTracking = true;

    [SerializeField] int monsterCurrentHp;

    ObservableProperty<int> CurrentHp;
    ObservableProperty<bool> IsMoving = new();  // 몬스터의 애니메이션이나 무빙 상태에 대해서 이벤트 적용 시 사용
    ObservableProperty<bool> IsAttacking = new();

    NavMeshAgent _navMeshAgent;
    [SerializeField] Transform _targetTransform;

    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        HandleControl();
    }

    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;
    }

    private void HandleControl()
    {
        if (!_isActivateControl)
        {
            return;
        }
        HandleMove();
    }

    private void HandleMove()
    {
        if (_targetTransform == null)
        {
            return;
        }

        // 몬스터의 플레이어 추적 조건
        if (_canTracking)
        {
            _navMeshAgent.SetDestination(_targetTransform.position);
        }

        _navMeshAgent.isStopped = !_canTracking;
        IsMoving.Value = _canTracking;
        
    }

    private void MonsterDie()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(int value)
    {
        // 데미지 판정 구현
        // 체력 깎고
        // 체력이 0 이하가 되면 Dead 처리
        Debug.Log($"{gameObject.name} : {value} 데미지 받음");
        monsterCurrentHp -= value;
        Debug.Log($"몬스터 남은 체력 : {monsterCurrentHp}");
        if(monsterCurrentHp <= 0)
        {
            MonsterDie();
            Debug.Log("몬스터가 죽었습니다.");
        }
    }
}
