using System;
using System.Collections;
using System.Collections.Generic;
using _Painting;
using _PaintToCard.Scripts;
using _SubtitleSelection.Scripts;
using General;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private StartPaintingButton startPaintingButton;
    

    private void Start()
    {
        startPaintingButton.OnPaintingStart += OnPaintingStart;
    }

    private void OnDestroy()
    {
        startPaintingButton.OnPaintingStart -= OnPaintingStart;
    }

    private void OnPaintingStart(object sender, string e)
    {
        LazyCoroutines.WaitForSeconds(.1f, () =>
        {
            PaintManager.SetPrompt(e);
        });
    }

    public string GetSubtitle()
    {
        return startPaintingButton.GetSubTitle();
    }

    
}
