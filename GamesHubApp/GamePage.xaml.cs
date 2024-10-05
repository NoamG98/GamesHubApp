using System.Windows;
using System.Windows.Controls;

namespace GamesHubApp
{
    public partial class GamesPage : Page
    {
        public GamesPage()
        {
            InitializeComponent();
        }

        private void SpaceBattleShooterGame_Click(object sender, RoutedEventArgs e)
        {
            var spaceBattleShooterGameLandingPage = new SpaceBattleShooterGameLandingPage();
            spaceBattleShooterGameLandingPage.Show();
        }

        private void FlappyBirdGame_Click(object sender, RoutedEventArgs e)
        {
            var flappyBirdGameLandingPage = new FlappyBirdGameLandingPage();
            flappyBirdGameLandingPage.Show();
        }

        private void SnakeGame_Click(object sender, RoutedEventArgs e)
        {
            var snakeGameLandingPage = new SnakeGameLandingPage();
            snakeGameLandingPage.Show();
        }

        private void PacmanGameButton_Click(object sender, RoutedEventArgs e)
        {
            var pacmanGameLandingPage = new PacmanGameAppLandingPage();
            pacmanGameLandingPage.Show();
        }

        private void TicTacToeGameButton_Click(object sender, RoutedEventArgs e)
        {
            var ticTacToeGameLandingPage = new TicTacToeGameLandingPage();
            ticTacToeGameLandingPage.Show();
        }

        private void ToDoListButton_Click(object sender, RoutedEventArgs e)
        {
            var toDoListLandingPage = new ToDoListLandingPage();
            toDoListLandingPage.Show();
        }

        private void CountriesProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var countriesProjectLandingPage = new CountriesProjectLandingPage();
            countriesProjectLandingPage.Show();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

            Window.GetWindow(this)?.Close();
        }
    }
}
