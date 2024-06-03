using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuPage : MonoBehaviour
    {
        [SerializeField] private Transform upPageTransform;
        [SerializeField] private Transform downPageTransform;
        [SerializeField] private Transform leftPageTransform;
        [SerializeField] private Transform rightPageTransform;
        [SerializeField] private Transform middlePageTransform;
        [SerializeField] private Transform mainTransform;
        [SerializeField] private BottomPageButton bottomPageButton;
        
        [SerializeField] private int pageNumber;
        
        
        public void MoveRight()
        {
            mainTransform.DOMove(rightPageTransform.position, .35f);
        }
        
        public void MoveLeft()
        {
            mainTransform.DOMove(leftPageTransform.position, .35f);
        }

        public void MoveMiddle()
        {
            mainTransform.DOMove(middlePageTransform.position, .35f);
        }
        
        public void MoveMiddleNoEase()
        {
            mainTransform.DOMove(middlePageTransform.position, .35f);
        }

        public void MoveDown()
        {
            mainTransform.DOMove(downPageTransform.position, .35f);
        }

        public void TakePositionAtRight()
        {
            mainTransform.position = rightPageTransform.position;
        }
        
        public void TakePositionAtLeft()
        {
            mainTransform.position = leftPageTransform.position;
        }

        public void TakePositionAtDown()
        {
            mainTransform.position = downPageTransform.position;
        }

        public void ButtonActive()
        {
            bottomPageButton.SetActiveImage();
        }

        public void ButtonDisactive()
        {
            bottomPageButton.SetPassiveImage();
        }

        public int GetPageNumber()
        {
            return pageNumber;
        }

        public Transform GetMiddlePageTransform()
        {
            return middlePageTransform;
        }

        public Transform GetUpPageTransform()
        {
            return upPageTransform;
        }

        public int GetMenuNumber()
        {
            return pageNumber;
        }
        
    }
}