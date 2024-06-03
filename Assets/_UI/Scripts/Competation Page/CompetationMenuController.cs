using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    

public class CompetationMenuController : MonoBehaviour
{
    [SerializeField] private Transform mainTransform;

    private GameMode selectedGameMode;
    
    
    private void Start()
    {
        GameModeController.Instance.OnModeSelected += OnModeSelected; //TODO COMPU SEÇİNCE
    }

    private void OnModeSelected(object sender, bool e)
    {
        if (e)
        {
            mainTransform.localScale = Vector3.zero;
            mainTransform.gameObject.SetActive(true);
            mainTransform.DOScale(Vector3.one, 0.35f);
        }
    }

    public void SetSelecetedGameMode(GameMode mode)
    {
        selectedGameMode = mode;
    }
    public GameMode GetSelectedGameMode()
    {
        return selectedGameMode;
    }
    
    public int GetPlayerCount()
    {
        return GetPlayerCount(selectedGameMode);
    }

    public static int GetPlayerCount(GameMode gameMode)
    {
        return gameMode switch
        {
            GameMode.Stack => 2,
            GameMode.Katana => 2,
            GameMode.FastEyes => 4,
            GameMode.Snow => 4,
            GameMode.DontFall => 8,
            GameMode.Laser => 8,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
}
}
