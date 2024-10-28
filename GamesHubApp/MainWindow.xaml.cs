using System.Windows;

namespace GamesHubApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            WelcomePanel.Visibility = Visibility.Collapsed; 
            MainFrame.Content = new GamesPage(); 
        }
    }
}
