using System;
using System.Collections;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using General;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class ShowroomController : MonoBehaviour
    {
        [SerializeField] private Transform showroomMain;
        [SerializeField] private Card showroomCard;
        [SerializeField] private CardOptions cardOptions;
        [SerializeField] private ParticleSystem upgradeParticle;
        [SerializeField] private ParticleSystem demolishParticle;
        
        [SerializeField] private Animator animator;
        [SerializeField] private Button closeShowroomButton;
        

        private void Start()
        {
            closeShowroomButton.onClick.AddListener((() =>
            {
                HideShowroom();
                closeShowroomButton.gameObject.SetActive(false);
            }));
            
            cardOptions.OnDemolishStart += OnDemolishStart;
            cardOptions.OnLoookStart += OnLoookStart;
            cardOptions.OnUpgradeStart += OnUpgradeStart;
            cardOptions.OnCraftPageCardChanged += OnCraftPageCardChanged;
        }
        
        private void OnDestroy()
        {
            cardOptions.OnDemolishStart -= OnDemolishStart;
            cardOptions.OnLoookStart -= OnLoookStart;
            cardOptions.OnUpgradeStart -= OnUpgradeStart;
            cardOptions.OnCraftPageCardChanged -= OnCraftPageCardChanged;
        }

        private void OnCraftPageCardChanged(object sender, Card e)
        {
            SetShowroomCard();
        }
        
        private void OnUpgradeStart(object sender, EventArgs e)
        {
            OnUpgradeSuccessed();
            void OnUpgradeSuccessed()
            {
                ShowShowroom();
                RotateCard();
                
                StartCoroutine(InnerRoutine());
                
                IEnumerator InnerRoutine()
                {
                    yield return new WaitForSeconds(5f);
                    ShowUpgradeParticle();
                    showroomCard.ChangeOutlineVisual(showroomCard.GetCardData().Rarity + 1);
                    showroomCard.ChangeCardPointVisual(25);
                    LocalUser.UpdateCard(cardOptions.GetCard().GetCardId(), cardOptions.GetCard().GetCardData());
                    
                    yield return new WaitForSeconds(6f);
                    HideShowroom();
                    HideUpgradeParticle();
                }
            } 
        }
            
        private void OnLoookStart(object sender, EventArgs e)
        {
            ShowShowroom();
            closeShowroomButton.gameObject.SetActive(true);
        }

        private void OnDemolishStart(object sender, EventArgs e)
        {
            ShowShowroom();
            ShowDemolishParticle();

            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                showroomCard.transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.InBack);
                LazyCoroutines.WaitForSeconds(2f, () =>
                {
                    HideShowroom();
                    HideDemolishParticle();
                    showroomCard.transform.localScale = Vector3.one *2;
                });
            });
        }

        private void SetShowroomCard()
        {
            showroomCard.SetCardData(cardOptions.GetCard().GetCardData());
        }

        private void ShowShowroom()
        {
            showroomMain.gameObject.SetActive(true);
        }
        
        private void HideShowroom()
        {
            showroomMain.gameObject.SetActive(false);
        }

        private void ShowUpgradeParticle()
        {
            upgradeParticle.gameObject.SetActive(true);
        }

        private void HideUpgradeParticle()
        {
            upgradeParticle.gameObject.SetActive(false);
        }

        private void ShowDemolishParticle()
        {
            demolishParticle.gameObject.SetActive(true);
        }

        private void HideDemolishParticle()
        {
            demolishParticle.gameObject.SetActive(false);
        }

        private void RotateCard()
        {
            animator.SetTrigger("Rotate");
        }
    }
}