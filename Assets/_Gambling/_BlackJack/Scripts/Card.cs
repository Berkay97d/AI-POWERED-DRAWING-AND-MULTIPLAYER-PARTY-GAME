using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace BlackJack
{ 
    public class Card : MonoBehaviour
    {
        [SerializeField] private int value;
        
        private bool isFront = false;
        
        
        public void TurnFrontAnimaton()
        {
            transform.DORotate(new Vector3(transform.rotation.x, 180, transform.rotation.z), 1f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    isFront = true;
                });
        }

        private void TurnFrontInMoment()
        {
            isFront = true;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
        }
        
        public void SetParent(Transform parent, Vector3 pos, bool isOpen)
        {
            if (isOpen)
            {
                TurnFrontInMoment();
            }
            transform.DOMove(pos, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.parent = parent;
            });
        }

        public int GetValue()
        {
            return value;
        }

        public bool GetIsFront()
        {
            return isFront;
        }

        
    }
}
