using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using UnityEngine;

namespace UI
{
    public class BookVisual : MonoBehaviour
    {
        [SerializeField] private CollectionMenuController collectionMenuController;
        [SerializeField] private Transform bookCover;


        private Coroutine m_TurnRoutine;
        

        private void Start()
        {
            collectionMenuController.OnCollectionPageArrive += OnCollectionPageArrive;
            collectionMenuController.OnCollectionPageGone += OnCollectionPageGone;
        }

        private void OnDestroy()
        {
            collectionMenuController.OnCollectionPageArrive -= OnCollectionPageArrive;
            collectionMenuController.OnCollectionPageGone -= OnCollectionPageGone;
        }

        private void OnCollectionPageGone(object sender, EventArgs e)
        {
            CloseBookCover();
        }

        private void OnCollectionPageArrive(object sender, EventArgs e)
        {
            OpenBookCover();
        }

        private void OpenBookCover()
        {
            m_TurnRoutine = LazyCoroutines.WaitForSeconds(.75f, () =>
            {
                bookCover.DOLocalRotate(new Vector3(0, 0, 179), .3f)
                    .SetEase(Ease.InCubic);
            });
        }

        private void CloseBookCover()
        {
            if (m_TurnRoutine != null)
            {
                LazyCoroutines.StopCoroutine(m_TurnRoutine);
            }
            
            bookCover.DOLocalRotate(new Vector3(0, 0, 0), .3f);
        }
    }
}