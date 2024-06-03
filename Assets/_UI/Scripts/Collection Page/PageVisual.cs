using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PageVisual : MonoBehaviour
{
    private Quaternion originalRotation;
    private bool isOriginalPos = true;

    private Tween pageTween;
    
    private void Start()
    {
        originalRotation = transform.rotation;
    }

    public void TurnPageLeft()
    {
        pageTween = transform.DOLocalRotate(new Vector3(0, 0, 179), .3f)
            .SetEase(Ease.InCubic);

        isOriginalPos = false;
    }
    
    public void TurnPageRight()
    {
        pageTween.Kill();
        
        transform.DOLocalRotate(new Vector3(0, 0, 0), .3f)
            .SetEase(Ease.OutCubic);
        
        isOriginalPos = true;
    }

    public void GoOriginalPos()
    {
        pageTween.Kill();
        transform.rotation = originalRotation;
    }

    public bool GetIsOriginalPos()
    {
        return isOriginalPos;
    }
}
