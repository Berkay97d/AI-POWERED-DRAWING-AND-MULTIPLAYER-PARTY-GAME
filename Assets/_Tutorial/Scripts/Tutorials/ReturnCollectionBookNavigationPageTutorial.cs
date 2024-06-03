using System;
using UI;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class ReturnCollectionBookNavigationPageTutorial : Tutorial
    {
        [SerializeField] private Book book;


        private NavigationPageTransferButton m_Button;
        

        protected override void OnBegin()
        {
            m_Button = ((CardPage) book.GetActivePage()).GetNavigationPageTransferButton();

            m_Button.OnClick += OnClickNavigationPageTransferButton;

            var target = m_Button.GetRect();
            
            Highlighter.HandFocus(target, Vector2.down * 25f, 1.5f, 25f);
            Highlighter.Focus(target, new Vector2(-30f, -30f), 9f);
        }

        protected override void OnComplete()
        {
            m_Button.OnClick -= OnClickNavigationPageTransferButton;
        }


        private void OnClickNavigationPageTransferButton(object sender, EventArgs e)
        {
            Complete();
        }
    }
}