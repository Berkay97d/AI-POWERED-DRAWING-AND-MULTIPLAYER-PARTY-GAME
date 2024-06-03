using System;
using General;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class WaitForSecondsTutorial : Tutorial
    {
        [SerializeField] private float seconds = 1f;
        [SerializeField] private bool hideHighlighterOnBegin = true;
        [SerializeField] private bool showHighlighterOnComplete = true;
        
        
        protected override void OnBegin()
        {
            if (hideHighlighterOnBegin)
            {
                Highlighter.Hide();
                Highlighter.SetFocusRaycastTarget(true);
            }
            
            LazyCoroutines.WaitForSeconds(seconds, Complete);
        }

        protected override void OnComplete()
        {
            if (showHighlighterOnComplete)
            {
                Highlighter.Show();
                Highlighter.SetFocusRaycastTarget(false);
            }
        }
    }
}