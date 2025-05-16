using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] float _attackRange;
    [SerializeField] int _shootDamage;
    [SerializeField] float _shootDelay;
    [SerializeField] AudioClip _shootSFX;

    CinemachineImpulseSource _impulse;
    Camera _camera;

    bool _canShoot { get => _currentCount <= 0; }
    float _currentCount;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        HandleCanShoot();
    } 

    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    public bool Shoot()
    {
        if (!_canShoot)
        {
            return false;
        }

        PlayShootSound();
        PlayCameraEffect();
        PlayShootEffect();
        _currentCount = _shootDelay;

        // TODO : Ray 발사 -> 반환받은 대상에게 데미지 부여. 몬스터 구현시 같이 구현
        GameObject target = RayShoot();
        if (target == null)
        {
            return true;
        } 

        Debug.Log($"{target.name} 이/가 총에 맞음");
        //-------

        return true;
    }

    private void HandleCanShoot()
    {
        if (_canShoot)
        {
            return;
        }

        _currentCount -= Time.deltaTime;
    }

    private GameObject RayShoot()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
        {
            // TODO: 몬스터를 어떻게 구현하는가에 따라 다름.
            return hit.transform.gameObject;
            //-----------------
        }

        return null;
    }

    private void PlayShootSound()
    {
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }

    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }

    private void PlayShootEffect()
    {
        // TODO: 총구 화염 효과. 파티클로 구현해보기기
    }

}
