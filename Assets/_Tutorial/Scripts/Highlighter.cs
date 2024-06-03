using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Tutorial
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private RectTransform main;
        [SerializeField] private RectTransform focus;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform bottom;
        [SerializeField] private RectTransform right;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform hand;
        [SerializeField] private Image[] images;
        [SerializeField] private Image[] extraImages;
        [SerializeField, Range(0f, 1f)] private float alpha = 0.75f;
        [SerializeField] private float fadeDuration = 0.1f;


        private static Highlighter ms_Instance;


        private Tween m_FadeTween; 


        private void Awake()
        {
            ms_Instance = this;
        }


        public static void SetActive(bool value)
        {
            ms_Instance.main.gameObject.SetActive(value);
        }

        public static void SetHandActive(bool value)
        {
            ms_Instance.hand.gameObject.SetActive(value);
        }

        public static void Focus(RectTransform target, Vector2 padding, float pixelPerUnitMultiplier)
        {
            var position = target.position;
            var size = target.rect.size - padding;
            position.z = ms_Instance.focus.position.z;
            
            ms_Instance.focus.position = position;
            ms_Instance.focus.sizeDelta = size;
            ms_Instance.images[0].pixelsPerUnitMultiplier = pixelPerUnitMultiplier;

            var totalSize = ms_Instance.main.rect.size;
            var focusAnchoredPosition = ms_Instance.focus.anchoredPosition;

            var leftWidth = totalSize.x * 0.5f + focusAnchoredPosition.x - size.x * 0.5f;
            var leftHeight = totalSize.y;
            ms_Instance.left.sizeDelta = new Vector2(leftWidth, leftHeight);
            
            var rightWidth = totalSize.x * 0.5f - focusAnchoredPosition.x - size.x * 0.5f;
            var rightHeight = totalSize.y;
            ms_Instance.right.sizeDelta = new Vector2(rightWidth, rightHeight);

            var topWidth = totalSize.x - leftWidth - rightWidth;
            var topHeight = totalSize.y * 0.5f - focusAnchoredPosition.y - size.y * 0.5f;
            var topPosition = ms_Instance.top.position;
            topPosition.x = position.x;
            ms_Instance.top.position = topPosition;
            ms_Instance.top.sizeDelta = new Vector2(topWidth, topHeight);

            var bottomWidth = topWidth;
            var bottomHeight = totalSize.y * 0.5f + focusAnchoredPosition.y - size.y * 0.5f;
            var bottomPosition = ms_Instance.bottom.position;
            bottomPosition.x = position.x;
            ms_Instance.bottom.position = bottomPosition;
            ms_Instance.bottom.sizeDelta = new Vector2(bottomWidth, bottomHeight);
        }

        public static void HandFocus(RectTransform target, Vector2 offset, float scale, float angle)
        {
            var position = target.position;
            position.z = ms_Instance.hand.position.z;

            ms_Instance.hand.position = position;
            ms_Instance.hand.anchoredPosition += offset;
            ms_Instance.hand.localScale = Vector3.one * scale;
            ms_Instance.hand.localEulerAngles = Vector3.forward * angle;
        }

        public static void Show()
        {
            ms_Instance.m_FadeTween?.Kill();

            ms_Instance.m_FadeTween = DOTween
                .To(ms_Instance.GetAlpha, ms_Instance.SetAlpha, ms_Instance.alpha, ms_Instance.fadeDuration);
        }

        public static void Hide()
        {
            ms_Instance.m_FadeTween?.Kill();

            ms_Instance.m_FadeTween = DOTween
                .To(ms_Instance.GetAlpha, ms_Instance.SetAlpha, 0f, ms_Instance.fadeDuration);
        }

        public static void SetFocusRaycastTarget(bool value)
        {
            ms_Instance.images[0].raycastTarget = value;
        }


        private float GetAlpha()
        {
            return images[0].color.a;
        }
        
        private void SetAlpha(float a)
        {
            foreach (var image in images)
            {
                var color = image.color;
                color.a = a;
                image.color = color;
            }

            foreach (var image in extraImages)
            {
                var color = image.color;
                color.a = a / alpha;
                image.color = color;
            }
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            SetAlpha(alpha);
        }

#endif
    }
}