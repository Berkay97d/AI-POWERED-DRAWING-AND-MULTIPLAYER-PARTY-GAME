namespace UniPaint
{
    public interface IPaintCommand
    {
        void Execute();
        void Undo();
        void Redo();
    }
}