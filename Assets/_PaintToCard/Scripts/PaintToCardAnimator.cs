using System;
using _Painting;
using General;
using UnityEngine;

namespace _PaintToCard.Scripts
{
    public class PaintToCardAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("speed");
        private static readonly int RevealHash = Animator.StringToHash("reveal");
        private static readonly int ExitHash = Animator.StringToHash("exit");
        private static readonly int FadeOutHash = Animator.StringToHash("fadeOut");


        [SerializeField] private Animator cardAnimator;
        [SerializeField] private Animator brushAnimator;
        [SerializeField] private Animator tapAnimator;
        [SerializeField] private Animator tapToSeeAnimator;
        [SerializeField] private Animator tapToClaimAnimator;
        [SerializeField] private float speedUp = 1.5f;
        [SerializeField] private float slowDown = 5f;


        public static event Action OnGeneratedCardClaimed;

        
        private Action m_OnTap;
        private bool m_IsActive;
        

        private void Awake()
        {
            m_OnTap = SpeedUp;

            PaintManager.OnPaintingComplete += OnPaintCompleted;
            PaintToCardConverter.OnImageGenerated += OnImageGenerated;
        }

        private void OnDestroy()
        {
            PaintManager.OnPaintingComplete -= OnPaintCompleted;
            PaintToCardConverter.OnImageGenerated -= OnImageGenerated;
        }

        private void Update()
        {
            if (!m_IsActive) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                m_OnTap?.Invoke();
                return;
            }
            
            SlowDown();
        }


        private void OnPaintCompleted(PaintingData data)
        {
            m_IsActive = true;
        }
        
        private void OnImageGenerated(Texture2D texture)
        {
            m_OnTap = null;
            
            ExitBrush();
            FadeOutTap();

            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                m_OnTap = Reveal;
                tapToSeeAnimator.gameObject.SetActive(true);
                SetTapToSeeSpeed(0.5f);
            });
        }


        private void Reveal()
        {
            m_OnTap = null;
            
            FadeOutTapToSee();
            RevealCard();
            
            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                m_OnTap = Claim;
                tapToClaimAnimator.gameObject.SetActive(true);
                SetTapToSeeSpeed(0.5f);
            });
        }

        private void Claim()
        {
            m_OnTap = null;

            OnGeneratedCardClaimed?.Invoke();
        }
        
        private void SlowDown()
        {
            var t = Time.deltaTime * slowDown;
            var speed = Mathf.Lerp(GetBrushSpeed(), 1f, t);
            SetBrushSpeed(speed);
            SetCardSpeed(speed);
            SetTapSpeed(speed);
        }
        
        private void SpeedUp()
        {
            var speed = GetBrushSpeed() + speedUp;
            SetBrushSpeed(speed);
            SetCardSpeed(speed);
            SetTapSpeed(speed);
        }

        private void RevealCard()
        {
            cardAnimator.SetTrigger(RevealHash);
        }

        private void ExitBrush()
        {
            brushAnimator.SetTrigger(ExitHash);
        }

        private void FadeOutTap()
        {
            tapAnimator.SetTrigger(FadeOutHash);
        }

        private void FadeOutTapToSee()
        {
            tapToSeeAnimator.SetTrigger(FadeOutHash);
        }

        private void FadeOutTapToClaim()
        {
            tapToClaimAnimator.SetTrigger(FadeOutHash);
        }

        private void SetBrushSpeed(float value)
        {
            brushAnimator.SetFloat(SpeedHash, value);
        }

        private float GetBrushSpeed()
        {
            return brushAnimator.GetFloat(SpeedHash);
        }

        private void SetCardSpeed(float value)
        {
            cardAnimator.SetFloat(SpeedHash, value);
        }

        private void SetTapSpeed(float value)
        {
            tapAnimator.SetFloat(SpeedHash, value);
        }

        private void SetTapToSeeSpeed(float value)
        {
            tapToSeeAnimator.SetFloat(SpeedHash, value);
        }

        private void SetTapToClaimSpeed(float value)
        {
            tapToClaimAnimator.SetFloat(SpeedHash, value);
        }
    }
}