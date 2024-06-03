using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Creation.SwapMatch.Scripts
{
    public class SwapMatchUI : MonoBehaviour
    {
        [SerializeField] private SwapMatchManager manager;
        [SerializeField] private SwapMatchAI ai;
        [SerializeField] private TMP_Text timerField;
        [SerializeField] private Button scrambleSkillButton;
        [SerializeField] private Button freezeSkillButton;
        [SerializeField] private GameObject frozenUI;
        [SerializeField] private TMP_Text frozenTimerField;

        public event EventHandler OnFreezeButtonClick; 

        private void Awake()
        {
            scrambleSkillButton.onClick.AddListener(() =>
            {
                ai.Scramble(30);
            });
            
            freezeSkillButton.onClick.AddListener(() =>
            {
                OnFreezeButtonClick?.Invoke(this, EventArgs.Empty);
                ai.AddFreezeForSeconds(5);
            });
        }

        private void Update()
        {
            UpdateTimer();
            UpdateFrozenUI();
        }


        private void UpdateTimer()
        {
            var elapsedTime = manager.IsPlaying()
                ? manager.GetElapsedTime()
                : manager.GetTotalTime();
            var minutes = $"{elapsedTime.Minutes:00}";
            var seconds = $"{elapsedTime.Seconds:00}";
            timerField.text = $"{minutes}:{seconds}";
        }

        private void UpdateFrozenUI()
        {
            frozenUI.SetActive(ai.IsFrozen());
            frozenTimerField.text = $"{ai.GetFreezeTimer():00.00}";
        }
    }
}