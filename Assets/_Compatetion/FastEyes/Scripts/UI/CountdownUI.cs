using System;
using _SoundSystem.Scripts;
using TMPro;
using UnityEngine;


namespace FastEyes
{
    public class CountdownUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private MinigameTutorial minigameTutorial;
        
        private bool m_IsTutorialOver;

        private void Start()
        {
            GameController.Instance.OnGameStateChanged += OnGameStateChanged;
            minigameTutorial.OnTutorialOver += OnTutorialOver;
        }

        private void OnTutorialOver(object sender, EventArgs e)
        {
            m_IsTutorialOver = true;
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.CountdownToStart)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Update()
        {
            if (m_IsTutorialOver)
            {
                UpdateText();
            }
        }

        private void UpdateText()
        {
            countdownText.text = Mathf.RoundToInt(GameController.Instance.GetCountdownTimer()).ToString();

            if (GameController.Instance.GetIsGameOver())
            {
                countdownText.gameObject.SetActive(false);
            }
        }

        private void Show()
        {
            countdownText.gameObject.SetActive(true);
        }

        private void Hide()
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}