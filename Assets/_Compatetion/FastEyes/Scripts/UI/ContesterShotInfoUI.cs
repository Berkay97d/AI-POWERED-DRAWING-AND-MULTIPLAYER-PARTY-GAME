using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace FastEyes
{
    public class ContesterShotInfoUI : MonoBehaviour
    {
        [SerializeField] private Contester contester;
        [SerializeField] private TMP_Text text;
        

        private void Start()
        {
            contester.ShowInfoEvent += ShowInfoEvent;
        }

        private void ShowInfoEvent(object sender, bool e)
        {
            ShowInfo(e);
        }

        private void ShowInfo(bool e)
        {
            StartCoroutine(InnerRoutine());

            IEnumerator InnerRoutine()
            {
                text.transform.localScale = Vector3.zero;
                text.gameObject.SetActive(true);
                if (e)
                {
                    text.color = Color.green;
                    text.text = "HIT!";
                }
                else
                {
                    text.color = Color.red;
                    text.text = "MISS!";
                }
                text.transform.DOScale(Vector3.one, .35f).SetEase(Ease.OutBack);
                yield return new WaitForSeconds(.5f);
                text.transform.DOScale(Vector3.zero, .2f);
                text.gameObject.SetActive(false);
            }
        }
    }
}