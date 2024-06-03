using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DailyCardGenerationVisual : MonoBehaviour
{
    [SerializeField] private Transform mainTransform;
    [SerializeField] private DailyCardGenerationController dailyCardGenerationController;


    private void Start()
    {
        dailyCardGenerationController.OnDailyGenerationActivated += OnDailyGenerationActivated;
        dailyCardGenerationController.OnDailyGenerationDisable += OnDailyGenerationDisable;
    }

    private void OnDestroy()
    {
        dailyCardGenerationController.OnDailyGenerationActivated -= OnDailyGenerationActivated;
        dailyCardGenerationController.OnDailyGenerationDisable -= OnDailyGenerationDisable;
    }

    private void OnDailyGenerationDisable(object sender, EventArgs e)
    {
        CloseGenerationPopUp();
    }

    private void OnDailyGenerationActivated(object sender, EventArgs e)
    {
        OpenGenerationPopUp();
    }

    private void OpenGenerationPopUp()
    {
        mainTransform.localScale = Vector3.zero;
        mainTransform.gameObject.SetActive(true);
        mainTransform.DOScale(Vector3.one , 0.25f).SetEase(Ease.OutBack);
    }

    private void CloseGenerationPopUp()
    {
        mainTransform.DOScale(Vector3.zero, 0.25f).OnComplete((() =>
        {
            mainTransform.gameObject.SetActive(false);
            mainTransform.localScale = Vector3.one;
        }));
    }
}
