using System;
using General;
using UI;
using UnityEngine;

namespace _SoundSystem.Scripts
{
    public class MainMenuSoundInitilazor : MonoBehaviour
    {
        [SerializeField] private CollectionPageArrow[] collectionPageArrows;
        [SerializeField] private CollectionMenuController collectionMenuController;
        
        
        private SoundManager soundManager;

        
        private void Awake()
        {
            soundManager = GetComponent<SoundManager>();
        }

        private void Start()
        {
            /*foreach (var collectionPageArrow in collectionPageArrows)  //AKTİF SAYFA DEĞİŞ
            {
                collectionPageArrow.OnCollectionPageArrowClicked += OnCollectionPageArrowClicked;
            } */   
            
            collectionMenuController.OnCollectionPageArrive += OnCollectionPageArrive;
            
            soundManager.PlayMenuBackgroundSound();
        }

        private void OnCollectionPageArrive(object sender, EventArgs e)
        {
            LazyCoroutines.WaitForSeconds(.8f, soundManager.PlayMenuTurnPageSound);
        }

        private void OnCollectionPageArrowClicked(object sender, bool e)
        {
            soundManager.PlayMenuTurnPageSound();
        }
    }
}