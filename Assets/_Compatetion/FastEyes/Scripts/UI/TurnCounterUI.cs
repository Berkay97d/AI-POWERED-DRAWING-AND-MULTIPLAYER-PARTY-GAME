using System;
using TMPro;
using UnityEngine;

namespace FastEyes
{
    public class TurnCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text turnText;


        private void Start()
        {
            GameController.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                turnText.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            turnText.text ="LAST " + (GameController.Instance.GetRemainingTurnCount() + 1) + " SHOT";
        }
    }
}