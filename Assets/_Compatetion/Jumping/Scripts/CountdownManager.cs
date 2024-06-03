using System;
using System.Collections;
using _Compatetion.Baseball.Scripts;
using _SoundSystem.Scripts;
using TMPro;
using UnityEngine;

namespace _Compatetion.Jumping.Scripts
{
    public class CountdownManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private MinigameTutorial minigame;
        
        
        public static CountdownManager Instance;

        private bool m_IsGamePaused;
        private int m_Count = 3;

        private void Awake()
        {
            Instance = this;
            minigame.OnTutorialOver += OnTutorialOver;
        }


        private void Start()
        {
            m_IsGamePaused = true;
        }


        private void OnDestroy()
        {
            minigame.OnTutorialOver -= OnTutorialOver;
        }

        
        private void OnTutorialOver(object sender, EventArgs e)
        {
            Countdown();
            SoundManager.Instance.PlayCompetitionCountdownSound();
        }


        private void Update()
        {
            countdownText.text = m_Count.ToString();
        }

        
        public void Countdown()
        {
            StartCoroutine(CountdownCoroutine());
        }

        
        private IEnumerator CountdownCoroutine()
        {
            PauseGame();

            m_Count = 3;

            while (m_Count > -1)
            {
                yield return new WaitForSecondsRealtime(1f);

                m_Count--;
            }

            ResumeGame();
        }

        
        private void PauseGame()
        {
            countdownText.gameObject.SetActive(true);
            Time.timeScale = 0;
            m_IsGamePaused = true;
        }

        
        // ReSharper disable Unity.PerformanceAnalysis
        private void ResumeGame()
        {
            countdownText.gameObject.SetActive(false);
            
            Time.timeScale = 1;
            m_IsGamePaused = false;
            
            SoundManager.Instance.PlayLaserLaserSound();
        }


        public bool GetIsGamePaused()
        {
            return m_IsGamePaused;
        }
    }
}