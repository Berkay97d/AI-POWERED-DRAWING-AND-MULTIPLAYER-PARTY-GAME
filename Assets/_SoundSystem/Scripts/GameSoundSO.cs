    using System;
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.Serialization;

    namespace _SoundSystem.Scripts
    {
        [CreateAssetMenu]
        public class GameSoundSO : ScriptableObject
        {
            [Foldout("Menu")]
            public GameSound 
                menuBackground,
                menuClassicButton, 
                menuPlayButton, 
                menuTurnPage, 
                menuDress,
                menuPacketPrize,
                menuPacketTearing;


            [Foldout("Creation")] 
            public GameSound
                creationWin,
                creationLose,
                creationSkill;


            [Foldout("Memory Match")] 
            public GameSound
                memoryMatchCardTurning,
                memoryMatchCorrectMatch,
                memoryMatchIncorrectMatch;


            [Foldout("8 Piece Puzzle")] 
            public GameSound
                puzzleSlide,
                puzzleFreezeSkill,
                puzzleIceBreaking;


            [Foldout("Player Customization")] 
            public GameSound
                playerCustomizationSelectingSkin;


            [Foldout("Market")] 
            public GameSound
                marketBuy;


            [Foldout("Competition")] 
            public GameSound
                competitionWin,
                competitionLose,
                competitionTourWin,
                competitionTourLose,
                competitionCountdown,
                competitionHorn;


            [Foldout("Laser")]
            public GameSound
                laserJump,
                laserLaserSound,
                laserDead;

            [Foldout("Make It Longer")]
            public GameSound
                makeItLongerAddingBox,
                makeItLongerBoxDropping;


            [Foldout("Fast Eyes")]
            public GameSound
                fastEyesGameStart,
                fastEyesShooting,
                fastEyesSkippingTarget,
                fastEyesCorrectShooting,
                fastEyesIncorrectShooting,
                fastEyesBoomBullet;


            [Foldout("Dont Fall")] 
            public GameSound
                dontFallLava,
                dontFallDead,
                dontFallShaking,
                dontFallBroking;


            [Foldout("Snowbutt")] 
            public GameSound
                snowbuttBackground,
                snowbuttThrowingSnow,
                snowbuttSnowHit,
                snowbuttDead,
                booomParticle,
                snow;

            [Foldout("Katana")]
            public GameSound
                cut,
                correctSlice;

            [FormerlySerializedAs("background")] [Foldout("Katana")]
            public GameSound
                katanaBackground;


            [Foldout("Gambling")]
            public GameSound
                gamblingIncreaseBet,
                gamblingDecreaseBet,
                gamblingWin,
                gamblingLose,
                gamblingBackground,
                gamblingButton;


            [Foldout("Blackjack")]
            public GameSound
                blackjackStartGameButton,
                blackjackCard;


            [Foldout("Horse Race")] 
            public GameSound
                horseRaceStartRaceButton,
                horseRaceHorseRunnig,
                horseRaceStartGun;


            [Foldout("Slot Machine")]
            public GameSound
                slotMachine;
        }


        [Serializable]
        public struct GameSound
        {
            public AudioClip clip;
            public SoundType type;
            [Range(0f, 1f)] public float volume;
            public bool loop;
        }
    }