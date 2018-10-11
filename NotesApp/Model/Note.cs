using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Model
{
    public class Note : INotifyPropertyChanged
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

        private int notebookId;
        [Indexed]
        public int NotebookId
        {
            get { return notebookId; }
            set {
                notebookId = value;
                OnNotifyPropertyChanged("NotebookId");
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set {
                title = value;
                OnNotifyPropertyChanged("Title");
            }
        }

        private DateTime createdTime;

        public DateTime CreatedTime
        {
            get { return createdTime; }
            set {
                createdTime = value;
                OnNotifyPropertyChanged("CreatedTime");
            }
        }

        private DateTime updatedTime;

        public DateTime UpdatedTime
        {
            get { return updatedTime; }
            set {
                updatedTime = value;
                OnNotifyPropertyChanged("UpdatedTime");
            }
        }

        private string  fileLocation;

        public string FileLocation
        {
            get { return fileLocation; }
            set {
                fileLocation = value;
                OnNotifyPropertyChanged("FileLocation");
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
