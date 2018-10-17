using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.ViewModel
{
    public class NotesVM
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                ReadNotes();
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }

        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            ReadNotebooks();
            ReadNotes();
        }

        public void CreateNewNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New Notebook",
                UserId = int.Parse(App.UserId)
            };

            DatabaseHelper.Insert(newNotebook);

            ReadNotebooks();
        }

        public void CreateNewNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                Title = "New note"
            };

            DatabaseHelper.Insert(newNote);
            ReadNotes();
        }

        public void ReadNotebooks()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                //TODO: handle this exception (When a table of notebook does not exists)
                var notebooks = connection.Table<Notebook>().ToList();
                Notebooks.Clear();
                foreach (Notebook notebook in notebooks)
                {
                    Notebooks.Add(notebook);
                }
                SelectedNotebook = Notebooks.FirstOrDefault();
            }

        }

        public void ReadNotes()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DatabaseHelper.dbFile))
            {
                if (SelectedNotebook != null)
                {
                    //TODO: handle this exception (When a table of notebook does not exists)
                    var notes = connection.Table<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

                    Notes.Clear();
                    foreach (Note note in notes)
                    {
                        Notes.Add(note);
                    }
                }
            }
        }
    }
}
