using System;
using TMPro;
using UnityEngine;

namespace MakeItLonger
{
    public class CountdownUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private GameController gameController;
        [SerializeField] private MinigameTutorial minigameTutorial;

        private bool m_IsTutorialOver;
        

        private void Start()
        {
            gameController.OnGameStateChanged += OnGameStateChanged;
            minigameTutorial.OnTutorialOver += OnTutorialOver;
        }


        private void Update()
        {
            if (m_IsTutorialOver)
            {
                UpdateText();
            }
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


        private void UpdateText()
        {
            countdownText.text = Mathf.RoundToInt(gameController.GetCountdownTimer()).ToString();

            if (gameController.GetIsGameOver())
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