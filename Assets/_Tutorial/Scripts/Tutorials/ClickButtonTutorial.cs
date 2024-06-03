using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Tutorial
{
    [Serializable]
    public class ClickButtonTutorial : Tutorial
    {
        [SerializeField] private Button button;
        [SerializeField] private Vector2 padding;
        [SerializeField] private float pixelPerUnitMultiplier = 1f;
        [SerializeField] private Vector2 handOffset;
        [SerializeField] private float handScale = 1f;
        [SerializeField] private float handAngle;


        protected override void OnBegin()
        {
            var rectTransform = button.GetComponent<RectTransform>();
            Highlighter.Focus(rectTransform, padding, pixelPerUnitMultiplier);
            Highlighter.HandFocus(rectTransform, handOffset, handScale, handAngle);
            button.onClick.AddListener(OnClickButton);
        }

        protected override void OnComplete()
        {
            button.onClick.RemoveListener(OnClickButton);
        }


        private void OnClickButton()
        {
            Complete();
        }
    }
}