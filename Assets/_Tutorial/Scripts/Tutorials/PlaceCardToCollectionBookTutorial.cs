using System;
using General;
using UI;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class PlaceCardToCollectionBookTutorial : Tutorial
    {
        [SerializeField] private CollectionPageCardInitilazor craftPage;
        [SerializeField] private Book book;
        [SerializeField] private int cardIndex;


        private NavigationElement m_NavigationElement;
        private Card m_Card;
        
        
        protected override void OnBegin()
        {
            m_Card = craftPage.GetCard(cardIndex);

            var navigationPage = book.GetNavigationPage();
            var subTitle = m_Card.GetCardData().subTitle;
            var page = book.GetPageBySubTitle(subTitle);
            m_NavigationElement = navigationPage.GetNavigationElementBySubTitle(subTitle);
            navigationPage.SetNavigationElementsNonInteractableExcept(m_NavigationElement);
            navigationPage.SetInteractableArrowButtons(false);
            page.SetInteractableArrowButtons(false);
            page.SetInteractableNavigationTransferButton(false);
            var rect = m_NavigationElement.GetRect();

            m_NavigationElement.OnClick += OnClickNavigationElement;
            
            Highlighter.HandFocus(rect, Vector2.zero, 1.5f, 25f);
            Highlighter.Focus(rect, new Vector2(-40f, -35f), 9f);
        }

        protected override void OnComplete()
        {
            m_Card.OnCardClicked -= OnClickCard;

            var subTitle = m_Card.GetCardData().subTitle;
            var page = book.GetPageBySubTitle(subTitle);
            var navigationPage = book.GetNavigationPage();
            
            navigationPage.SetInteractableNavigationElements(true);
            navigationPage.SetInteractableArrowButtons(true);
            
            page.SetInteractableNavigationTransferButton(true);
        }


        private void OnClickNavigationElement(object sender, CardPage cardPage)
        {
            m_NavigationElement.OnClick -= OnClickNavigationElement;
            
            Highlighter.Hide();
            Highlighter.SetFocusRaycastTarget(true);

            LazyCoroutines.WaitForSeconds(1.5f, () =>
            {
                m_Card.OnCardClicked += OnClickCard;

                var target = m_Card.GetNormalRect();

                Highlighter.Show();
                Highlighter.SetFocusRaycastTarget(false);
                Highlighter.HandFocus(target, Vector2.down * 100f, 1.5f, 25f);
                Highlighter.Focus(target, new Vector2(-25f, -25f), 7f);
            });
        }

        private void OnClickCard(object sender, Card card)
        {
            Complete();
        }
    }
}