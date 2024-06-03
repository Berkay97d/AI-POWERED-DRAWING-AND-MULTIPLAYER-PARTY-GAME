using System;
using System.Collections;
using System.Collections.Generic;
using _Compatetion.MemoryMatch;
using _SoundSystem.Scripts;
using General;
using UnityEngine;

public class MemoryMatchSoundInitilazor : MonoBehaviour
{
    [SerializeField] private MemoryMatchManager memoryMatchManager;

    private SoundManager soundManager;
    

    private void Awake()
    {
        soundManager = GetComponent<SoundManager>();
    }

    private void Start()
    {
        LazyCoroutines.WaitForSeconds(0.5f, () =>
        {
            memoryMatchManager.GetGame().OnGameOver += OnGameOver;
            memoryMatchManager.GetGame().OnPairMatch += OnOnPairMatch;
        });
    }

    private void OnOnPairMatch(object sender, bool e)
    {
        if (e)
        {
            soundManager.PlayMemoryMatchCorrectMatchSound();
        }
        else
        {
            soundManager.PlayMemoryMatchIncorrectMatchSound();
        }
    }

    private void OnGameOver(object sender, bool e)
    {
        if (e)
        {
            soundManager.PlayCreationWinSound();
        }
        else
        {
            soundManager.PlayCreationLoseSound();
        }
    }

    public void PlayCorrectMatchSound()
    {
        soundManager.PlayMemoryMatchCorrectMatchSound();
    }
    
    public void PlayIncorrectMatchSound()
    {
        soundManager.PlayMemoryMatchIncorrectMatchSound();
    }
    
}
