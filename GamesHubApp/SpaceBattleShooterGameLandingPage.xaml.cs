using System.Windows;

namespace GamesHubApp
{
    public partial class SpaceBattleShooterGameLandingPage : Window
    {
        public SpaceBattleShooterGameLandingPage()
        {
            InitializeComponent();
        }

        private void PlaySpaceBattleShooterGame(object sender, RoutedEventArgs e)
        {
   
            SpaceBattleShooterGame gameWindow = new SpaceBattleShooterGame();
            gameWindow.Show();
            this.Close();
        }

        private void BackToGamesPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
