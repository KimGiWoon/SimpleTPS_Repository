using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine;

public class SFXController : PooledObject
{
    AudioSource _audioSource;
    float _currentCount;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _currentCount -= Time.deltaTime;

        if(_currentCount <= 0)
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            ReturnPool();
        }
    }

    public void Play(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        _currentCount = clip.length;
    }
}
