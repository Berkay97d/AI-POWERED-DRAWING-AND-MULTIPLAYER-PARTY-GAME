using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class ScrollRectEvents : MonoBehaviour,
        IBeginDragHandler,
        IEndDragHandler
    {
        public UnityEvent onBeginVerticalDrag;
        public UnityEvent onEndDrag;
        

        public void OnBeginDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;
            var tangent = delta.y / delta.x;
            var absTangent = Mathf.Abs(tangent);
            
            if (absTangent < 1f) return;
            
            onBeginVerticalDrag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke();
        }
    }
}