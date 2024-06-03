using System;
using System.Collections;
using System.Collections.Generic;
using _Creation.SwapMatch.Scripts;
using _SoundSystem.Scripts;
using General;
using UnityEngine;

public class SwapMatchSoundInitilazor : MonoBehaviour
{
    [SerializeField] private SwapMatchUI swapMatchUI;
    [SerializeField] private SwapMatchManager swapMatchManager;
    [SerializeField] private SwapMatchAI ai;

    private static SwapMatchSoundInitilazor Instance;
    private SoundManager soundManager;
    

    private void Awake()
    {
        Instance = this;
        soundManager = GetComponent<SoundManager>();
        
        ai.OnScrambled += OnScrambled;
    }

    private void OnScrambled()
    {
        soundManager.PlayCreationSkillSound();
    }

    private void Start()
    {
        swapMatchUI.OnFreezeButtonClick += OnFreezeButtonClick;    
        swapMatchManager.OnGameOver += OnGameOver;
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

    private void OnFreezeButtonClick(object sender, EventArgs e)
    {
        LazyCoroutines.WaitForSeconds(4.75f, () =>
        {
            soundManager.PlayPuzzleIceBreaking();
        });
    }

    public static void PlaySlideSound()
    {
        Instance.soundManager.PlayPuzzleSlideSound();
    }

    
}
