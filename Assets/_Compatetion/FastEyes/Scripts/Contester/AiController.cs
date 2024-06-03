using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FastEyes
{
    public enum AILevel
    {
        Low,
        LowMid,
        Mid,
        MidHigh,
        High
    }
    
    public class AiController : Contester
    {
        [SerializeField] private AiStatSO aiStat;
        [SerializeField] private AILevel aiLevel;

        private float currentWaitedTime;
        private float waitTime;
        private bool canShoot = false;

        private new void Start()
        {
            base.Start();
            
            TargetAdjuster.Instance.OnTargetsBecameShotable += OnTargetsBecameShotable;
        }

        private void Update()
        {
            if(!canShoot) return;

            currentWaitedTime += Time.deltaTime;

            if (currentWaitedTime > waitTime)
            {
                if (aiStat.GetSuccess(aiLevel))
                {
                    ThrowArrow(TargetAdjuster.Instance.GetDifferentTarget());
                }
                else
                {
                    ThrowArrow(TargetAdjuster.Instance.GetRandomTarget());
                }
                canShoot = false;
                currentWaitedTime = 0;
            }
            

        }

        private void OnTargetsBecameShotable(object sender, EventArgs e)
        {
            canShoot = true;
            waitTime = aiStat.GetRandomReactionTime(aiLevel);
        }

        public override void ThrowArrow(Target e)
        {
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
    }
}