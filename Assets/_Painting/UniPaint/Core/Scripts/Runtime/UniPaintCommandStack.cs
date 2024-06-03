using System.Collections.Generic;

namespace UniPaint
{
    public class UniPaintCommandStack
    {
        private readonly Stack<IPaintCommand> m_Commands = new Stack<IPaintCommand>();
        private readonly Stack<IPaintCommand> m_UndoneCommands = new Stack<IPaintCommand>();


        public void ExecuteCommand(IPaintCommand command)
        {
            command.Execute();
            m_Commands.Push(command);
            m_UndoneCommands.Clear();
        }

        public void Undo()
        {
            if (!m_Commands.TryPop(out var command)) return;
            
            command.Undo();
            m_UndoneCommands.Push(command);
        }

        public void Redo()
        {
            if (!m_UndoneCommands.TryPop(out var command)) return;
            
            command.Redo();
            m_Commands.Push(command);
        }
    }
}