using UnityEngine;

namespace _SoundSystem.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        
        [SerializeField] private GameSoundSO gameSounds;
        

        private void Awake()
        {
            Instance = this;
        }


        public void PlayLaserLaserSound()
        {
            Play(gameSounds.laserLaserSound);
        }


        public void PlayLaserLaserDead()
        {
            Play(gameSounds.laserDead);
        }


        public void PlayLaserJump()
        {
            Play(gameSounds.laserJump);
        }


        public void PlayKatanaCutSound()
        {
            Play(gameSounds.cut);
        }


        public void PlayKatanaCorrectSliceSound()
        {
            Play(gameSounds.correctSlice);
        }


        public void PlayKatanaBackgroundSound()
        {
            Play(gameSounds.katanaBackground);
        }


        public void PlayMenuClassicButtonSound()
        {
            Play(gameSounds.menuClassicButton);
        }


        public void PlaySnowbuttSnowParticleSound()
        {
            Play(gameSounds.snow);
        }


        public void PlaySnowbuttBooomParticleSound()
        {
            Play(gameSounds.booomParticle);
        }


        public void PlayMenuPlayButtonSound()
        {
            Play(gameSounds.menuPlayButton);
        }
        
        
        public void PlayMenuTurnPageSound()
        {
            Play(gameSounds.menuTurnPage);
        }


        public void PlayMenuDressSound()
        {
            Play(gameSounds.menuDress);
        }


        public void PlayMenuBackgroundSound()
        {
            Play(gameSounds.menuBackground);
        }
        
        
        public void PlayMenuPacketPrizeSound()
        {
            Play(gameSounds.menuPacketPrize);
        }
        
        
        public void PlayMenuPacketTearingSound()
        {
            Play(gameSounds.menuPacketTearing);
        }
        
        
        public void PlayMemoryMatchCardTurningSound()
        {
            Play(gameSounds.memoryMatchCardTurning);
        }
        
        
        public void PlayMemoryMatchCorrectMatchSound()
        {
            Play(gameSounds.memoryMatchCorrectMatch);
        }
        
        
        public void PlayMemoryMatchIncorrectMatchSound()
        {
            Play(gameSounds.memoryMatchIncorrectMatch);
        }
        
        
        public void PlayCreationSkillSound()
        {
            Play(gameSounds.creationSkill);
        }
        
        
        public void PlayCreationWinSound()
        {
            Play(gameSounds.creationWin);
        }
        
        
        public void PlayCreationLoseSound()
        {
            Play(gameSounds.creationLose);
        }


        public void PlayPuzzleSlideSound()
        {
            Play(gameSounds.puzzleSlide);
        }
        
        
        public void PlayPlayerCustomizationSelectingSkinSound()
        {
            Play(gameSounds.playerCustomizationSelectingSkin);
        }
        
        
        public void PlayMarketBuySound()
        {
            Play(gameSounds.marketBuy);
        }

        
        public void PlayCompetitionWinSound()
        {
            Play(gameSounds.competitionWin);
        }
        
        
        public void PlayCompetitionLoseSound()
        {
            Play(gameSounds.competitionLose);
        }
        
        
        public void PlayCompetitionTourWinSound()
        {
            Play(gameSounds.competitionTourWin);
        }
        
        
        public void PlayCompetitionTourLoseSound()
        {
            Play(gameSounds.competitionTourLose);
        }
        
        
        public void PlayCompetitionCountdownSound()
        {
            Play(gameSounds.competitionCountdown);
        }
        
        
        public void PlayCompetitionHornSound()
        {
            Play(gameSounds.competitionHorn);
        }
        
        
        public void PlayMakeItLongerAddingBoxSound()
        {
            Play(gameSounds.makeItLongerAddingBox);
        }
        
        
        public void PlayMakeItLongerBoxDroppingSound()
        {
            Play(gameSounds.makeItLongerBoxDropping);
        }
        
        
        public void PlayFastEyesGameStartSound()
        {
            Play(gameSounds.fastEyesGameStart);
        }
        
        
        public void PlayFastEyesShootingSound()
        {
            Play(gameSounds.fastEyesShooting);
        }
        
        
        public void PlayFastEyesSkippingTargetSound()
        {
            Play(gameSounds.fastEyesSkippingTarget);
        }
        
        
        public void PlayFastEyesCorrectShootingSound()
        {
            Play(gameSounds.fastEyesCorrectShooting);
        }
        
        
        public void PlayFastEyesIncorrectShootingSound()
        {
            Play(gameSounds.fastEyesIncorrectShooting);
        }
        
        
        public void PlayFastEyesBoomBulletSound()
        {
            Play(gameSounds.fastEyesBoomBullet);
        }
        
        
        public void PlayDontFallLavaSound()
        {
            Play(gameSounds.dontFallLava);
        }
        
        
        public void PlayDontFallDeadSound()
        {
            Play(gameSounds.dontFallDead);
        }
        
        
        public void PlayDontFallShakingSound()
        {
            Play(gameSounds.dontFallShaking);
        }
        
        
        public void PlayDontFallBrokingSound()
        {
            Play(gameSounds.dontFallBroking);
        }


        public void PlayPuzzleFreezeSkill()
        {
            Play(gameSounds.puzzleFreezeSkill);
        }


        public void PlayPuzzleIceBreaking()
        {
            Play(gameSounds.puzzleIceBreaking);
        }
        
        
        public void PlaySnowbuttBackgroundSound()
        {
            Play(gameSounds.snowbuttBackground);
        }
        
        
        public void PlaySnowbuttThrowingSnowSound()
        {
            Play(gameSounds.snowbuttThrowingSnow);
        }
        
        
        public void PlaySnowbuttSnowHitSound()
        {
            Play(gameSounds.snowbuttSnowHit);
        }
        
        
        public void PlaySnowbuttDeadSound()
        {
            Play(gameSounds.snowbuttDead);
        }
        
        
        public void PlayGamblingIncreaseBetSound()
        {
            Play(gameSounds.gamblingIncreaseBet);
        }
        
        
        public void PlayGamblingDecreaseBetSound()
        {
            Play(gameSounds.gamblingDecreaseBet);
        }
        
        
        public void PlayGamblingWinSound()
        {
            Play(gameSounds.gamblingWin);
        }
        
        
        public void PlayGamblingLoseSound()
        {
            Play(gameSounds.gamblingLose);
        }
        
        
        public void PlayGamblingBackgroundSound()
        {
            Play(gameSounds.gamblingBackground);
        }
        
        
        public void PlayGamblingButtonSound()
        {
            Play(gameSounds.gamblingButton);
        }
        
        
        public void PlayBlackjackStartGameButtonSound()
        {
            Play(gameSounds.blackjackStartGameButton);
        }
        
        
        public void PlayBlackjackCardSound()
        {
            Play(gameSounds.blackjackCard);
        }
        
        
        public void PlayHorseRaceStartRaceButtonSound()
        {
            Play(gameSounds.horseRaceStartRaceButton);
        }
        
        
        public void PlayHorseRaceHorseRunningSound()
        {
            Play(gameSounds.horseRaceHorseRunnig);
        }
        
        
        public void PlayHorseRaceStartGunSound()
        {
            Play(gameSounds.horseRaceStartGun);
        }


        public void PlaySlotMachine()
        {
            Play(gameSounds.slotMachine);
        }


        private void Play(GameSound gameSound)
        {
            var soundPlayer = new GameObject(gameSound.clip.name)
                .AddComponent<SoundPlayer>();
            
            soundPlayer.SetClip(gameSound.clip);
            soundPlayer.SetType(gameSound.type);
            soundPlayer.SetVolume(gameSound.volume);
            soundPlayer.SetLoop(gameSound.loop);
            soundPlayer.Play();
        }
    }
}
