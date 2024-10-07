using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GamesHubApp.Properties;

namespace GamesHubApp
{
    public partial class GamesPage : Page
    {
        private DispatcherTimer timer;

        public GamesPage()
        {
            InitializeComponent();
            PromptUserDetails(); // Prompt for user details when the page loads
            LoadUserDetails(); // Load saved user details
            InitializeTimer(); // Start the timer for updating date and time
        }

        private void PromptUserDetails()
        {
            if (string.IsNullOrEmpty(Settings.Default.UserName) || string.IsNullOrEmpty(Settings.Default.ProfilePicturePath))
            {
                // If user details are not saved, prompt for them
                UserInputWindow userInputWindow = new UserInputWindow();
                if (userInputWindow.ShowDialog() == true)
                {
                    SaveUserDetails(userInputWindow.UserName, userInputWindow.PicturePath);
                }
            }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateDateTime;
            timer.Start();
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("F"); // Full date and time pattern
        }

        private void LoadUserDetails()
        {
            // Load user name and profile picture from local storage
            if (!string.IsNullOrEmpty(Settings.Default.UserName))
            {
                UserNameTextBlock.Text = Settings.Default.UserName;
            }

            if (File.Exists(Settings.Default.ProfilePicturePath))
            {
                UserProfilePicture.Source = new BitmapImage(new Uri(Settings.Default.ProfilePicturePath));
            }
        }

        private void SaveUserDetails(string userName, string profilePicturePath)
        {
            // Save user details to local storage
            Settings.Default.UserName = userName;
            Settings.Default.ProfilePicturePath = profilePicturePath;
            Settings.Default.Save();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear user details from local storage
            Settings.Default.UserName = string.Empty;
            Settings.Default.ProfilePicturePath = string.Empty;
            Settings.Default.Save();

            // Show the login prompt again
            PromptUserDetails();

            // Reset the displayed user info
            UserNameTextBlock.Text = string.Empty;
            UserProfilePicture.Source = null;
        }

        // שאר המתודות עבור המשחקים
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
