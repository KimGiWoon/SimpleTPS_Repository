using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource _bgmSource;
    ObjectPool _sfxPool;

    [SerializeField] List<AudioClip> _bgmList = new();
    [SerializeField] SFXController _sfxPrefab;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();
        _sfxPool = new ObjectPool(transform, _sfxPrefab, 10);
    }

    public void BgmPlay(int index)
    {
        if(0 <= index && index < _bgmList.Count)
        {
            _bgmSource.Stop();
            _bgmSource.clip = _bgmList[index];
            _bgmSource.Play();
        }
    }

    public SFXController GetSFX()
    {
        // Pool ���� Pop�ϰ� ��ȯ
        // SFXController sfx = _sfxPool.PopPool() as SFXController; �� ����
        PooledObject poolObj = _sfxPool.PopPool();
        return poolObj as SFXController;    // SFXŸ������ ��ȯ�Ͽ� ��ȯ
    }
}
