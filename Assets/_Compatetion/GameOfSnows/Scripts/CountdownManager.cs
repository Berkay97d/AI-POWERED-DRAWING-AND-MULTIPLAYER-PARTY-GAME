using System;
using System.Collections;
using _SoundSystem.Scripts;
using TMPro;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts
{
    public class CountdownManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private MinigameTutorial minigameTutorial;
        
        public static CountdownManager Instance;

        private bool m_IsGamePaused = false;
        private int m_Count = 3;

        
        private void Awake()
        {
            Instance = this;
            
            minigameTutorial.OnTutorialOver += OnTutorialOver;
        }

        
        private void OnDestroy()
        {
            minigameTutorial.OnTutorialOver -= OnTutorialOver;
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
            Debug.Log("Game paused");
            PauseGame();

            m_Count = 3;

            while (m_Count > -1)
            {
                Debug.Log(m_Count);
                yield return new WaitForSecondsRealtime(1f);

                m_Count--;
            }
            
            
            Debug.Log("Countdown finished");
            ResumeGame();
        }

        
        private void PauseGame()
        {
            countdownText.gameObject.SetActive(true);
            
            Time.timeScale = 0;
            m_IsGamePaused = true;
        }

        
        private void ResumeGame()
        {
            countdownText.gameObject.SetActive(false);
            
            Time.timeScale = 1;
            m_IsGamePaused = false;
        }
    }
}