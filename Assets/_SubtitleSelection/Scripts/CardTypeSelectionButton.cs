using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using MakeItLonger;
using UnityEngine;
using UnityEngine.UI;

public class OnCardTypeSelectionButtonClickEventArgs
{
    public OnCardTypeSelectionButtonClickEventArgs(bool isOpened, CardTypeSelectionButton cardTypeSelectionButton)
    {
        this.isOpened = isOpened;
        this.cardTypeSelectionButton = cardTypeSelectionButton;
    }
    
    public bool isOpened;
    public CardTypeSelectionButton cardTypeSelectionButton;
}

public abstract class CardTypeSelectionButton : MonoBehaviour
{
    public Action<string> OnSelected;
    
    
    private Button button;
    private bool isOpen;
    private bool isSelected;
    public event EventHandler<OnCardTypeSelectionButtonClickEventArgs> OnCardTypeSelectionButtonClick; 
    private bool isİnteractable = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TakeOnClickAction);
    }

    protected void TakeOnClickAction()
    {
        OnCardTypeSelectionButtonClick?.Invoke(this, new OnCardTypeSelectionButtonClickEventArgs(!isOpen, this));

        BeUnInteractable();
        
        LazyCoroutines.WaitForSeconds(.5f, () =>
        {
            if (!isOpen)
            {   
                GetOpen();
                SetIsOpen(true);
                return;
            }
            
            GetClose();
            SetIsOpen(false);
        });
    }
    
    protected abstract void GetOpen();

    protected abstract void GetClose();

    protected void GoLeft()
    {
        var targetPos = transform.position;
        targetPos.x = -4;

        transform.DOMove(targetPos, .2f);
    }

    protected void GoRight()
    {
        LazyCoroutines.WaitForSeconds(.8f, () =>
        {
            var targetPos = transform.position;
            targetPos.x = 0;

            transform.DOMove(targetPos, .2f);

        });
    }
    
    private void SetIsClickable(bool i)
    {
        button.enabled = i;
    }

    private void SetIsInteractable(bool i)
    {
        button.interactable = i;
    }
    
    private void SetIsOpen(bool i)
    {
        isOpen = i;
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }

    protected void SetIsSelected(bool i)
    {
        isSelected = i;
    }
    
    protected void BeUnInteractable()
    {
        if (button.enabled)
        {
            button.enabled = !isİnteractable;
            isİnteractable = !isİnteractable;
        }
        else
        {
            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                button.enabled = !isİnteractable;
                isİnteractable = !isİnteractable;
            });
        }
    }

}
