using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace DailyPlanner
{
    public partial class MainWindow : Window
    {
        private List<Note> notes;
        private string filePath = "notes.json";

        public DateTime SelectedDate { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedDate = DateTime.Today;
            datePicker.SelectedDate = SelectedDate;
            LoadNotes();
            UpdateNotesList();
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
            else
            {
                notes = new List<Note>();
            }
        }

        private void SaveNotes()
        {
            string json = JsonConvert.SerializeObject(notes);
            File.WriteAllText(filePath, json);
        }

        private void UpdateNotesList()
        {
            List<Note> notesForSelectedDate = notes.FindAll(note => note.DueDate.Date == SelectedDate.Date);
            notesList.ItemsSource = notesForSelectedDate;
        }

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = titleTextBox.Text,
                Description = descriptionTextBox.Text,
                DueDate = SelectedDate
            };
            notes.Add(newNote);
            SaveNotes();
            UpdateNotesList();
        }

        private void editNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (notesList.SelectedItem is Note selectedNote)
            {
                selectedNote.Title = titleTextBox.Text;
                selectedNote.Description = descriptionTextBox.Text;
                SaveNotes();
                UpdateNotesList();
            }
        }

        private void deleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (notesList.SelectedItem is Note selectedNote)
            {
                notes.Remove(selectedNote);
                SaveNotes();
                UpdateNotesList();
            }
        }

        private void notesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (notesList.SelectedItem is Note selectedNote)
            {
                titleTextBox.Text = selectedNote.Title;
                descriptionTextBox.Text = selectedNote.Description;
            }
        }

        private void datePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedDate = datePicker.SelectedDate ?? DateTime.Today;
            UpdateNotesList();
        }
    }

    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
