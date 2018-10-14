using System;
using System.Collections.Generic;
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
        SpeechRecognitionEngine recognizer;
        public NotesWindow()
        {
            InitializeComponent();

            var currentCulture = (from r in SpeechRecognitionEngine.InstalledRecognizers()
                                  where r.Culture.Equals(Thread.CurrentThread.CurrentUICulture)
                                  select r).FirstOrDefault();

            recognizer = new SpeechRecognitionEngine(currentCulture);

            GrammarBuilder builder = new GrammarBuilder();
            builder.AppendDictation();
            Grammar grammar = new Grammar(builder);

            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double> { 7, 8, 9, 10, 11, 12, 14, 16, 20, 28, 36, 48, 72 };
            fontSizeComboBox.ItemsSource = fontSizes;
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedTest = e.Result.Text;
            var newBlockItem = new Paragraph(new Run(recognizedTest));

            contentRichTextBox.Document.Blocks.Add(newBlockItem);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
        //private void SpeechButton_Click(object sender, RoutedEventArgs e)
        //{
        //    bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
        //    if (!isButtonEnabled)
        //    {
        //        recognizer.RecognizeAsync(RecognizeMode.Multiple);
        //    }
        //    else
        //    {
        //        recognizer.RecognizeAsyncStop();
        //    }
        //}

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
    }
}
