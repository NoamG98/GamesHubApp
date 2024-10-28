using System.Windows;

namespace GamesHubApp
{
    public partial class SnakeGameLandingPage : Window
    {
        public SnakeGameLandingPage()
        {
            InitializeComponent();
        }

        private void PlaySnakeGame(object sender, RoutedEventArgs e)
        {
            var snakeGameWindow = new SnakeGameApp.SnakeGame();
            snakeGameWindow.Show();
            this.Close();
        }

        private void BackToGamesPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
