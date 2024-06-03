using System;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using FastEyes;
using UnityEngine;

namespace _SoundSystem.Scripts
{
    public class FastEyesSoundInitilazor : MonoBehaviour
    {
        private SoundManager soundManager;
        private bool flag = false;
        private static FastEyesSoundInitilazor Instance;
        
        [SerializeField] private GameController gameController;
        [SerializeField] private Contester[] contesters;
        
        
        private void Awake()
        {
            Instance = this;
            
            soundManager = GetComponent<SoundManager>();
            gameController.OnGameStateChanged += OnGameStateChanged;

            foreach (var contester in contesters)
            {
                contester.OnArrowShooted += OnArrowShooted;
                contester.OnArrowArrivedTarget += OnArrowArrivedTarget;
            }
        }

        private void OnDestroy()
        {
            gameController.OnGameStateChanged -= OnGameStateChanged;

            foreach (var contester in contesters)
            {
                contester.OnArrowShooted -= OnArrowShooted;
                contester.OnArrowArrivedTarget -= OnArrowArrivedTarget;
            }
        }

        private void OnArrowArrivedTarget(object sender, Target e)
        {
            soundManager.PlayFastEyesBoomBulletSound();
        }

        private void OnArrowShooted(object sender, EventArgs e)
        {
            soundManager.PlayFastEyesShootingSound();
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.CountdownToStart)
            {
                return;
            }

            if (e == GameState.GameOver && gameController.GetWinner() is PlayerController && !flag)
            {
                LocalUser.AddQuestProgress(QuestType.Win_Competition);
                LocalUser.AddQuestProgress(QuestType.Win_FastEye);
                
                flag = true;
                
                soundManager.PlayCompetitionWinSound();
                return;
            }
            
            if (e == GameState.GameOver && gameController.GetWinner() is not PlayerController && !flag)
            {
                flag = true;
                
                soundManager.PlayCompetitionLoseSound();
                return;
            }
        }

        public static void PlayTargetGroupMoveSound()
        {
            Instance.soundManager.PlayFastEyesSkippingTargetSound();
        }
    }
}