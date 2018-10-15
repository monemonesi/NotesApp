using NotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotesApp.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public RegisterCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            var user = parameter as User;

            bool usernameNotInserted = string.IsNullOrEmpty(user.UserName);
            if (usernameNotInserted) return false;

            bool passwordNotInserted = string.IsNullOrEmpty(user.Password);
            if (passwordNotInserted) return false;

            bool emailNotInserted = string.IsNullOrEmpty(user.Email);
            if (emailNotInserted) return false;

            bool nameNotInserted = string.IsNullOrEmpty(user.Name);
            if (nameNotInserted) return false;

            bool lastNameNotInserted = string.IsNullOrEmpty(user.LastName);
            if (lastNameNotInserted) return false;

            return true;
        }

        public void Execute(object parameter)
        {
            VM.Register();
        }
    }
}
