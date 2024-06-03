using System;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class FocusTutorial : Tutorial
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private Vector2 padding;
        [SerializeField] private float pixelPerUnitMultiplier = 1f;


        protected override void OnBegin()
        {
            Highlighter.Focus(target, padding, pixelPerUnitMultiplier);
            Complete();
        }
    }
}