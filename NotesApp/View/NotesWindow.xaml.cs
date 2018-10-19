using NotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM viewModel;

        public NotesWindow()
        {
            InitializeComponent();

            viewModel = new NotesVM();
            Container.DataContext = viewModel;
            viewModel.SelectedNoteChanged += ViewModel_SelectedNoteChanged;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double> { 7, 8, 9, 10, 11, 12, 14, 16, 20, 28, 36, 48, 72 };
            fontSizeComboBox.ItemsSource = fontSizes;
        }

        private void ViewModel_SelectedNoteChanged(object sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear(); // clear the texbox when selctedNoteChanged

            string fileLocation = viewModel.SelectedNote.FileLocation;
            bool theNoteDoesNotExist = string.IsNullOrEmpty(fileLocation);
            if (!theNoteDoesNotExist)
            {
                FileStream fileStream = new FileStream(fileLocation, FileMode.Open);

                TextPointer docStart = contentRichTextBox.Document.ContentStart;
                TextPointer docEnd = contentRichTextBox.Document.ContentEnd;
                TextRange allTheContentRange = new TextRange(docStart, docEnd);
                allTheContentRange.Load(fileStream, DataFormats.Rtf);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextPointer startPosition = contentRichTextBox.Document.ContentStart;
            TextPointer endPosition = contentRichTextBox.Document.ContentEnd;
            int amountOfChar = new TextRange(startPosition, endPosition).Text.Length;

            statusBarTextBlock.Text = $"Note length = {amountOfChar} characters";
        }


        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonEnabled)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
            
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedFontWeightState = 
                contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BoldButton.IsChecked = (selectedFontWeightState != DependencyProperty.UnsetValue) &&
                (selectedFontWeightState.Equals(FontWeights.Bold));

            var selectedFontStyleState =
                contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ItalicButton.IsChecked = (selectedFontStyleState != DependencyProperty.UnsetValue) &&
                (selectedFontStyleState.Equals(FontStyles.Italic));

            var selectedFontDecorationState =
                contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (selectedFontDecorationState != DependencyProperty.UnsetValue) &&
                (selectedFontDecorationState.Equals(TextDecorations.Underline));

            fontFamilyComboBox.SelectedItem =
                contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);

            fontSizeComboBox.Text =
                (contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();

        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonEnabled)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }

        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonEnabled)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                object noTextDecorationObject =
                    contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                TextDecorationCollection noTextDecoration;
                ( noTextDecorationObject as TextDecorationCollection).TryRemove(TextDecorations.Underline, out noTextDecoration);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, noTextDecoration);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            int noteId = viewModel.SelectedNote.Id;
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, $"{noteId}.rtf");
            viewModel.SelectedNote.FileLocation = rtfFile;

            FileStream fileStream = new FileStream(rtfFile, FileMode.Create);
            TextPointer docStart = contentRichTextBox.Document.ContentStart;
            TextPointer docEnd = contentRichTextBox.Document.ContentEnd;
            TextRange completeDoc = new TextRange(docStart, docEnd);
            completeDoc.Save(fileStream, DataFormats.Rtf);

            viewModel.UpdateSelectedNote();
        }
    }
}
