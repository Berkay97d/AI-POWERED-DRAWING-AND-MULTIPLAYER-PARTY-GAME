using System;
using System.Collections;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using MakeItLonger;
using TMPro;
using UI;
using UnityEngine;

public class CraftPageController : MonoBehaviour
{
    [SerializeField] private TMP_Text[] tokenAmountTexts;
    

    private void Awake()
    {
        LocalUser.OnUserDataLoaded += OnUserDataLoaded;
        LocalUser.OnUserDataChanged += OnUserDataChanged;
    }

    

    private void OnDestroy()
    {
        LocalUser.OnUserDataLoaded -= OnUserDataLoaded;
        LocalUser.OnUserDataChanged -= OnUserDataChanged;
    }
    
    
    
    private void OnUserDataChanged(UserData obj)
    {
        UpdateTokenAmounts(obj.upgradeTokens);
    }

    private void OnUserDataLoaded(UserData obj)
    {
        UpdateTokenAmounts(obj.upgradeTokens);
    }
    
    private void UpdateTokenAmounts(int[] tokenValues)
    {
        for (int i = 0; i < tokenAmountTexts.Length; i++)
        {
            tokenAmountTexts[i].text = tokenValues[i].ToString();
        }
    }

   

    
    
    
    
    
}
