﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GamesHubApp
{
    public partial class PacmanGameApp : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private Rectangle player = new Rectangle();
        private List<Rectangle> coins = new List<Rectangle>();
        private List<Rectangle> enemies = new List<Rectangle>();
        private List<Rectangle> walls = new List<Rectangle>();
        private int score;
        private bool gameOver;

        private const int EntitySize = 40;
        private const int GameAreaWidth = 680;
        private const int GameAreaHeight = 600;

        private readonly char[,] map = new char[,]
        {
            { '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?' },
            { '?', '.', '.', '.', '?', '.', '.', '.', '.', '.', '.', '.', '.', '?', '.', '.', '.', '?' },
            { '?', '.', '?', '.', '?', '.', '?', '?', '?', '?', '.', '?', '.', '?', '.', '?', '.', '?' },
            { '?', '.', '.', '.', '.', '.', '.', '.', '?', '.', '.', '.', '.', '.', '.', '.', '.', '?' },
            { '?', '?', '?', '.', '?', '?', '?', '?', '?', '?', '?', '?', '.', '?', '?', '?', '?', '?' },
            { '?', '.', '.', '.', '?', '.', '.', '.', '.', '.', '.', '.', '.', '?', '.', '.', '.', '?' },
            { '?', '.', '?', '.', '?', '.', '?', '?', '$', '?', '?', '.', '.', '?', '.', '?', '.', '?' },
            { '?', '.', '.', '.', '.', '.', '.', '.', '?', '.', '.', '.', '.', '.', '.', '.', '.', '?' },
            { '?', '.', '?', '.', '?', '.', '?', '?', '?', '?', '?', '?', '.', '?', '.', '?', '.', '?' },
            { '?', '.', '.', '.', '?', '.', '.', '@', '@', '@', '.', '.', '.', '?', '.', '.', '.', '?' },
            { '?', '?', '?', '.', '?', '?', '?', '?', '?', '?', '?', '?', '.', '?', '?', '?', '?', '?' },
            { '?', '.', '.', '.', '.', '.', '.', '.', '?', '.', '.', '.', '.', '.', '.', '.', '.', '?' },
            { '?', '.', '?', '.', '?', '.', '?', '.', '?', '?', '.', '?', '.', '?', '.', '?', '.', '?' },
            { '?', '.', '.', '.', '?', '.', '.', '.', 'C', '.', '.', '.', '.', '?', '.', '.', '.', '?' },
            { '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?', '?' },
        };

        public PacmanGameApp()
        {
            InitializeComponent();
            SetupGame();
        }

        private void SetupGame()
        {
            Width = GameAreaWidth + 20;
            Height = GameAreaHeight + 60;
            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Background = Brushes.Black;

            gameOver = false;
            score = 0;

            InitializeGameArea();
            InitializeMap();
            InitializePlayer();
            InitializeEnemies();

            gameTimer.Interval = TimeSpan.FromMilliseconds(100);
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            this.KeyDown += OnKeyDown;
        }

        private void InitializeGameArea()
        {
            GameCanvas.Width = GameAreaWidth;
            GameCanvas.Height = GameAreaHeight;
            GameCanvas.Background = Brushes.Black;
            GameCanvas.Margin = new Thickness(10);
        }

        private void InitializeMap()
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    char mapChar = map[row, col];
                    double x = col * EntitySize;
                    double y = row * EntitySize;

                    if (mapChar == '?')
                    {
                        Rectangle wall = new Rectangle
                        {
                            Width = EntitySize,
                            Height = EntitySize,
                            Fill = Brushes.Gray
                        };
                        Canvas.SetLeft(wall, x);
                        Canvas.SetTop(wall, y);
                        walls.Add(wall);
                        GameCanvas.Children.Add(wall);
                    }
                    else if (mapChar == '.')
                    {
                        Rectangle coin = new Rectangle
                        {
                            Width = EntitySize,
                            Height = EntitySize,
                            Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\coin.gif", UriKind.Absolute))
                            }
                        };
                        Canvas.SetLeft(coin, x);
                        Canvas.SetTop(coin, y);
                        coins.Add(coin);
                        GameCanvas.Children.Add(coin);
                    }
                }
            }
        }

        private void InitializePlayer()
        {
            player.Width = EntitySize;
            player.Height = EntitySize;
            player.Fill = GetPacmanImage("RIGHT");


            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == 'C')
                    {
                        Canvas.SetLeft(player, col * EntitySize);
                        Canvas.SetTop(player, row * EntitySize);
                    }
                }
            }
            GameCanvas.Children.Add(player);
        }

        private void InitializeEnemies()
        {
            string[] enemyImages = {
                @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\pink_guy.gif",
                @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\red_guy.gif",
                @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\yellow_guy.gif"
            };
            Random rand = new Random();

            // Place enemies at positions marked by '@' in the map
            int enemyIndex = 0;
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == '@' && enemyIndex < enemyImages.Length)
                    {
                        Rectangle enemy = new Rectangle
                        {
                            Width = EntitySize,
                            Height = EntitySize,
                            Fill = new ImageBrush
                            {
                                ImageSource = new BitmapImage(new Uri(enemyImages[enemyIndex], UriKind.Absolute))
                            }
                        };

                        Canvas.SetLeft(enemy, col * EntitySize);
                        Canvas.SetTop(enemy, row * EntitySize);
                        enemies.Add(enemy);
                        GameCanvas.Children.Add(enemy);
                        enemyIndex++;
                    }
                }
            }
        }

        private void GameTick(object? sender, EventArgs e)
        {
            if (gameOver)
            {
                gameTimer.Stop();
                ShowGameOver();
                return;
            }

            MoveEnemies();
            CheckCollisions();
        }

        private void MoveEnemies()
        {
            Random rand = new Random();

            foreach (var enemy in enemies)
            {
                double left = Canvas.GetLeft(enemy);
                double top = Canvas.GetTop(enemy);

                int direction = rand.Next(1, 5);
                double newLeft = left, newTop = top;

                switch (direction)
                {
                    case 1:
                        newTop = top - EntitySize;
                        break;
                    case 2:
                        newTop = top + EntitySize;
                        break;
                    case 3:
                        newLeft = left - EntitySize;
                        break;
                    case 4:
                        newLeft = left + EntitySize; 
                        break;
                }


                if (IsWalkable(newLeft, newTop) && IsOnPath(newLeft, newTop))
                {
                    Canvas.SetLeft(enemy, newLeft);
                    Canvas.SetTop(enemy, newTop);
                }
            }
        }

        private bool IsWalkable(double x, double y)
        {
            if (x < 0 || y < 0 || x >= GameAreaWidth || y >= GameAreaHeight)
            {
                return false;
            }

            foreach (var wall in walls)
            {
                double wallLeft = Canvas.GetLeft(wall);
                double wallTop = Canvas.GetTop(wall);

                if (x == wallLeft && y == wallTop)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsOnPath(double x, double y)
        {

            int col = (int)(x / EntitySize);
            int row = (int)(y / EntitySize);

            return map[row, col] == '.' || map[row, col] == '$' || map[row, col] == 'C';
        }

        private void CheckCollisions()
        {
            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            foreach (var coin in coins.ToArray())
            {
                Rect coinHitBox = new Rect(Canvas.GetLeft(coin), Canvas.GetTop(coin), coin.Width, coin.Height);

                if (playerHitBox.IntersectsWith(coinHitBox) || playerHitBox.Contains(Canvas.GetLeft(coin), Canvas.GetTop(coin)))
                {
                    score++;
                    GameCanvas.Children.Remove(coin);
                    coins.Remove(coin);
                    break;
                }
            }

            foreach (var enemy in enemies)
            {
                Rect enemyHitBox = new Rect(Canvas.GetLeft(enemy), Canvas.GetTop(enemy), enemy.Width, enemy.Height);

                if (playerHitBox.IntersectsWith(enemyHitBox))
                {
                    gameOver = true;
                }
            }


            if (coins.Count == 0)
            {
                int col = (int)(Canvas.GetLeft(player) / EntitySize);
                int row = (int)(Canvas.GetTop(player) / EntitySize);
                if (map[row, col] == '$')
                {
                    gameOver = true;
                    ShowVictoryMessage();
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            double left = Canvas.GetLeft(player);
            double top = Canvas.GetTop(player);

            double newLeft = left, newTop = top;

            switch (e.Key)
            {
                case Key.Up:
                    newTop = top - EntitySize;
                    player.Fill = GetPacmanImage("UP");
                    break;
                case Key.Down:
                    newTop = top + EntitySize;
                    player.Fill = GetPacmanImage("DOWN");
                    break;
                case Key.Left:
                    newLeft = left - EntitySize;
                    player.Fill = GetPacmanImage("LEFT");
                    break;
                case Key.Right:
                    newLeft = left + EntitySize;
                    player.Fill = GetPacmanImage("RIGHT");
                    break;
            }

  
            if (IsWalkable(newLeft, newTop) && IsOnPath(newLeft, newTop))
            {
                Canvas.SetLeft(player, newLeft);
                Canvas.SetTop(player, newTop);
            }
        }

        private ImageBrush GetPacmanImage(string direction)
        {
            string imagePath = direction switch
            {
                "UP" => @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\Up.gif",
                "DOWN" => @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\down.gif",
                "LEFT" => @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\left.gif",
                "RIGHT" => @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\right.gif",
                _ => @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\Pacmanimages\right.gif",
            };

            return new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute))
            };
        }

        private void ShowGameOver()
        {
            MessageBoxResult result = MessageBox.Show($"Game Over! Your score: {score}\nDo you want to play again?", "Game Over", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ResetGame();
            }
            else
            {
                var gamesPage = new GamesPage();
                Application.Current.MainWindow.Content = gamesPage;
                this.Close();
            }
        }

        private void ShowVictoryMessage()
        {
            MessageBoxResult result = MessageBox.Show($"Congratulations! You've reached the goal with all the coins! Your score: {score}\nDo you want to play again?", "Victory!", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ResetGame();
            }
            else
            {
                var gamesPage = new GamesPage();
                Application.Current.MainWindow.Content = gamesPage;
                this.Close();
            }
        }

        private void ResetGame()
        {
            GameCanvas.Children.Clear();
            coins.Clear();
            enemies.Clear();
            walls.Clear();
            SetupGame();
        }
    }
}
