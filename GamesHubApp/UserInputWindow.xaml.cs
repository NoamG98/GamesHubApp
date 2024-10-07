using Microsoft.Win32;
using System;
using System.Windows;

namespace GamesHubApp
{
    public partial class UserInputWindow : Window
    {
        public string UserName { get; private set; }
        public string PicturePath { get; private set; }

        public UserInputWindow()
        {
            InitializeComponent();
        }

        private void BrowsePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                PicturePath = openFileDialog.FileName;
                PicturePathTextBlock.Text = PicturePath;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = NameTextBox.Text;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PicturePath))
            {
                MessageBox.Show("Please provide a name and select a picture.");
                return;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
