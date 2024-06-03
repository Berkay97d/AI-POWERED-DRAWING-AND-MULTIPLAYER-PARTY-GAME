using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace EnterAndExitUI_Competation.Scripts
{
    public class PlayerInfoTableUI : MonoBehaviour
    {
        [SerializeField] private PlayerInfoUI playerInfoUIPrefab;
        [SerializeField] private Transform infoParent;


        private readonly List<PlayerInfoUI> m_PlayerInfoUIs = new();


        public void Sort(int[] order)
        {
            for (var i = 0; i < order.Length; i++)
            {
                m_PlayerInfoUIs[i].SetSiblingIndex(order[i]);
            }
        }
        
        public void Hide()
        {
            transform.DOMoveY(transform.position.y + 4000, 0.5f)
                .SetEase(Ease.OutSine);
        }
        
        public int GetPlayerCount()
        {
            return m_PlayerInfoUIs.Count;
        }
        
        public void SetPlayerCount(int count)
        {
            for (var i = 0; i < count; i++)
            {
                AddPlayerInfoUI();
            }
        }

        public PlayerInfoUI GetPlayerInfoUI(int index)
        {
            return m_PlayerInfoUIs[index];
        }

        public PlayerInfoUI GetOrderedPlayerInfoUI(int index)
        {
            return infoParent.GetChild(index).GetComponent<PlayerInfoUI>();
        }

        public void CollectPlayerCardPreviews(Vector3 position, float duration)
        {
            foreach (var playerInfoUI in m_PlayerInfoUIs)
            {
                playerInfoUI.CollectCardPreviews(position, duration);
            }
        }


        private void AddPlayerInfoUI()
        {
            var playerInfoUI = Instantiate(playerInfoUIPrefab, infoParent);
            playerInfoUI.SetBannerIndex(m_PlayerInfoUIs.Count);
            m_PlayerInfoUIs.Add(playerInfoUI);
        }
    }
}