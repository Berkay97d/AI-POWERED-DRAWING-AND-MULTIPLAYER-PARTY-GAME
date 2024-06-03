using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FastBetButton : MonoBehaviour
{
    [SerializeField] private int betAmount;
    [SerializeField] private TMP_Text betAmountText;
    
    
    public event EventHandler<int> OnFastBetButtonClick; 

    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();

        betAmountText.text = betAmount.ToString();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            OnFastBetButtonClick?.Invoke(this, betAmount);
        });
    }
}
