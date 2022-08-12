﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace LogBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double zoomScale = 1;
        private double incrementation = 0.1;

        bool match = true;

        string filePath = "";
        string titleName = "Untitled";
        string documentContent = "";

        bool showStatusBar = true;
        bool wordWrap = true;
        private object e;

        private string DocumentContent
        {
            get => documentContent;
            set => documentContent = value;

        }

        private string TitleName
        {
            get => titleName;
            set => titleName = value;
        }

        private string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        private bool Match
        {
            get => match;
            set => match = value;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Open_FileDialog();
        }

        /// <summary>
        /// Open the OpenFileDialoge
        /// </summary>
        private void Open_FileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // OpenFileDialog dialoge
            ofd.ShowDialog();

            // Make sure the ofd has a value selected
            try
            {
                if (ofd.FileName != null)
                {
                    // Path to document
                    FilePath = ofd.FileName;
                    // Document name
                    TitleName = ofd.SafeFileName;
                    // if the document matches the textbox
                    Match = true;

                    // Stream Reader to read the content of the document
                    StreamReader FileReader = new StreamReader(FilePath);
                    // Add the content of the document to the DocumentContent
                    DocumentContent = FileReader.ReadToEnd();
                    // Add the DocumentContent to the TextArea
                    TextArea.Text = DocumentContent;
                    // Close stream
                    FileReader.Close();
                }
            }
            catch
            {
            }
        }

        private void SaveDocument()
        {
            if (filePath != "")
            {
                //Save file stream
                StreamWriter SaveFileStream = new StreamWriter(FilePath);
                //Assign textbox content to documentContent
                DocumentContent = TextArea.Text;
                //Write documentContent to file
                SaveFileStream.Write(DocumentContent);
                //Close stream
                SaveFileStream.Close();

                Match = true;
            }
            else
            {
                SaveAs();
            }

            TitleManager();
        }

        private void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File (*.txt)|ALL (*.*)";
            sfd.ShowDialog();

            if(sfd.FileName != "")
            {
                FilePath = sfd.FileName;
                TitleName = sfd.SafeFileName;
                StreamWriter sw = new StreamWriter(FilePath);
                DocumentContent = TextArea.Text;
                sw.Write(DocumentContent);
                sw.Close();

                Match = true;
            }

        }

        private void Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveDocument();
        }

        private void TextArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TextArea.Text != DocumentContent)
            {
                //Add strix to beginning of document title
                match = false;
            } else
            {
                match = true;
            }
            TitleManager();
        }

        private void TitleManager()
        {
            if(match)
            {
                Title = TitleName + " - " + "NotepadClone";
            }
            else
            {
                Title = "*" + TitleName + " - " + "Notepad Clone";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (showStatusBar)
                StatusBar.Visibility = Visibility.Visible;
            else
            {
                StatusBar.Visibility = Visibility.Collapsed;
            }
            TitleManager();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!match)
            {
                e.Cancel = true;
                if(MessageBox.Show("You haven't saved your progress yet, would you like to?",
                    "Save Document",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SaveDocument();
                    Environment.Exit(0);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private void NewWindow_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to do that!" + ex.Message);
            }
        }

        private void NewDocument()
        {
            TitleName = "Untitled";
            FilePath = "";
            DocumentContent = "";
            Match = true;
            TextArea.Text = "";
        }

        private void NewDocument_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!Match)
            {
                if (MessageBox.Show("You haven't saved your progress yet, would you like to?",
                    "Save Document",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SaveDocument();
                    NewDocument();
                }
                else
                {
                    NewDocument();
                }
            }
            else
            {
                NewDocument();
            }
        }

        private void PageSetup_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
        }

        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Print_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.ShowDialog();
        }

        private void SaveAs_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void Undo_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(TextArea.CanUndo)
            {
                TextArea.Undo();
            }
        }

        private void Cut_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextArea.Cut();
        }

        private void Copy_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextArea.Copy();
        }

        private void Paste_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextArea.Paste();
        }

        private void Delete_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextArea.Text = TextArea.Text.Remove(TextArea.SelectionStart, TextArea.SelectionLength);
        }

        private void SelectAll_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextArea.SelectAll();
        }

        private void TimeDate_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextArea.Text = TextArea.Text.Insert(TextArea.CaretIndex, DateTime.Now.ToString());
        }

        private void Replace_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ZoomIn_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            zoomScale += incrementation;
        }

        private void ZoomOut_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            zoomScale -= incrementation;
        }

        private void ZoomRestore_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            zoomScale = 1;
        }

        private void StatusBarManager()
        {
            if (showStatusBar)
                showStatusBar = false;
            else
                showStatusBar = true;

            if (showStatusBar)
                StatusBar.Visibility = Visibility.Visible;
            else
                StatusBar.Visibility = Visibility.Collapsed;
        }

        private void ShowStatusBar_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            StatusBarManager();
        }

        private void WordWrapManager()
        {
            if (wordWrap)
                wordWrap = false;
            else
                wordWrap = true;

            if (wordWrap)
                TextArea.TextWrapping = TextWrapping.Wrap;
            else
                TextArea.TextWrapping = TextWrapping.NoWrap;
        }

        private void WordWrap_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WordWrapManager();
        }

        private void TextArea_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
