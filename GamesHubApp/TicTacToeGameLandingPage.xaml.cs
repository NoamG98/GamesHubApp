using System.Windows;

namespace GamesHubApp
{
    public partial class TicTacToeGameLandingPage : Window
    {
        public TicTacToeGameLandingPage()
        {
            InitializeComponent();
        }

        private void PlayTicTacToeGame(object sender, RoutedEventArgs e)
        {
            TicTacToeGame ticTacToeGame = new TicTacToeGame();
            ticTacToeGame.Show();
            this.Close();
        }

        private void BackToGamePage(object sender, RoutedEventArgs e)
        {

            MainWindow gamesPage = new MainWindow();
            gamesPage.Show();
            this.Close();
        }
    }
}
