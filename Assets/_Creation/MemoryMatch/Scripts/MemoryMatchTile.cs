using System;
using DG.Tweening;
using UnityEngine;

namespace _Compatetion.MemoryMatch
{
    public class MemoryMatchTile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer frontFaceSpriteRenderer;
        [SerializeField] private SpriteRenderer backFaceSpriteRenderer;
        [SerializeField] private float toggleDuration = 0.25f;
        
        
        private MemoryMatchGame m_Game;
        private Sprite m_HiddenSprite;
        private BoxCollider m_Collider;
        private int m_PairIndex;
        private bool m_IsClickable = true;
        private int m_GridPositionX;
        private int m_GridPositionY;


        private void OnMouseUpAsButton()
        {
            if (!GetIsClickable()) return;
            
            if (m_Game.GetIsBusy()) return;
            
            if (!m_Game.IsPlayerTurn(m_Game.GetLocalPlayer())) return;
            
            if (m_Game.IsMaxClickReached()) return;

            Click();
        }


        public void OnMatchSucceed()
        {
            Reveal();
        }
        
        public void OnMatchFailed()
        {
            Close(() =>
            {
                SetIsClickable(true);
            });
        }

        public void Click()
        {
            SetIsClickable(false);
            m_Game.OnTileClicked(this);
            
            Open(() =>
            {
                m_Game.OnTileSelected(this);
            });
        }

        public void SetGridPosition(int x, int y)
        {
            m_GridPositionX = x;
            m_GridPositionY = y;
        }

        public int GetGridPositionX()
        {
            return m_GridPositionX;
        }

        public int GetGridPositionY()
        {
            return m_GridPositionY;
        }
        
        public void RevealForSeconds(float seconds)
        {
            Open(null);
            Close(null, seconds);
        }
        
        public void Inject(MemoryMatchGame game)
        {
            m_Game = game;
        }

        public void InstantClose()
        {
            transform.eulerAngles = Vector3.up * 180f;
        }

        public void SetHiddenSprite(Sprite sprite)
        {
            m_HiddenSprite = sprite;
        }

        public void SetFrontFaceSprite(Sprite sprite)
        {
            frontFaceSpriteRenderer.sprite = sprite;
        }

        public int GetPairIndex()
        {
            return m_PairIndex;
        }
        
        public void SetPairIndex(int index)
        {
            m_PairIndex = index;
        }

        public bool GetIsClickable()
        {
            return m_IsClickable;
        }

        public void SetIsClickable(bool value)
        {
            m_IsClickable = value;
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

        public void MatchFrontFaceWithSpriteSize(Sprite sprite)
        {
            var bounds = frontFaceSpriteRenderer.sprite.bounds;
            var otherBounds = sprite.bounds;
            var xFactor = otherBounds.size.x / bounds.size.x;
            var yFactor = otherBounds.size.y / bounds.size.y;

            frontFaceSpriteRenderer.transform.localScale = new Vector3(xFactor, yFactor, 1f);
        }
        
        public void MatchBackFaceWithSpriteSize(Sprite sprite)
        {
            var bounds = backFaceSpriteRenderer.sprite.bounds;
            var otherBounds = sprite.bounds;
            var xFactor = otherBounds.size.x / bounds.size.x;
            var yFactor = otherBounds.size.y / bounds.size.y;

            backFaceSpriteRenderer.transform.localScale = new Vector3(xFactor, yFactor, 1f);
        }
        
        
        private void Open(TweenCallback onOpen)
        {
            transform.DORotate(Vector3.zero, toggleDuration)
                .OnComplete(onOpen);
        }

        private void Close(TweenCallback onClose, float delay = 0f)
        {
            transform.DORotate(Vector3.up * 180f, toggleDuration)
                .SetDelay(delay)
                .OnComplete(onClose);
        }
        
        private void Reveal()
        {
            frontFaceSpriteRenderer.sprite = m_HiddenSprite;
            MatchFrontFaceWithSpriteSize(m_HiddenSprite);
        }
    }
}