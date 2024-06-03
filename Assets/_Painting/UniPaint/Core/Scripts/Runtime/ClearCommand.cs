using UnityEngine;

namespace UniPaint
{
    public class ClearCommand : IPaintCommand
    {
        private readonly UniPaintCanvas m_Canvas;
        
        private Color[] m_OldTextureColors;
        
        
        public ClearCommand(UniPaintCanvas canvas)
        {
            m_Canvas = canvas;
        }
        
        
        public void Execute()
        {
            m_OldTextureColors = m_Canvas.GetTextureColors();
            m_Canvas.SetColorForAllPixels(Color.clear);
        }

        public void Undo()
        {
            m_Canvas.SetTextureColors(m_OldTextureColors);
        }

        public void Redo()
        {
            m_Canvas.SetColorForAllPixels(Color.clear);
        }
    }
}