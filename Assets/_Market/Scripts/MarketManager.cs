using System;
using UnityEngine;

namespace _Market.Scripts
{
    public class MarketManager : MonoBehaviour
    {
        [SerializeField] private ConfirmPanel confirmPanel;
        [SerializeField] private RectTransform content;


        private Vector2? m_ContentTargetAnchoredPos;
        

        private void Awake()
        {
            InjectToBuyButtons();
        }

        private void Update()
        {
            if (m_ContentTargetAnchoredPos.HasValue)
            {
                content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, m_ContentTargetAnchoredPos.Value, 25f * Time.deltaTime);

                if ((content.anchoredPosition - m_ContentTargetAnchoredPos.Value).sqrMagnitude <= 0.01f)
                {
                    m_ContentTargetAnchoredPos = null;
                }
            }
        }


        public void OpenConfirmPanelFromBuyButton(MarketBuyButton buyButton)
        {
            confirmPanel.SyncWithBuyButton(buyButton);
            confirmPanel.Show();
        }

        public void OnNotEnoughCoins()
        {
            confirmPanel.OnNotEnoughCoins();
        }

        public void OnNotEnoughGems()
        {
            confirmPanel.OnNotEnoughGems();
        }


        public void FocusGemSection()
        {
            m_ContentTargetAnchoredPos = Vector2.up * 810;
        }

        public void FocusCoinSection()
        {
            m_ContentTargetAnchoredPos = Vector2.up * 2375;
        }
        
        
        private void InjectToBuyButtons()
        {
            var buyButtons = GetComponentsInChildren<MarketBuyButton>();

            foreach (var buyButton in buyButtons)
            {
                buyButton.SetMarketManager(this);
            }
        }
    }
}