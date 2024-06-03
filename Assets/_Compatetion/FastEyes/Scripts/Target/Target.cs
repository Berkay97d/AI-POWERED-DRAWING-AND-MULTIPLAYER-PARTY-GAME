using System;
using UnityEngine;
using UnityEngine.UI;

namespace FastEyes
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] shotSprites;
        
        public static event EventHandler<Target> OnPlayerClick;
        
        private bool isDifferent = false;
        public static int TotalShotOnTargets = 0;

        public void InstatiateShotColor(Contester contester)
        {
            shotSprites[TotalShotOnTargets].color = contester.GetColor();
        }

        private void InstantiateTargetColor(Color color)
        {
            foreach (var sprite in shotSprites)
            {
                sprite.gameObject.SetActive(true);
                sprite.color = color;
            }
        }
        
        public void CloseTargetColor( )
        {
            foreach (var sprite in shotSprites)
            {
                sprite.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            TargetGroupMover.Instance.OnNewTargetGroupSpawned += OnNewTargetGroupSpawned;
            TargetAdjuster.OnColorOpened += OnColorOpened;
        }

        private void OnDestroy()
        {
            TargetGroupMover.Instance.OnNewTargetGroupSpawned -= OnNewTargetGroupSpawned;
            TargetAdjuster.OnColorOpened -= OnColorOpened;
        }

        private void OnColorOpened(object sender, Color e)
        {
            InstantiateTargetColor(e);
        }

        private void OnNewTargetGroupSpawned(object sender, EventArgs e)
        {
            ResetTotalShotNumber();
            CloseTargetColor();
        }

        private void OnMouseDown()
        {
            if (!TargetAdjuster.Instance.GetIsShootable())
            {
                return;
            }
            
            RaiseOnPlayerClick();
        }

        private void RaiseOnPlayerClick()
        {
            OnPlayerClick?.Invoke(this, this);
        }

        public bool GetIsDifferent()
        {
            return isDifferent;
        }

        public void SetIsDifferent(bool isDif)
        {
            isDifferent = isDif;
        }

        public static void IncreaseTotalShotNumber()
        {
            TotalShotOnTargets++;
        }

        public static void ResetTotalShotNumber()
        {
            TotalShotOnTargets = 0;
        }

        public static int GetTotalShotNumber()
        {
            return TotalShotOnTargets;
        }
    }
}