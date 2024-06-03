using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace FastEyes
{
    public class PlayerController : Contester
    {
        
        private new void Start()
        {
            base.Start();
            
            Target.OnPlayerClick += SpotOnOnPlayerClick;
        }

        public override void ThrowArrow(Target e)
        {
            if (isArrowShoted)
            {
                return;
            }
            RaiseOnArrowShooted();
            
            arrow.DOMove(e.transform.position, arrowArriveTime).OnComplete(() =>
            {
                e.InstatiateShotColor(this);
                if (e.GetIsDifferent())
                {
                    RaiseShowInfoEvent(true);
                    Target.IncreaseTotalShotNumber();
                }
                else
                {
                    RaiseShowInfoEvent(false);
                }
                RaiseOnArrowArrivedTarget(e);
            });
            arrow.SetParent(e.transform);
            isArrowShoted = true;
        }

        private void OnDestroy()
        {
            Target.OnPlayerClick -= SpotOnOnPlayerClick;
        }

        private void SpotOnOnPlayerClick(object sender, Target e)
        {
            ThrowArrow(e);
        }

        
        
        
        
    }
}