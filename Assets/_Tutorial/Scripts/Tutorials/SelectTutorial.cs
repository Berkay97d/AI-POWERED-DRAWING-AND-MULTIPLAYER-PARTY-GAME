using System;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class SelectTutorial : Tutorial
    {
        [SerializeField] private CardTypeSelectionButton titleSelectionButton;


        protected override void OnBegin()
        {
            titleSelectionButton.OnSelected += OnTitleSelected;
        }

        protected override void OnComplete()
        {
            titleSelectionButton.OnSelected -= OnTitleSelected;
        }


        private void OnTitleSelected(string title)
        {
            Complete();
        }
    }
}