using TMPro;
using UnityEngine;

namespace _Compatetion.MemoryMatch
{
    public class MemoryMatchPlayerTimerUI : MonoBehaviour
    {
        [SerializeField] private GameObject main;
        [SerializeField] private TMP_Text timerField;


        private MemoryMatchPlayer m_Player;

        
        private void OnDestroy()
        {
            m_Player.OnTimerChanged -= OnPlayerTimerChanged;
        }

        private void Update()
        {
            main.SetActive(m_Player.IsMyTurn());
        }


        private void OnPlayerTimerChanged(float seconds)
        {
            var deci = seconds - Mathf.FloorToInt(seconds);
            var secondsInt = Mathf.RoundToInt(seconds - deci);
            var milliSecondsInt = Mathf.RoundToInt(deci * 1000);
            timerField.text = $"{secondsInt:00}.<size=60%>{milliSecondsInt:000}";
        }

        
        public void SetPlayer(MemoryMatchPlayer player)
        {
            m_Player = player;
            m_Player.OnTimerChanged += OnPlayerTimerChanged;
        }
    }
}