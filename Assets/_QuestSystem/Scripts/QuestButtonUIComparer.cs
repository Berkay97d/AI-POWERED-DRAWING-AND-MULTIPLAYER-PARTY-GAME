using System.Collections.Generic;

namespace _QuestSystem.Scripts
{
    public class QuestButtonUIComparer : IComparer<QuestButtonUI>
    {
        public int Compare(QuestButtonUI x, QuestButtonUI y)
        {
            var xPriority = x.GetPriority();
            var yPriority = y.GetPriority();

            if (xPriority > yPriority) return 1;

            if (xPriority < yPriority) return -1;

            return 0;
        }
    }
}