using System;
using System.Collections;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using MakeItLonger;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class Page : MonoBehaviour
    {
        [SerializeField] private PageVisual visual;
        [SerializeField] private CollectionPageArrow[] collectionPageArrows;

        protected BookSubTitle subTitle = new BookSubTitle();
        

        public PageVisual GetVisual()
        {
            return visual;
        }
        
        public CollectionPageArrow[] GetCollectionPageArrows()
        {
            return collectionPageArrows;
        }

        public void SetSubtitle(BookSubTitle s)
        {
            subTitle = s;
        }

        public BookSubTitle GetSubTitle()
        {
            return subTitle;
        }

        public void SetInteractableArrowButtons(bool value)
        {
            foreach (var pageArrow in collectionPageArrows)
            {
                pageArrow.SetInteractable(value);
            }
        }
    }
}