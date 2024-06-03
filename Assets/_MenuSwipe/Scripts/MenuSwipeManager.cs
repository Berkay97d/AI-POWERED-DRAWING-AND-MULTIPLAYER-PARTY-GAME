using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _MenuSwipe.Scripts
{
    public class MenuSwipeManager : MonoBehaviour
    {
        [SerializeField] private MenuPanel[] menuPanels;
        [SerializeField] private AnimationCurve clampCurve;
        [SerializeField] private RectOffset padding;
        [SerializeField] private bool isVertical = true;
        [SerializeField] private bool isHorizontal = true;
        [SerializeField] private float snapSpeed = 25f;
        [SerializeField] private float sensitivity = 1f;
        [SerializeField] private float elasticity = 5f;


        public event Action<EventArgs> OnActivePanelChanged;


        private IEnumerable<UIBehaviour> m_InteractableUIBehaviours;
        private Vector3[] m_InitialMenuPanelPositions;
        private float[] m_MenuPanelScaleFactors;
        private Vector3 m_TouchPosition;
        private Vector3 m_TargetOffset;
        private Vector2 m_MinPosition;
        private Vector2 m_MaxPosition;
        private Vector3 m_Offset;
        private bool m_IsPressed;
        private bool m_IsDragging;
        private bool m_IsClamping;
        private bool m_IsDragable;
        private int m_Index = -1;


        private void Awake()
        {
            m_IsDragable = true;
            m_InteractableUIBehaviours = FindObjectsOfType<UIBehaviour>(true)
                .Where(uiBehaviour => uiBehaviour is Button or ScrollRect);

            CacheMenuPanelValues();
        }

        private void Update()
        {
            HandleDrag();
            HandleSmoothSnap();
        }

        
        public void SnapToMenuPanel(int index)
        {
            var initialPosition = m_InitialMenuPanelPositions[index];
            var scaleFactor = m_MenuPanelScaleFactors[index];
            var oldIndex = m_Index;
            
            m_TargetOffset = new Vector3(-initialPosition.x, -initialPosition.y) / scaleFactor;
            m_Index = index;
            
            if (oldIndex == m_Index) return;
            
            OnActivePanelChanged?.Invoke(new EventArgs
            {
                previousPanelIndex = oldIndex,
                currentPanelIndex = m_Index
            });
        }

        public void SetDragable(bool value)
        {
            m_IsDragable = value;
        }

        public int GetActivePanelIndex()
        {
            return m_Index;
        }
        
        
        private void HandleSmoothSnap()
        {
            if (m_IsPressed) return;
            
            m_Offset = Vector3.Lerp(m_Offset, m_TargetOffset, Time.deltaTime * snapSpeed);
            ApplyOffset();
        }

        private void HandleDrag()
        {
            const float dragThreshold = 10f;

            if (!m_IsDragable)
            {
                m_IsPressed = false;
                m_IsDragging = false;
                m_IsClamping = false;
                return;
            }
            
            var touches = Input.touches;
            
            if (touches.Length <= 0) return;

            var touch = touches[0];
            var touchPhase = touch.phase;

            if (touchPhase == TouchPhase.Began)
            {
                m_TouchPosition = GetTouchPosition();
                m_IsPressed = true;
                return;
            }
            
            if (!m_IsPressed) return;
            
            var mouseDelta = GetMouseDelta();

            if (!m_IsDragging && mouseDelta.sqrMagnitude > dragThreshold)
            {
                m_IsDragging = true;
                
                foreach (var uiBehaviour in m_InteractableUIBehaviours)
                {
                    uiBehaviour.enabled = false;
                }
            }

            m_Offset += mouseDelta;
            ApplyOffset();

            m_TouchPosition = GetTouchPosition();

            if (touchPhase == TouchPhase.Ended)
            {
                TrySnapToClosestMenuPanel();
                m_IsPressed = false;
                m_IsDragging = false;
                m_IsClamping = false;

                foreach (var uiBehaviour in m_InteractableUIBehaviours)
                {
                    uiBehaviour.enabled = true;
                }
            }
        }
        
        private void CacheMenuPanelValues()
        {
#if UNITY_EDITOR
            const float bugFixConst = 1.18888891f;
#else
            const float bugFixConst = 1f;
#endif
            
            m_InitialMenuPanelPositions = new Vector3[menuPanels.Length];
            m_MenuPanelScaleFactors = new float[menuPanels.Length];
            
            m_MaxPosition = Vector2.negativeInfinity;
            m_MinPosition = Vector2.positiveInfinity;

            for (var i = 0; i < menuPanels.Length; i++)
            {
                var canvas = menuPanels[i].rectTransform.GetComponentInParent<Canvas>();
                var scaleFactor = canvas.transform.localScale.x / canvas.scaleFactor;
                var position = menuPanels[i].rectTransform.position;

                position.x *= bugFixConst;
                position.y *= bugFixConst;

                m_InitialMenuPanelPositions[i] = position;
                m_MenuPanelScaleFactors[i] = scaleFactor;

                m_MaxPosition.x = Mathf.Max(m_MaxPosition.x, position.x / scaleFactor);
                m_MaxPosition.y = Mathf.Max(m_MaxPosition.y, position.y / scaleFactor);
                m_MinPosition.x = Mathf.Min(m_MinPosition.x, position.x / scaleFactor);
                m_MinPosition.y = Mathf.Min(m_MinPosition.y, position.y / scaleFactor);
            }
        }
        
        private Vector3 GetMouseDelta()
        {
            m_IsClamping = false;
            
            var currentMousePosition = GetTouchPosition();
            var mouseDelta = currentMousePosition - m_TouchPosition;

            mouseDelta.x = isHorizontal ? mouseDelta.x : 0f;
            mouseDelta.y = isVertical ? mouseDelta.y : 0f;
            mouseDelta *= sensitivity;

            var clampScalarX = 1f;
            
            if (m_Offset.x > m_MaxPosition.x)
            {
                var distance = m_Offset.x - m_MaxPosition.x;
                var t = Mathf.InverseLerp(0f, padding.left, distance);
                clampScalarX = elasticity * clampCurve.Evaluate(t);
                m_IsClamping = true;
            }

            if (m_Offset.x < m_MinPosition.x)
            {
                var distance = m_MinPosition.x - m_Offset.x;
                var t = Mathf.InverseLerp(0f, padding.right, distance);
                clampScalarX = elasticity * clampCurve.Evaluate(t);
                m_IsClamping = true;
            }

            var clampScalarY = 1f;

            if (m_Offset.y > m_MaxPosition.y)
            {
                var distance = m_Offset.y - m_MaxPosition.y;
                var t = Mathf.InverseLerp(0f, padding.bottom, distance);
                clampScalarY = elasticity * clampCurve.Evaluate(t);
                m_IsClamping = true;
            }
            
            if (m_Offset.y < m_MinPosition.y)
            {
                var distance = m_MinPosition.y - m_Offset.y;
                var t = Mathf.InverseLerp(0f, padding.top, distance);
                clampScalarY = elasticity * clampCurve.Evaluate(t);
                m_IsClamping = true;
            }
            
            mouseDelta.x *= clampScalarX;
            mouseDelta.y *= clampScalarY;

            return mouseDelta;
        }

        private void ApplyOffset()
        {
            for (var i = 0; i < menuPanels.Length; i++)
            {
                menuPanels[i].rectTransform.position = GetMenuPanelOffsetPosition(i);
            }
        }

        private bool TrySnapToClosestMenuPanel()
        {
            if (!m_IsDragging) return false;
            
            var index = GetClosestMenuPanelIndex();
            SnapToMenuPanel(index);

            return true;
        }

        private int GetClosestMenuPanelIndex()
        {
            if (m_IsClamping) return m_Index;

            const float penaltyScalar = 50f;
            
            var minSqrDistance = float.PositiveInfinity;
            var index = -1;
            
            for (var i = 0; i < menuPanels.Length; i++)
            {
                if (!menuPanels[i].autoSnap) continue;
                
                var position = menuPanels[i].rectTransform.position;
                var scalar = i == m_Index ? penaltyScalar : 1f;
                var sqrDistance = position.x * position.x + position.y + position.y;
                
                if (sqrDistance * scalar > minSqrDistance) continue;

                minSqrDistance = sqrDistance;
                index = i;
            }

            return index;
        }

        private Vector3 GetMenuPanelOffsetPosition(int index)
        {
            return m_InitialMenuPanelPositions[index] + m_Offset * m_MenuPanelScaleFactors[index];
        }

        
        private static Vector3 GetTouchPosition()
        {
            var touches = Input.touches;

            return touches.Length > 0 ? touches[0].position : Input.mousePosition;
        }
        
        
        
        
        public struct EventArgs
        {
            public int previousPanelIndex;
            public int currentPanelIndex;
        }
        
        [Serializable]
        private struct MenuPanel
        {
            public RectTransform rectTransform;
            public bool autoSnap;
        }
    }
}