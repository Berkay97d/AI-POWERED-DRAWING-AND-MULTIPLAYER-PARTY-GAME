using System;
using System.Collections;
using System.Collections.Generic;
using _SoundSystem.Scripts;
using General;
using MakeItLonger;
using UnityEngine;

public class MakeItLongerSoundInitilazor : MonoBehaviour
{
    private SoundManager soundManager;
    private bool flag = false;

    private static MakeItLongerSoundInitilazor Instance; 
    
    [SerializeField] private GameController gameController;
    
    
    
    private void Awake()
    {
        Instance = this;
        
        soundManager = GetComponent<SoundManager>();
        
        gameController.OnGameStateChanged += OnGameStateChanged;
        gameController.OnRoundOver += OnRoundOver;
        InputObject.OnPlayerClick += OnPlayerClick;
    }

    private void OnRoundOver(object sender, OnRoundOverArgs e)
    {
        if (e.WinnerContester is Player)
        {
            soundManager.PlayCompetitionTourWinSound();
        }
        else
        {
            soundManager.PlayCompetitionTourLoseSound();
        }
    }

    private void OnDestroy()
    {
        gameController.OnGameStateChanged -= OnGameStateChanged;
        InputObject.OnPlayerClick -= OnPlayerClick;
    }

    private void OnPlayerClick(object sender, EventArgs e)
    {
        soundManager.PlayMakeItLongerAddingBoxSound();
    }

    private void OnGameStateChanged(object sender, GameState e)
    {
        if (e == GameState.CountdownToStart && !flag)
        {
            flag = true;

            LazyCoroutines.WaitForSeconds(3f, () =>
            {
                flag = false;
            });
            
            return;
        }

        if (e == GameState.GameOver)
        {
            if (gameController.GetWinner() is Player)
            {
                soundManager.PlayCompetitionWinSound();
            }

            else
            {
                soundManager.PlayCompetitionLoseSound();
            }
            
            return;
        }
    }

    public static void PlayBoxDropSound()
    {
        Instance.soundManager.PlayMakeItLongerBoxDroppingSound();
    }
}
