using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FastEyes
{
    public abstract class Contester : MonoBehaviour , IComparable<Contester>
    {
        [SerializeField] protected Transform arrow;
        [SerializeField] protected Transform arrowParent;
        [SerializeField] protected float arrowArriveTime;
        [SerializeField] private PlayerInfo info;
        [SerializeField] private Color color;
        
        
        public event EventHandler<Target> OnArrowArrivedTarget;
        public event EventHandler OnArrowShooted;
        public event EventHandler<bool> ShowInfoEvent;
        
        protected bool isArrowShoted = false;

        private int score;
        private int index;


        public void RaiseShowInfoEvent(bool isHit)
        {
            ShowInfoEvent?.Invoke(this, isHit);
        }

        protected void Start()
        {
            TargetGroupMover.Instance.OnNewTargetGroupSpawned += OnNewTargetGroupSpawned;
            OnArrowArrivedTarget += InternalOnArrowArrivedTarget;
        }


        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int _index)
        {
            index = _index;
        }
        
        private void InternalOnArrowArrivedTarget(object sender, Target e)
        {
            if (e.GetIsDifferent())
            {
                EarnScore(Target.GetTotalShotNumber());
                info.UpdateScoreText();
            }
            
            arrow.gameObject.SetActive(false);
        }

        private void OnNewTargetGroupSpawned(object sender, EventArgs e)
        {
            GiveArrowsBack();
        }
        
        private void GiveArrowsBack()
        {
            arrow.gameObject.SetActive(true);
            arrow.SetParent(arrowParent);
            arrow.localPosition = Vector3.zero;
            isArrowShoted = false;
        }

        public abstract void ThrowArrow(Target e);

        public bool GetIsArrowShooted()
        {
            return isArrowShoted;
        }

        protected void EarnScore(int grade)
        {
            switch (grade)
            {
                case 1: 
                    score += 4;
                    break;
                case 2: 
                    score += 3;
                    break;
                case 3: 
                    score += 2;
                    break;
                case 4: 
                    score += 1;
                    break;
                
            }
        }

        protected void RaiseOnArrowArrivedTarget(Target target)
        {
            OnArrowArrivedTarget?.Invoke(this, target);
        }

        protected void RaiseOnArrowShooted()
        {
            OnArrowShooted?.Invoke(this, EventArgs.Empty);
        }

        public int GetScore()
        {
            return score;
        }

        public Color GetColor()
        {
            return color;
        }


        public int CompareTo(Contester other)
        {
            if (score > other.GetScore())
            {
                return 1;
            }
            if (score == other.GetScore())
            {
                if (this is PlayerController)
                {
                    return 1;
                }

                return -1;
            }
            
            return -1;
            

        }
    }
}