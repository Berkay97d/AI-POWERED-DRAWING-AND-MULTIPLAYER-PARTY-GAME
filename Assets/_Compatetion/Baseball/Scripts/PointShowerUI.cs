using System;
using DG.Tweening;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Compatetion.Baseball.Scripts
{
    public class PointShowerUI : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform textTransformPrefab;
        [SerializeField] private Contester contester;

        private Color m_Color;

        private void Start()
        {
            contester.OnContesterGetPoint += OnContesterGetPoint;
            contester.GetContesterKicker().OnCreatedCircleInFruit += OnCreatedCircleInFruit;
        }

        private void OnCreatedCircleInFruit(Color obj)
        {
            m_Color = obj;
        }

        private void OnDestroy()
        {
            contester.OnContesterGetPoint -= OnContesterGetPoint;
        }

        private void OnContesterGetPoint(object sender, int e)
        {
            var pointText = Instantiate(textTransformPrefab, transform);
            pointText.GetComponentInChildren<TMP_Text>().text = e.ToString();
            var pointTextImage = GetComponentInChildren<Image>();
            pointTextImage.color = m_Color;

            LazyCoroutines.WaitForSeconds(.5f, (() =>
            {
                pointText.transform.DOMove(targetTransform.position, .5f);
                pointText.transform.DOScale(Vector3.zero * .3f, .5f).OnComplete((() =>
                {
                    Destroy(pointText.gameObject);
                }));
            }));

        }
        
    }
}
