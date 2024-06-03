using System;
using System.Collections.Generic;
using _Painting;
using StableDiffusionAPI;
using UnityEngine;

namespace UniPaint
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class UniPaintCanvas : MonoBehaviour
    {
        private static readonly int MainTexID = Shader.PropertyToID("_MainTex");


        public event Action<float, float, float, float> OnColorPicked; 


        private delegate void ToolMethod(Vector2Int position);
        
        
        [SerializeField, Min(1f)] private Vector2Int resolution = new Vector2Int(256, 256);
        [SerializeField, Min(0f)] private float size = 5f;
        [SerializeField, Min(0f)] private float maxToolSize = 25f;
        [SerializeField, Min(0f)] private float minToolSize = 1f;
        [SerializeField, Range(0f, 1f)] private float defaultToolSize = 0.2f;


        private readonly UniPaintCommandStack m_CommandStack = new UniPaintCommandStack();
        private readonly HashSet<int> m_FloodFillBuffer = new HashSet<int>();
        private readonly Stack<(int, int)> m_FloodFillCallStack = new Stack<(int, int)>();


        private Color m_SelectedColor;
        private Color m_FloodFillStartColor;
        private Color[] m_TextureColors;
        private Color[] m_BeginDragTextureColors;
        private Vector3 m_PreviousFrameMousePosition;
        private ColorWheelUI m_ColorWheel;
        private ToolMethod m_SelectedTool;
        private Texture2D m_CanvasTexture;
        private Material m_CanvasMaterial;
        private MeshRenderer m_MeshRenderer;
        private MeshFilter m_MeshFilter;
        private Mesh m_CanvasMesh;
        private Camera m_Camera;
        private float m_ToolSize;
        private int m_Width;
        private int m_Height;
        private bool m_IsDirty;
        private bool m_IsDragging;
        private bool m_IsClickOnlyTool;
        private bool m_IsPaintable = true;


        private void Awake()
        {
            m_Width = resolution.x;
            m_Height = resolution.y;
            m_Camera = Camera.main;
            m_ColorWheel = FindObjectOfType<ColorWheelUI>();
            InitializeMeshRenderer();
            InitializeMesh();
            InitializeTexture();
            m_SelectedTool = CircleDrawTool;
        }

        private void Update()
        {
            if (!m_IsPaintable) return;
            
            HandleInput();
            HandleTool();
            TryApplyChanges();
        }


        public void SetIsPaintable(bool value)
        {
            m_IsPaintable = value;
        }
        
        public UniPaintCommandStack GetCommandStack()
        {
            return m_CommandStack;
        }
        
        public void Clear()
        {
            var command = new ClearCommand(this);
            m_CommandStack.ExecuteCommand(command);
        }
        
        public void SetToolSize(float sizeNormalized)
        {
            m_ToolSize = Mathf.Lerp(minToolSize, maxToolSize, sizeNormalized);
        }

        public void SetToolSizeToDefault()
        {
            SetToolSize(defaultToolSize);
        }

        public float GetDefaultToolSize()
        {
            return defaultToolSize;
        }

        public void SetToolCirclePen()
        {
            m_IsClickOnlyTool = false;
            m_SelectedTool = CircleDrawTool;
        }
        
        public void SetToolCircleEraser()
        {
            m_IsClickOnlyTool = false;
            m_SelectedTool = CircleEraseTool;
        }
        
        public void SetToolSquarePen()
        {
            m_IsClickOnlyTool = false;
            m_SelectedTool = SquareDrawTool;
        }
        
        public void SetToolSquareEraser()
        {
            m_IsClickOnlyTool = false;
            m_SelectedTool = SquareEraseTool;
        }

        public void SetToolColorPicker()
        {
            m_IsClickOnlyTool = true;
            m_SelectedTool = ColorPickerTool;
        }

        public void SetToolColorBucket()
        {
            m_IsClickOnlyTool = true;
            m_SelectedTool = ColorBucketTool;
        }

        public void SetSelectedColor(Color color)
        {
            m_SelectedColor = color;
        }

        public string ToBase64String()
        {
            return TextureUtils.ToBase64String(m_CanvasTexture);
        }

        public Texture2D GetTexture()
        {
            return m_CanvasTexture;
        }

        public Color[] GetTextureColors()
        {
            var textureColors = new Color[m_TextureColors.Length];

            for (var i = 0; i < textureColors.Length; i++)
            {
                textureColors[i] = m_TextureColors[i];
            }

            return textureColors;
        }

        public void SetTextureColors(Color[] textureColors)
        {
            for (var i = 0; i < textureColors.Length; i++)
            {
                m_TextureColors[i] = textureColors[i];
            }
            
            SetDirty();
        }

        public void SetColorForAllPixels(Color color)
        {
            for (var i = 0; i < m_TextureColors.Length; i++)
            {
                SetPixelColorAtIndex(i, color);
            }
            
            SetDirty();
        }


        private bool TryApplyChanges()
        {
            if (!m_IsDirty) return false;

            m_IsDirty = false;
            ApplyChanges();

            return true;
        }

        private void InitializeMeshRenderer()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_MeshFilter = GetComponent<MeshFilter>();

            m_MeshRenderer.material = GetDefaultMaterial();
            m_CanvasMaterial = m_MeshRenderer.material;
        }
        
        private void InitializeMesh()
        {
            m_CanvasMesh = new Mesh();
            m_CanvasMesh.name = "Canvas Mesh";
            var aspectRatio = GetAspectRatio();
            m_CanvasMesh.vertices = new Vector3[]
            {
                new Vector3(-aspectRatio, +1) * size,
                new Vector3(-aspectRatio, -1) * size,
                new Vector3(+aspectRatio, -1) * size,
                new Vector3(+aspectRatio, +1) * size
            };
            m_CanvasMesh.uv = new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };
            m_CanvasMesh.triangles = new int[]
            {
                0, 3, 1, 1, 3, 2
            };
            m_MeshFilter.mesh = m_CanvasMesh;
        }
        
        private void InitializeTexture()
        {
            var pixelCount = GetPixelCount();
            m_TextureColors = new Color[pixelCount];
            m_CanvasTexture = new Texture2D(m_Width, m_Height, TextureFormat.ARGB32, false)
            {
                filterMode = FilterMode.Point
            };
            m_CanvasMaterial.SetTexture(MainTexID, m_CanvasTexture);

            for (int x = 0; x < m_Width; x++)
            {
                for (int y = 0; y < m_Height; y++)
                {
                    SetPixelColor(x, y, Color.white);
                }
            }
            
            ApplyChanges();
        }

        private float GetAspectRatio()
        {
            return (float) m_Width / m_Height;
        }

        private Vector2 GetTotalSize()
        {
            var scale = transform.lossyScale;
            var aspectRatio = GetAspectRatio();
            return new Vector2(scale.x * aspectRatio, scale.y) * (size * 2f);
        }

        private Vector3 GetOrigin()
        {
            return transform.position;
        }

        private void SetDirty()
        {
            m_IsDirty = true;
        }
        
        private void ApplyChanges()
        {
            m_CanvasTexture.SetPixels(m_TextureColors);
            m_CanvasTexture.Apply(false);
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var position = MousePositionToCanvasPosition(Input.mousePosition);
                var pixelPosition = RoundToInt(position);

                if (!IsValidPixelPosition(pixelPosition.x, pixelPosition.y)) return;

                if (m_IsClickOnlyTool)
                {
                    m_SelectedTool(pixelPosition);
                    return;
                }
                    
                m_IsDragging = true;
                m_BeginDragTextureColors = GetTextureColors();
                m_PreviousFrameMousePosition = Input.mousePosition;
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (!m_IsDragging) return;
                
                if (!m_IsClickOnlyTool)
                {
                    HandleDragToolRelease();
                }
                
                m_IsDragging = false;
            }
        }

        private void HandleDragToolRelease()
        {
            var oldColors = new Color[m_BeginDragTextureColors.Length];

            for (var i = 0; i < oldColors.Length; i++)
            {
                oldColors[i] = m_BeginDragTextureColors[i];
            }
            
            var command = new ColorizeCommand(this, oldColors, GetTextureColors());
            m_CommandStack.ExecuteCommand(command);
        }
        
        private void HandleTool()
        {
            if (m_IsClickOnlyTool) return;
            
            if (!m_IsDragging) return;
            
            if (!Input.GetMouseButton(0)) return;

            var mousePosition = Input.mousePosition;
            var deltaDistance = (mousePosition - m_PreviousFrameMousePosition).magnitude;

            for (var i = 0f; i <= deltaDistance; i += m_ToolSize * 1f)
            {
                var t = Mathf.InverseLerp(0, deltaDistance, i);
                var lerpPosition = Vector3.Lerp(mousePosition, m_PreviousFrameMousePosition, t);
                var position = MousePositionToCanvasPosition(lerpPosition);
                m_SelectedTool(RoundToInt(position));
            }
            
            m_PreviousFrameMousePosition = mousePosition;
        }

        private void SquareColorTool(Vector2Int position, Color color)
        {
            if (position.x < 0 || position.x >= m_Width) return;
            
            if (position.y < 0 || position.y >= m_Height) return;
            
            var radiusInt = Mathf.CeilToInt(m_ToolSize);
            var left = Mathf.Max(0, position.x - radiusInt);
            var right = Mathf.Min(m_Width, position.x + radiusInt + 1);
            var bottom = Mathf.Max(0, position.y - radiusInt);
            var top = Mathf.Min(m_Height, position.y + radiusInt + 1);
            
            for (int x = left; x < right; x++)
            {
                for (int y = bottom; y < top; y++)
                {
                    SetPixelColor(x, y, color);
                }
            }
            
            SetDirty();
        }

        private void CircleColorTool(Vector2Int position, Color color)
        {
            if (position.x < 0 || position.x >= m_Width) return;
            
            if (position.y < 0 || position.y >= m_Height) return;
            
            var radiusInt = Mathf.CeilToInt(m_ToolSize);
            var left = Mathf.Max(0, position.x - radiusInt);
            var right = Mathf.Min(m_Width, position.x + radiusInt + 1);
            var bottom = Mathf.Max(0, position.y - radiusInt);
            var top = Mathf.Min(m_Height, position.y + radiusInt + 1);
            var sqrPenSize = m_ToolSize * m_ToolSize;
            
            for (int x = left; x < right; x++)
            {
                for (int y = bottom; y < top; y++)
                {
                    var deltaX = position.x - x;
                    var deltaY = position.y - y;
                    var sqrDistance = deltaX * deltaX + deltaY * deltaY;
                    
                    if (sqrDistance > sqrPenSize) continue;
                    
                    SetPixelColor(x, y, color);
                }
            }
            
            SetDirty();
        }

        private void ColorPickerTool(Vector2Int position)
        {
            if (!IsValidPixelPosition(position.x, position.y)) return;
            
            var index = GetPixelIndex(position.x, position.y);

            var color = m_TextureColors[index];
            Color.RGBToHSV(color, out var h, out var s, out var v);
            PaintManager.SetBrushColor(color);
            OnColorPicked?.Invoke(h, s, v, color.a);
        }

        private void ColorBucketTool(Vector2Int position)
        {
            if (!IsValidPixelPosition(position.x, position.y)) return;
            
            var newColor = GetSelectedColor();
            var command = new ColorBucketCommand(this, newColor, position.x, position.y);
            
            m_CommandStack.ExecuteCommand(command);
        }

        private void SquareDrawTool(Vector2Int position)
        {
            SquareColorTool(position, GetSelectedColor());
        }
        
        private void CircleDrawTool(Vector2Int position)
        {
            CircleColorTool(position, GetSelectedColor());
        }

        private void SquareEraseTool(Vector2Int position)
        {
            SquareColorTool(position, Color.clear);
        }

        private void CircleEraseTool(Vector2Int position)
        {
            CircleColorTool(position, Color.clear);
        }

        private Vector2 MousePositionToCanvasPosition(Vector3 mousePosition)
        {
            var totalSize = GetTotalSize();
            var halfSize = GetTotalSize() * 0.5f;
            var leftBottomPosition = GetOrigin() - new Vector3(halfSize.x, halfSize.y);
            var mouseWorldPosition = m_Camera.ScreenToWorldPoint(mousePosition);
            var worldPosition = mouseWorldPosition - leftBottomPosition;
            var tX = InverseLerpUnclamped(0f, totalSize.x, worldPosition.x);
            var tY = InverseLerpUnclamped(0f, totalSize.y, worldPosition.y);
            return new Vector2(tX * m_Width, tY * m_Height);
        }

        private Color GetSelectedColor()
        {
            return m_SelectedColor;
        }

        private int GetPixelCount()
        {
            return m_Width * m_Height;
        }

        private void SetPixelColor(int x, int y, Color color)
        {
            var index = GetPixelIndex(x, y);
            SetPixelColorAtIndex(index, color);
        }

        private void SetPixelColorAtIndex(int index, Color color)
        {
            m_TextureColors[index] = color;
        }

        private int GetPixelIndex(int x, int y)
        {
            return x + y * m_Width;
        }

        private bool IsValidPixelPosition(int x, int y)
        {
            if (x < 0) return false;

            if (x >= m_Width) return false;

            if (y < 0) return false;

            if (y >= m_Height) return false;

            return true;
        }
        

        public void ApplyFloodFillBuffer(Color color)
        {
            foreach (var index in m_FloodFillBuffer)
            {
                SetPixelColorAtIndex(index, color);
            }
            
            SetDirty();
        }
        
        public void FloodFill(int x, int y)
        {
            var pixelIndex = GetPixelIndex(x, y);

            m_FloodFillBuffer.Clear();
            m_FloodFillCallStack.Clear();
            m_FloodFillStartColor = m_TextureColors[pixelIndex];
            m_FloodFillCallStack.Push((x, y));

            while (true)
            {
                if (!m_FloodFillCallStack.TryPop(out var pixel)) break;

                x = pixel.Item1;
                y = pixel.Item2;
                
                m_FloodFillBuffer.Add(GetPixelIndex(x, y));

                var right = x + 1;
                if (right < m_Width)
                {
                    var index = GetPixelIndex(right, y);
                    var color = m_TextureColors[index];

                    if (EqualColors(color, m_FloodFillStartColor) && !m_FloodFillBuffer.Contains(index))
                    {
                        m_FloodFillCallStack.Push((right, y));
                    }
                }
                
                var top = y + 1;
                if (top < m_Height)
                {
                    var index = GetPixelIndex(x, top);
                    var color = m_TextureColors[index];

                    if (EqualColors(color, m_FloodFillStartColor) && !m_FloodFillBuffer.Contains(index))
                    {
                        m_FloodFillCallStack.Push((x, top));
                    }
                }
                
                var left = x - 1;
                if (left >= 0)
                {
                    var index = GetPixelIndex(left, y);
                    var color = m_TextureColors[index];

                    if (EqualColors(color, m_FloodFillStartColor) && !m_FloodFillBuffer.Contains(index))
                    {
                        m_FloodFillCallStack.Push((left, y));
                    }
                }
                
                var bottom = y - 1;
                if (bottom >= 0)
                {
                    var index = GetPixelIndex(x, bottom);
                    var color = m_TextureColors[index];

                    if (EqualColors(color, m_FloodFillStartColor) && !m_FloodFillBuffer.Contains(index))
                    {
                        m_FloodFillCallStack.Push((x, bottom));
                    }
                }
            }
        }


        private static bool EqualColors(Color lhs, Color rhs)
        {
            const float tolerance = 0.0001f;
            
            if (Mathf.Abs(lhs.r - rhs.r) > tolerance) return false;
            
            if (Mathf.Abs(lhs.g - rhs.g) > tolerance) return false;
            
            if (Mathf.Abs(lhs.b - rhs.b) > tolerance) return false;
            
            if (Mathf.Abs(lhs.a - rhs.a) > tolerance) return false;

            return true;
        }
        
        private static float InverseLerpUnclamped(float a, float b, float value)
        {
            return (double) a != (double) b 
                ? (float) (((double) value - (double) a) / ((double) b - (double) a)) 
                : 0.0f;
        }

        private static Vector2Int RoundToInt(Vector2 vector)
        {
            var x = Mathf.RoundToInt(vector.x);
            var y = Mathf.RoundToInt(vector.y);
            return new Vector2Int(x, y);
        }
        
        private static Material GetDefaultMaterial()
        {
            return Resources.Load<Material>("Default-UniPaint-Material");
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            OnDrawCanvasBorderGizmos();
        }

        private void OnDrawCanvasBorderGizmos()
        {
            var origin = GetOrigin();
            var totalSize = GetTotalSize();
            var bottomLeft = origin + new Vector3(-totalSize.x, -totalSize.y) * 0.5f;
            var bottomRight = origin + new Vector3(totalSize.x, -totalSize.y) * 0.5f;
            var topLeft = origin + new Vector3(-totalSize.x, totalSize.y) * 0.5f;
            var topRight = origin + new Vector3(totalSize.x, totalSize.y) * 0.5f;

            Gizmos.color = Color.black;
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }

#endif
    }
}