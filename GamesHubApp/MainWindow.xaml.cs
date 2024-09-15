using System.Windows;
using System.Windows.Navigation;

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
            GamesPage gamePage = new GamesPage();
            NavigationWindow window = new NavigationWindow();
            window.Content = gamePage;
            window.Show();
            this.Close();
        }
    }
}
