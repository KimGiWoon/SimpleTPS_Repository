using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _bgmSource;

    //private ObjectPool<PlayerStatus> _pool = new();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();
    }

    public void BgmPlay()
    {
        _bgmSource.Play();
    }
}
