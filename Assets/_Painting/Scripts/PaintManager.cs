using System;
using DG.Tweening;
using ImageUtils;
using UniPaint;
using UnityEngine;
using UnityEngine.UI;

namespace _Painting
{
    public class PaintManager : MonoBehaviour
    {
        private const BrushShape DefaultBrushShape = BrushShape.Circle;
        private const Tool DefaultTool = Tool.Pen;
        private static readonly Color DefaultBrushColor = Color.black;
        private const float DefaultBrushSizeNormalized = 0.4f;
        private const float MinBrushSizeNormalized = 0.1f;


        [SerializeField] private PaintUI paintUI;
        [SerializeField] private UniPaintCanvas canvas;
        [SerializeField] private GameObject toolAndColorPanel;
        [SerializeField] private GameObject brushSizePanel;
        [SerializeField] private Button brushShapeButton;
        [SerializeField] private Button brushSizeButton;
        [SerializeField] private Button[] toolButtons;
        [SerializeField] private Button trashButton;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button redoButton;
        [SerializeField] private Slider brushSizeSlider;
        [SerializeField] private Button doneBrushSizeButton;
        [SerializeField] private Button donePaintingButton;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject[] toolSelects;
        [SerializeField] private Image brushPreviewImage;
        [SerializeField] private Sprite[] brushShapeSprites;
        [SerializeField] private Transform paintPageMainTransform;
        [SerializeField] private Transform gameModeSelectionTransform;

        public static event Action<PaintingData> OnPaintingComplete; 

        private static PaintManager ms_Instance;
        private string m_prompt;
        private BrushShape m_BrushShape;
        private Tool m_Tool;
        private Color m_BrushColor;
        private float m_BrushSizeNormalized;


        private void Awake()
        {
            ms_Instance = this;
            
            brushShapeButton.onClick.AddListener(ToggleBrushShape);
            brushSizeButton.onClick.AddListener(() =>
            {
                SetActiveToolAndColorPanel(false);
                SetActiveBrushSizePanel(true);
            });

            for (var i = 0; i < toolButtons.Length; i++)
            {
                var tool = (Tool) i;
                
                toolButtons[i].onClick.AddListener(() =>
                {
                    SetTool(tool);
                });
            }
            
            trashButton.onClick.AddListener(canvas.Clear);
            undoButton.onClick.AddListener(canvas.GetCommandStack().Undo);
            redoButton.onClick.AddListener(canvas.GetCommandStack().Redo);

            doneBrushSizeButton.onClick.AddListener(() =>
            {
                SetActiveToolAndColorPanel(true);
                SetActiveBrushSizePanel(false);
            });

            brushSizeSlider.minValue = MinBrushSizeNormalized;
            brushSizeSlider.value = DefaultBrushSizeNormalized;
            brushSizeSlider.onValueChanged.AddListener(SetBrushSizeNormalized);
            
            donePaintingButton.onClick.AddListener(() =>
            {
                paintUI.SetPaintPreview(canvas.GetTexture().ToSprite());
                paintUI.ShowConfirmPaintPanel();
            });
            
            SetBrushShape(DefaultBrushShape);
            SetTool(DefaultTool);
            SetBrushColor(DefaultBrushColor);
            SetBrushSizeNormalized(DefaultBrushSizeNormalized);
            
            backButton.onClick.AddListener(GoBackToCardCustomizationPage);
        }

        private void GoBackToCardCustomizationPage()
        {
            paintPageMainTransform.localScale = Vector3.zero;
            
            gameModeSelectionTransform.localScale = Vector3.zero;
            gameModeSelectionTransform.gameObject.SetActive(true);
            gameModeSelectionTransform.DOScale(Vector3.one, 0.35f).OnComplete(() =>
            {
                paintPageMainTransform.gameObject.SetActive(false);
            });
        }
        
        private void OnBrushShapeChanged()
        {
            UpdateBrushPreview();
            UpdateTool();
        }

        private void OnBrushSizeChanged()
        {
            UpdateBrushPreview();
            ms_Instance.canvas.SetToolSize(m_BrushSizeNormalized);
        }

        private void OnBrushColorChanged()
        {
            UpdateBrushPreview();
            ms_Instance.canvas.SetSelectedColor(ms_Instance.m_BrushColor);
        }

        private void OnToolChanged()
        {
            UpdateSelectedToolPreview();
            UpdateTool();
        }


        private void SetActiveToolAndColorPanel(bool value)
        {
            toolAndColorPanel.SetActive(value);
        }
        
        private void SetActiveBrushSizePanel(bool value)
        {
            brushSizePanel.SetActive(value);
        }
        
        private void SetBrushShape(BrushShape brushShape)
        {
            m_BrushShape = brushShape;
            OnBrushShapeChanged();
        }

        private void SetBrushSizeNormalized(float size)
        {
            m_BrushSizeNormalized = size;
            OnBrushSizeChanged();
        }

        private void SetTool(Tool tool)
        {
            m_Tool = tool;
            OnToolChanged();
        }

        private void ToggleBrushShape()
        {
            var brushShape = m_BrushShape == BrushShape.Circle
                ? BrushShape.Square
                : BrushShape.Circle;
                
            SetBrushShape(brushShape);
        }

        private void UpdateBrushPreview()
        {
            brushPreviewImage.sprite = brushShapeSprites[(int) m_BrushShape];
            brushPreviewImage.color = m_BrushColor;
            brushPreviewImage.transform.localScale = Vector3.one * Mathf.Clamp(m_BrushSizeNormalized, 0.15f, 1f);
        }

        private void UpdateSelectedToolPreview()
        {
            for (var i = 0; i < toolSelects.Length; i++)
            {
                var isSelected = i == (int) m_Tool;
                toolSelects[i].SetActive(isSelected);
            }
        }

        private void UpdateTool()
        {
            Action action = (m_BrushShape, m_Tool) switch
            {
                (BrushShape.Circle, Tool.Pen) => canvas.SetToolCirclePen,
                (BrushShape.Circle, Tool.Eraser) => canvas.SetToolCircleEraser,
                (BrushShape.Square, Tool.Pen) => canvas.SetToolSquarePen,
                (BrushShape.Square, Tool.Eraser) => canvas.SetToolSquareEraser,
                (_, Tool.ColorPicker) => canvas.SetToolColorPicker,
                (_, Tool.ColorBucket) => canvas.SetToolColorBucket,
                _ => null
            };
            
            action?.Invoke();
        }
        

        public static void SetBrushColor(Color color)
        {
            ms_Instance.m_BrushColor = color;
            ms_Instance.OnBrushColorChanged();
        }

        public static void SetIsPaintable(bool value)
        {
            ms_Instance.canvas.SetIsPaintable(value);
        }

        public static void CompletePaint()
        {
            OnPaintingComplete?.Invoke(new PaintingData
            {
                imageBase64 = ms_Instance.canvas.ToBase64String(),
                prompt = ms_Instance.m_prompt
            });
        }

        public static void SetPrompt(string prompt)
        {
            ms_Instance.m_prompt = prompt;
        }
        

        private enum BrushShape
        {
            Circle,
            Square
        }
        
        private enum Tool
        {
            Pen,
            Eraser,
            ColorPicker,
            ColorBucket
        }
    }
}