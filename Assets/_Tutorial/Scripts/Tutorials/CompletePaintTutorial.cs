using System;
using _Painting;

namespace _Tutorial
{
    [Serializable]
    public class CompletePaintTutorial : Tutorial
    {
        protected override void OnBegin()
        {
            PaintManager.OnPaintingComplete += PaintManager_OnPaintingComplete;
        }

        protected override void OnComplete()
        {
            PaintManager.OnPaintingComplete -= PaintManager_OnPaintingComplete;
        }


        private void PaintManager_OnPaintingComplete(PaintingData data)
        {
            Complete();
        }
    }
}