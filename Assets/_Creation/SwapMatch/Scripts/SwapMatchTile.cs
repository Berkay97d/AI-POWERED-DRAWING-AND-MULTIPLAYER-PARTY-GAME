using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Creation.SwapMatch.Scripts
{
    public class SwapMatchTile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text numberField;
        [SerializeField] private float swapDuration = 0.15f;


        private Tween m_MoveTween;
        private BoxCollider m_Collider;
        private SwapMatchGame m_Game;
        private int m_InitialPositionX;
        private int m_InitialPositionY;
        private bool m_IsDiscarded;


        private void OnMouseUpAsButton()
        {
            if (!m_Game.GetAllowInput()) return;
            
            TrySwap();
        }


        public void SetGame(SwapMatchGame game)
        {
            m_Game = game;
        }
        
        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void SetNumber(int value)
        {
            numberField.text = value.ToString();
        }

        public void HideNumber()
        {
            numberField.gameObject.SetActive(false);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void MoveToPosition(Vector3 position, TweenCallback onComplete = default)
        {
            m_MoveTween?.Kill();
            SwapMatchSoundInitilazor.PlaySlideSound();
            m_MoveTween = transform.DOMove(position, swapDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(onComplete);
        }

        public void SetInitialPosition(int x, int y)
        {
            m_InitialPositionX = x;
            m_InitialPositionY = y;
        }

        public bool IsInitialPosition(int x, int y)
        {
            return x == m_InitialPositionX && y == m_InitialPositionY;
        }
        
        public void MatchColliderWithSpriteSize(Sprite sprite)
        {
            if (!m_Collider)
            {
                m_Collider = gameObject.AddComponent<BoxCollider>();
            }

            var bounds = sprite.bounds;

            m_Collider.center = bounds.center;
            m_Collider.size = bounds.size;
        }

        public void Discard()
        {
            m_IsDiscarded = true;
            gameObject.SetActive(false);
        }

        public void Return()
        {
            m_IsDiscarded = false;
            gameObject.SetActive(true);
        }

        public bool IsDiscarded()
        {
            return m_IsDiscarded;
        }
        
        
        private IReadOnlyList<SwapMatchTile> GetNeighbourTiles()
        {
            return m_Game.GetNeighbourTiles(this);
        }

        private bool TrySwap()
        {
            var otherTile = GetNeighbourTiles()
                .FirstOrDefault(tile => tile.IsDiscarded());

            if (!otherTile) return false;
            
            m_Game.SwapTiles(this, otherTile);
            return true;
        }
    }
}