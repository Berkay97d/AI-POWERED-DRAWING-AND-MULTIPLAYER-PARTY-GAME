using System;
using UnityEngine;

namespace _Compatetion.MemoryMatch
{
    public abstract class MemoryMatchPlayer
    {
        public event Action<int> OnScoreChanged;
        public event Action<float> OnTimerChanged; 


        protected MemoryMatchGame game;

        
        private string m_Name;
        private int m_Score;
        private float m_MoveTimer;


        public MemoryMatchPlayer(string name)
        {
            m_Name = name;
        }


        public void Update()
        {
            if (!game.IsPlaying()) return;
            
            if (!IsMyTurn()) return;
            
            SetMoveTimer(Mathf.Max(m_MoveTimer - Time.deltaTime, 0f));

            if (m_MoveTimer <= 0)
            {
                game.MoveNextPlayer();
            }
        }

        public void SetMoveTimer(float seconds)
        {
            m_MoveTimer = seconds;
            OnTimerChanged?.Invoke(m_MoveTimer);
        }
        
        public string GetName()
        {
            return m_Name;
        }

        public void AssignGame(MemoryMatchGame memoGame)
        {
            game = memoGame;
        }

        public void AddScore(int value)
        {
            var score = m_Score + value;
            SetScore(score);
        }

        public int GetScore()
        {
            return m_Score;
        }
        
        public void SetScore(int value)
        {
            m_Score = value;
            OnScoreChanged?.Invoke(value);
        }
        
        public bool IsMyTurn()
        {
            return game.IsPlayerTurn(this);
        }

        public virtual void MakeMove()
        {
            
        }
    }
}