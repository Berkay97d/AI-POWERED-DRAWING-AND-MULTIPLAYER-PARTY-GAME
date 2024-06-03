using System;
using System.Collections;
using System.Collections.Generic;
using _MenuSwipe.Scripts;
using UnityEngine;

namespace UI
{
    public class CollectionMenuController : MonoBehaviour
    {
        [SerializeField] private MenuSwipeManager menuSwipeManager;
        
        
        public static CollectionMenuController Instance;
        public event EventHandler OnCollectionPageArrive;
        public event EventHandler OnCollectionPageGone;


        private void Awake()
        {
            Instance = this;

            menuSwipeManager.OnActivePanelChanged += OnActivePageChanged;
        }

        private void OnDestroy()
        {
            menuSwipeManager.OnActivePanelChanged -= OnActivePageChanged;
        }


        private void OnActivePageChanged(MenuSwipeManager.EventArgs args)
        {
            if (args.currentPanelIndex == 3)
            {
                RaiseOnCollectionPageArrive();
                return;
            }

            if (args.previousPanelIndex == 3)
            {
                RaiseOnCollectionPageGone();
            }
        }
        
        
        public static void RaiseOnCollectionPageArrive()
        {
            Instance.OnCollectionPageArrive?.Invoke(Instance, EventArgs.Empty);
        }
        
        public static void RaiseOnCollectionPageGone()
        {
            Instance.OnCollectionPageGone?.Invoke(Instance, EventArgs.Empty);
        }
    }
}