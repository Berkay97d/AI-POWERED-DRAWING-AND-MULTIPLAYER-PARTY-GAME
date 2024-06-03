using System;
using System.Collections;
using System.Collections.Generic;
using _SoundSystem.Scripts;
using DG.Tweening;
using General;
using UnityEngine;

namespace DontFall
{
    public class Tile : MonoBehaviour
    {
        private Vector3 gridPosition;
        private bool isGoingDown = false;
        private Tween tween;
        private bool isInDangerZone = false;

        private void Start()
        {
            gridPosition = transform.position;
        }

        public void GoDown()
        {
            if (isGoingDown) return;

            tween = transform.DOMove(Vector3.up * - 10, 1f);
            DontFallSoundInitilazor.OnTilesMove();
            isGoingDown = true;
        }

        public void GoUp()
        {
            tween.Kill();
            transform.DOMove(gridPosition, 1f).SetEase(Ease.OutBack);
            isGoingDown = false;
        }

        public bool GetIsInDangerZone()
        {
            return isInDangerZone;
        }

        public void SetIsInDangerZone(bool isIn)
        {
            isInDangerZone = isIn;
        }


    }
}