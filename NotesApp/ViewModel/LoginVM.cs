using NotesApp.Model;
using NotesApp.ViewModel.Commands;
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

        public event EventHandler HasLoggedIn;

        public LoginVM()
        {
            User = new User();
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
        }

        public void Login()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                connection.CreateTable<User>();

                //Handle exception here
                var user = connection.Table<User>().Where(u => u.Username == User.Username).FirstOrDefault();
                if (user.Password == User.Password)
                {
                    App.UserId = user.Id.ToString();
                    HasLoggedIn(this, new EventArgs());
                }
            }
        }

        public void Register()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                connection.CreateTable<User>();
                bool succesfullInsertion = DatabaseHelper.Insert(User);
                if (succesfullInsertion)
                {
                    App.UserId = User.Id.ToString();
                    HasLoggedIn(this, new EventArgs());
                }

            }
        }
    }
}
