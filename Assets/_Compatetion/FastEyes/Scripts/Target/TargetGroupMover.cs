using System;
using _SoundSystem.Scripts;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FastEyes
{
    public class TargetGroupMover : MonoBehaviour
    {
        [SerializeField] [Foldout("Values")] private float maxWaitTime;
        [SerializeField] [Foldout("Values")] private float sendXPos;
        [SerializeField] [Foldout("Values")] private float comeXPos;
        [SerializeField] [Foldout("Values")] private float sendTime;
        [SerializeField] [Foldout("Values")] private float comeTime;

        [SerializeField] private Contester[] contesters;
        
        public event EventHandler OnNewTargetGroupSpawned;
        public static TargetGroupMover Instance;
        
        private float waitTime;
        private Vector3 originalPos;
        private bool isGroupSended = false;

        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            originalPos = transform.position;
        }

        private void Update()
        {
            if (!GameController.Instance.GetIsGameStarted()) return;

            OnAllPlayerArrowCameBack();

            if (GameController.Instance.IsTargetLimitFull()) return;
            SendGroupInTime();
        }

        private void SendGroupInTime()
        {
            if (CheckIsAllPlayersShot())
            {
                isGroupSended = true;
                waitTime = maxWaitTime - 1f;
            }
            
            waitTime += Time.deltaTime;

            if (waitTime > maxWaitTime)
            {
                SendTargetGroup();
                waitTime = 0;
            }

            CheckIsAllPlayersShot();
        }

        private bool CheckIsAllPlayersShot()
        {
            if (isGroupSended)
            {
                return false;
            }
            
            foreach (var player in contesters)
            {
                if (!player.GetIsArrowShooted())
                {
                    return false;
                }
            }

            return true;
        }
        
        private void SendTargetGroup()
        {
            TargetAdjuster.Instance.SetIsShootable(false);
            
            FastEyesSoundInitilazor.PlayTargetGroupMoveSound();
            
            var pos = new Vector3(sendXPos, originalPos.y, originalPos.z);
            transform.DOMove(pos, sendTime).OnComplete(() =>
            {
                OnNewTargetGroupSpawned?.Invoke(this, EventArgs.Empty);
                transform.position = new Vector3(comeXPos, originalPos.y, originalPos.z);
                transform.DOMove(originalPos, comeTime).SetEase(Ease.OutBack);
                
                FastEyesSoundInitilazor.PlayTargetGroupMoveSound();
            });
        }

        private void OnAllPlayerArrowCameBack()
        {
            foreach (var player in contesters)
            {
                if (player.GetIsArrowShooted())
                {
                    return;
                }
            }

            isGroupSended = false;
        }

    }
}