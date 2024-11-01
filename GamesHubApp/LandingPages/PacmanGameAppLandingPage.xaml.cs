﻿using System.Windows;

namespace GamesHubApp
{
    public partial class PacmanGameAppLandingPage : Window
    {
        public PacmanGameAppLandingPage()
        {
            InitializeComponent();
        }

        private void PlayPacmanGame(object sender, RoutedEventArgs e)
        {
            var pacmanGameWindow = new PacmanGameApp();
            pacmanGameWindow.Show(); 
            this.Close();
        }

        private void BackToGamesPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
