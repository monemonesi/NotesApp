using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class RemoveNotebookCommand : ICommand
    {
        public NotesVM VM;

        public event EventHandler CanExecuteChanged;

        public RemoveNotebookCommand(NotesVM _VM)
        {
            VM = _VM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Notebook notebook = parameter as Notebook;
            VM.RemoveNoteBook(notebook);
        }
    }
}
