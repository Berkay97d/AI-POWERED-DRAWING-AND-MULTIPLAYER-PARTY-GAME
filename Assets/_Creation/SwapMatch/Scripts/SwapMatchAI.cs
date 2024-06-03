using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Creation.SwapMatch.Scripts
{
    public class SwapMatchAI : MonoBehaviour
    {
        [SerializeField] private SwapMatchManager manager;
        [SerializeField] private float minThinkDuration = 0.2f;
        [SerializeField] private float maxThinkDuration = 1f;


        public Action OnScrambled;
        

        private SwapMatchGame m_Game;
        private float m_ThinkTimer;
        private float m_FreezeTimer;
        private bool m_IsScramble;


        private void Awake()
        {
            m_ThinkTimer = GetRandomThinkDuration();
        }


        private void Update()
        {
            if (!manager.IsPlaying()) return;
            
            if (!TickFreezeTimer()) return;
            
            if (!TickThinkTimer()) return;
            
            MakeMove();
            m_ThinkTimer = GetRandomThinkDuration();
        }
        
        
        public void SetGame(SwapMatchGame game)
        {
            m_Game = game;
        }

        public void Scramble(int iteration)
        {
            if (m_IsScramble) return;

            StartCoroutine(Routine());
            
            IEnumerator Routine()
            {
                const float duration = 0.75f;

                m_IsScramble = true;
                m_Game.SmoothScrambleTiles(iteration);
                AddThinkTimer(duration);
                OnScrambled?.Invoke();
                yield return new WaitForSeconds(duration);

                m_IsScramble = false;
            }
        }
        
        public void AddFreezeForSeconds(float seconds)
        {
            m_FreezeTimer += seconds;
        }

        public void AddThinkTimer(float seconds)
        {
            m_ThinkTimer += seconds;
        }

        public float GetFreezeTimer()
        {
            return m_FreezeTimer;
        }
        
        public bool IsFrozen()
        {
            return m_FreezeTimer > 0f;
        }


        private bool TickThinkTimer()
        {
            m_ThinkTimer = Mathf.Max(m_ThinkTimer - Time.deltaTime, 0f);
            return m_ThinkTimer <= 0f;
        }

        private bool TickFreezeTimer()
        {
            m_FreezeTimer = Mathf.Max(m_FreezeTimer - Time.deltaTime, 0f);
            return m_FreezeTimer <= 0f;
        }
        
        private float GetRandomThinkDuration()
        {
            return Random.Range(minThinkDuration, maxThinkDuration);
        }
        
        private void MakeMove()
        {
            var tile = m_Game.GetDiscardedTile();
            var neighbourTiles = m_Game.GetNeighbourTiles(tile);
            var otherTile = neighbourTiles[Random.Range(0, neighbourTiles.Count)];
            
            m_Game.SwapTiles(tile, otherTile);
        }
    }
}