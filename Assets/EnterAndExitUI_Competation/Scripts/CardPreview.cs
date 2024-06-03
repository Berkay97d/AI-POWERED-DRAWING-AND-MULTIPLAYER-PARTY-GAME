using _DatabaseAPI.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace EnterAndExitUI_Competation.Scripts
{
    public class CardPreview : MonoBehaviour
    {
        [SerializeField] private Sprite[] cardPreviewsSprites;
        [SerializeField] private Image previewImage;
        [SerializeField] private AnimationCurve[] collectCurvesX;
        [SerializeField] private AnimationCurve[] collectCurvesY;
        

        public void MoveTo(Vector3 to, float duration, TweenCallback onComplete = default)
        {
            transform.DOMove(to, duration)
                .OnComplete(onComplete);
        }
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public void SetLocalPosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetCardData(CardData cardData)
        {
            previewImage.sprite = cardPreviewsSprites[(int)cardData.Rarity];
        }

        public void Collect(Vector3 position, float duration)
        {
            transform.DOMoveX(position.x, duration)
                .SetEase(GetRandomCollectCurveX());

            transform.DOMoveY(position.y, duration)
                .SetEase(GetRandomCollectCurveY());
        }

        public Transform GetParent()
        {
            return transform.parent;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }


        private AnimationCurve GetRandomCollectCurveX()
        {
            return collectCurvesX[Random.Range(0, collectCurvesX.Length)];
        }

        private AnimationCurve GetRandomCollectCurveY()
        {
            return collectCurvesY[Random.Range(0, collectCurvesY.Length)];
        }
    }
}