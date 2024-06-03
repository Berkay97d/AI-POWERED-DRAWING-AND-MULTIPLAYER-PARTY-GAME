using System;
using _DatabaseAPI.Scripts;
using _PaintToCard.Scripts;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using ImageUtils;
using UnityEngine;

namespace _Creation.SwapMatch.Scripts
{
    public class SwapMatchManager : MonoBehaviour
    {
        [SerializeField] private SwapMatchAI ai;
        [SerializeField] private SwapMatchTile tilePrefab;
        [SerializeField] private Texture2D testTexture;
        
        [SerializeField] private int row;
        [SerializeField] private int column;

        public event EventHandler<bool> OnGameOver;

        private DateTime m_StartTime;
        private DateTime m_EndTime;
        private bool m_IsPlaying;
        
        
        private void Start()
        {
            const float scale = 4f;
            
            var game1 = CreateGame("Game 1", () =>
            {
                EndGame();
                OnWin();
            }, Vector3.down * 9f, scale);
            var game2 = CreateGame("Game 2", () =>
            {
                EndGame();
                OnLose();
            }, Vector3.up * 14.5f, scale);

            game1.SetAllowInput(true);
            game2.SetAllowInput(false);
            
            ai.SetGame(game2);
            
            StartGame();
        }


        private void OnWin()
        {
            LocalUser.AddQuestProgress(QuestType.Win_Generation);
            LocalUser.AddQuestProgress(QuestType.Win_SwapMatch);
            
            LocalUser.AddCard(new CardData
            {
                imageID = PaintToCardConverter.GetGeneratedImageID()
            }, response =>
            {
                OnGameOver?.Invoke(this, true);
            });
        }

        private void OnLose()
        {
            OnGameOver?.Invoke(this, false);
        }
        

        public TimeSpan GetElapsedTime()
        {
            return DateTime.Now - m_StartTime;
        }

        public TimeSpan GetTotalTime()
        {
            return m_EndTime - m_StartTime;
        }

        public bool IsPlaying()
        {
            return m_IsPlaying;
        }


        private void StartGame()
        {
            m_IsPlaying = true;
            m_StartTime = DateTime.Now;
        }
        
        private void EndGame()
        {
            m_IsPlaying = false;
            m_EndTime = DateTime.Now;
        }
        
        private SwapMatchGame CreateGame(string gameName, Action onComplete, Vector3 offset = default, float scale = 1f)
        {
            var game = new SwapMatchGame(gameName, onComplete, tilePrefab, GetTexture(), row, column, offset, scale);
            return game;
        }

        private Texture2D GetTexture()
        {
            var texture = PaintToCardConverter.GetGeneratedImage();
            
            return texture ? texture : testTexture;
        }
    }
}