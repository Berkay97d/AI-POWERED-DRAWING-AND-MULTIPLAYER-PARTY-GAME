using UnityEngine;
using UnityEngine.UI;

namespace _Painting
{
    public class PaintColorButton : MonoBehaviour
    {
        [SerializeField] private Color color;


        private Image m_Image;
        private Button m_Button;
        

        private void Start()
        {
            m_Image = GetComponent<Image>();
            m_Button = GetComponent<Button>();

            m_Image.color = color;
            
            m_Button.onClick.AddListener(() =>
            {
                PaintManager.SetBrushColor(color);
            });
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            GetComponent<Image>().color = color;
        }

#endif
    }
}