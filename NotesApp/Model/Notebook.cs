using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class Notebook : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set {
                id = value;
                OnNotifyPropertyChanged("Id");
            }
        }

        private int userId;
        [Indexed]
        public int UserId
        {
            get { return userId; }
            set {
                userId = value;
                OnNotifyPropertyChanged("UserId");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set {
                name = value;
                OnNotifyPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
