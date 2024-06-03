using System;
using System.Linq;
using General;
using TMPro;
using UnityEngine;

namespace UI
{
    public class NavigationPage : Page
    {
        [SerializeField] private NavigationElement[] navigationElements;
        [SerializeField] private TMP_Text bookPointText;
        
        private Book book;

        private void Awake()
        {
            book = FindObjectOfType<Book>();
        }

        private void Start()
        {
            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                SetNavigationElements();
            });
        }

        private void Update()
        {
            bookPointText.text = book.GetBookPoint().ToString();
        }

        private void SetNavigationElements()
        {
            for (int i = 0; i < navigationElements.Length; i++)
            {
                navigationElements[i].SetCardPage(book.GetAllCardPages()[i]);
            }
        }

        public NavigationElement[] GetNavigationElements()
        {
            return navigationElements;
        }

        public NavigationElement GetNavigationElementBySubTitle(string value)
        {
            return navigationElements
                .FirstOrDefault(navElement => navElement.GetCardPage().GetSubTitle().Name == value);
        }

        public void SetNavigationElementsNonInteractableExcept(NavigationElement navigationElement)
        {
            foreach (var element in navigationElements)
            {
                var interactable = element == navigationElement;
                element.SetInteractable(interactable);
            }
        }

        public void SetInteractableNavigationElements(bool value)
        {
            foreach (var element in navigationElements)
            {
                element.SetInteractable(value);
            }
        }
    }
}