using System;
using TMPro;
using UnityEngine;

namespace MakeItLonger
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timetText;
        [SerializeField] private GameController gameController;
        

        private void Update()
        {
            timetText.text = gameController.GetRoundTimer().ToString("0.00");
        }
    }
}