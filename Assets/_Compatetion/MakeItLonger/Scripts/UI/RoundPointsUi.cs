using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace MakeItLonger
{
    public class RoundPointsUi : MonoBehaviour
    {
        [SerializeField] private Image[] rounds;
        [SerializeField] private Contester player;
        [SerializeField] private Color playerColor;
        [SerializeField] private Color enemyColor;
        [SerializeField] private GameController gameController;
        

        private void Start()
        {
            gameController.OnRoundOver += OnRoundOver;
        }

        private void OnDestroy()
        {
            gameController.OnRoundOver -= OnRoundOver;
        }

        private void OnRoundOver(object sender, OnRoundOverArgs e)
        {
            if (e.WinnerContester == player)
            {
                rounds[e.OveredRoundNum -1 ].gameObject.SetActive(true);
                rounds[e.OveredRoundNum -1 ].color = playerColor;
            }
            else
            {
                rounds[e.OveredRoundNum -1 ].gameObject.SetActive(true);
                rounds[e.OveredRoundNum -1 ].color = enemyColor;
            }
        }
    }
}