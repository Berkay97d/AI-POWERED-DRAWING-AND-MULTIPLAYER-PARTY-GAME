using UnityEngine;
using UnityEngine.UI;

namespace _Compatetion.MemoryMatch
{
    public class MemoryMatchUI : MonoBehaviour
    {
        [SerializeField] private MemoryMatchManager manager;
        [SerializeField] private Button revealSkillButton;
        [SerializeField] private Button autoMatchSkillButton;


        private void Awake()
        {
            revealSkillButton.onClick.AddListener(() =>
            {
                var game = manager.GetGame();
                var canUse = game
                    .GetLocalPlayer()
                    .IsMyTurn();

                if (!canUse) return;
                
                game.RevealAllTilesForSeconds(1);
            });
            
            autoMatchSkillButton.onClick.AddListener(() =>
            {
                var game = manager.GetGame();
                var canUse = game
                    .GetLocalPlayer()
                    .IsMyTurn();

                if (!canUse) return;
                
                game.AutoMatchSinglePair();
            });
        }
    }
}