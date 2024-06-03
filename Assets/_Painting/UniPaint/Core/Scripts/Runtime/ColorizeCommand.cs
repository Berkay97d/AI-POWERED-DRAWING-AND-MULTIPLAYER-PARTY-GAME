using UnityEngine;

namespace UniPaint
{
    public class ColorizeCommand : IPaintCommand
    {
        private readonly UniPaintCanvas m_Canvas;
        private readonly Color[] m_OldTextureColors;
        private readonly Color[] m_NewTextureColors;


        public ColorizeCommand(UniPaintCanvas canvas, Color[] oldTextureColors, Color[] newTextureColors)
        {
            m_Canvas = canvas;
            m_OldTextureColors = oldTextureColors;
            m_NewTextureColors = newTextureColors;
        }
        
        
        public void Execute()
        {
            
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