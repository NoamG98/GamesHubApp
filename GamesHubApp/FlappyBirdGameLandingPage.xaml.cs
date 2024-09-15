using System.Windows;

namespace GamesHubApp
{
    public partial class FlappyBirdGameLandingPage : Window
    {
        public FlappyBirdGameLandingPage()
        {
            InitializeComponent();
        }

        private void PlayFlappyBirdGame(object sender, RoutedEventArgs e)
        {
            var flappyBirdGameWindow = new FlappyBirdGame.FlappyBirdGameWindow();
            flappyBirdGameWindow.ShowDialog();
        }

        private void BackToGamesPage(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
