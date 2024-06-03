using UnityEngine;

namespace UniPaint
{
    public class ColorBucketCommand : IPaintCommand
    {
        private readonly UniPaintCanvas m_Canvas;
        private readonly Color m_NewColor;
        private readonly int m_PositionX;
        private readonly int m_PositionY;
        
        private Color[] m_OldTextureColors;
        private Color[] m_NewTextureColors;


        public ColorBucketCommand(UniPaintCanvas canvas, Color newColor, int x, int y)
        {
            m_Canvas = canvas;
            m_NewColor = newColor;
            m_PositionX = x;
            m_PositionY = y;
        }
        
        
        public void Execute()
        {
            m_OldTextureColors = m_Canvas.GetTextureColors();
            m_Canvas.FloodFill(m_PositionX, m_PositionY);
            m_Canvas.ApplyFloodFillBuffer(m_NewColor);
            m_NewTextureColors = m_Canvas.GetTextureColors();
        }

        public void Undo()
        {
            m_Canvas.SetTextureColors(m_OldTextureColors);
        }

        public void Redo()
        {
            m_Canvas.SetTextureColors(m_NewTextureColors);
        }
    }
}