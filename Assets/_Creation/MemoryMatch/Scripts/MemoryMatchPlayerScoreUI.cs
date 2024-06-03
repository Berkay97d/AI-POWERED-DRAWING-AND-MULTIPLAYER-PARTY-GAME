using TMPro;
using UnityEngine;

namespace _Compatetion.MemoryMatch
{
    public class MemoryMatchPlayerScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameField;
        [SerializeField] private TMP_Text scoreField;


        private MemoryMatchPlayer m_Player;


        private void OnDestroy()
        {
            m_Player.OnScoreChanged -= OnPlayerScoreChanged;
        }


        private void OnPlayerScoreChanged(int score)
        {
            scoreField.text = score.ToString();
        }
        
        
        public void SetPlayer(MemoryMatchPlayer player)
        {
            m_Player = player;
            nameField.text = player.GetName();
            player.OnScoreChanged += OnPlayerScoreChanged;
        }
    }
}