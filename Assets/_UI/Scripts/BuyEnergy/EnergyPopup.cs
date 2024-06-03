using System;
using System.Collections;
using System.Collections.Generic;
using _UserOperations.Scripts;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPopup : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyWithAddButton;
    [SerializeField] private Button buyWithGemButton;
    [SerializeField] private TMP_Text withAddEnergyAmountText;
    [SerializeField] private TMP_Text withGemGemCountText;
    [SerializeField] private Transform popupTransform;
    [SerializeField] private Transform mainTransform;

    private int withAddEnergyAmount = 10;
    private int withGemGemCount = 25;
    private int maxEnergyCount = 50;
    private bool isPopupOpen = false;

    private void Start()
    {
        SetBuyAmounths();
        
        openButton.onClick.AddListener(OpenPopUp);
        
        closeButton.onClick.AddListener(ClosePopUp);
        
        buyWithAddButton.onClick.AddListener(() =>
        {
            LocalUser.AddEnergy(withAddEnergyAmount);
            ClosePopUp();
        });
        
        buyWithGemButton.onClick.AddListener(() =>
        {
            LocalUser.TryRemoveGem(withGemGemCount, () =>
            {
                LocalUser.AddEnergy(maxEnergyCount);
            });
            
            ClosePopUp();
            
        });
    }
    

    private void OpenPopUp()
    {
        if (!MenuManager.IsMainMenuActive()) return;
        if (isPopupOpen) return;

        popupTransform.localScale = Vector3.zero;
        
        mainTransform.gameObject.SetActive(true);

        popupTransform.DOScale(Vector3.one * 1.4f, .35f).SetEase(Ease.OutBack);

        isPopupOpen = true;
    }

    private void ClosePopUp()
    {
        popupTransform.DOScale(Vector3.zero, .35f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                mainTransform.gameObject.SetActive(false);
                isPopupOpen = false;
            });
        
    }

    private void SetBuyAmounths()
    {
        withAddEnergyAmountText.text = withAddEnergyAmount.ToString();
        withGemGemCountText.text = withGemGemCount.ToString();
    }
    
}
