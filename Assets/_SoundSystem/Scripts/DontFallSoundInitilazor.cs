using System;
using DontFall;
using General;
using UnityEngine;

namespace _SoundSystem.Scripts
{
    public class DontFallSoundInitilazor : MonoBehaviour
    {
        private SoundManager soundManager;
        private static DontFallSoundInitilazor Instance;
        private static bool flag = false;
        
        [SerializeField] private KillDetector kill;
        [SerializeField] private AreaController areaController;
        


        private void Awake()
        {
            Instance = this;
            
            soundManager = GetComponent<SoundManager>();
            
            kill.OnContesterDied += OnContesterDied;
            areaController.OnWarningShake += OnWarningShake;
            
        }

        public static void OnTilesMove()
        {
            if (!flag)
            {
                Instance.soundManager.PlayDontFallBrokingSound();
                flag = true;
                
                LazyCoroutines.WaitForSeconds(3f,() =>
                {
                    flag = false;
                });
            }
        }

        private void OnWarningShake(object sender, EventArgs e)
        {
            soundManager.PlayDontFallShakingSound();
        }
        
        private void OnContesterDied(object sender, EventArgs e)
        {
            soundManager.PlayDontFallDeadSound();
        }

        private void Start()
        {
            soundManager.PlayDontFallLavaSound();
        }
    }
}