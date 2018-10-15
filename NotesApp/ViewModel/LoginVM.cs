using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class LoginVM
    {
        private User user;

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public LoginVM()
        {
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }

        public void Login()
        {
            using(SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                connection.CreateTable<User>();

                var user = connection.Table<User>().Where(u => u.UserName == User.UserName).FirstOrDefault();
                if(user.Password == User.Password)
                {
                    //TODO: establish a succesfull login
                }
            }
        }

        public void Register()
        {
            using(SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
            {
                connection.CreateTable<User>();
                bool succesfullInsertion = DatabaseHelper.Insert(User);
                if (succesfullInsertion)
                {
                    //TODO:
                }

            }
        }
    }
}
