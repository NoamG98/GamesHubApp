using GamesHubApp;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SnakeGameApp
{
    public partial class SnakeGame : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private List<Point> snakeParts = new List<Point>();
        private Point snakeDirection;
        private Point applePosition;
        private int score;

        private const int SnakeSize = 20;

        public SnakeGame()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            snakeParts = new List<Point> { new Point(512, 384) };
            snakeDirection = new Point(SnakeSize, 0);
            applePosition = GetRandomPosition();
            score = 0;

            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            DrawGame();
            this.KeyDown += OnKeyDown;
        }

        private void GameTick(object? sender, EventArgs e)
        {
            MoveSnake();
            if (CheckCollisions())
            {
                EndGame();
            }
            else
            {
                DrawGame();
            }
        }

        private void MoveSnake()
        {
            Point newHead = new Point(snakeParts[0].X + snakeDirection.X, snakeParts[0].Y + snakeDirection.Y);
            snakeParts.Insert(0, newHead);

            if (Math.Abs(newHead.X - applePosition.X) < SnakeSize && Math.Abs(newHead.Y - applePosition.Y) < SnakeSize)
            {
                score++;
                applePosition = GetRandomPosition();
            }
            else
            {
                snakeParts.RemoveAt(snakeParts.Count - 1);
            }
        }

        private bool CheckCollisions()
        {
            Point head = snakeParts[0];

            if (head.X < 0 || head.Y < 0 || head.X >= 1024 || head.Y >= 768)
            {
                return true;
            }

            for (int i = 1; i < snakeParts.Count; i++)
            {
                if (head == snakeParts[i])
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawGame()
        {
            GameCanvas.Children.Clear();

            for (int i = 0; i < snakeParts.Count; i++)
            {
                Image snakePartImage = new Image
                {
                    Width = SnakeSize,
                    Height = SnakeSize,
                    Source = GetSnakeImage(i)
                };
                Canvas.SetLeft(snakePartImage, snakeParts[i].X);
                Canvas.SetTop(snakePartImage, snakeParts[i].Y);
                GameCanvas.Children.Add(snakePartImage);
            }

            Image appleImage = new Image
            {
                Width = SnakeSize,
                Height = SnakeSize,
                Source = new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\apple.png", UriKind.Absolute))
            };
            Canvas.SetLeft(appleImage, applePosition.X);
            Canvas.SetTop(appleImage, applePosition.Y);
            GameCanvas.Children.Add(appleImage);
        }

        private BitmapImage GetSnakeImage(int index)
        {
            if (index == 0)
            {
                if (snakeDirection.Y == -SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\head_up.png", UriKind.Absolute));
                if (snakeDirection.Y == SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\head_down.png", UriKind.Absolute));
                if (snakeDirection.X == -SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\head_left.png", UriKind.Absolute));
                if (snakeDirection.X == SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\head_right.png", UriKind.Absolute));
            }
            else if (index == snakeParts.Count - 1)
            {
                Point tailDirection = new Point(snakeParts[index - 1].X - snakeParts[index].X, snakeParts[index - 1].Y - snakeParts[index].Y);
                if (tailDirection.Y == -SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\tail_down.png", UriKind.Absolute));
                if (tailDirection.Y == SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\tail_up.png", UriKind.Absolute));
                if (tailDirection.X == -SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\tail_right.png", UriKind.Absolute));
                if (tailDirection.X == SnakeSize) return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\tail_left.png", UriKind.Absolute));
            }
            else
            {
                Point previousPart = snakeParts[index - 1];
                Point nextPart = snakeParts[index + 1];
                Point differencePrev = new Point(previousPart.X - snakeParts[index].X, previousPart.Y - snakeParts[index].Y);
                Point differenceNext = new Point(nextPart.X - snakeParts[index].X, nextPart.Y - snakeParts[index].Y);

                if ((differencePrev.X == SnakeSize && differenceNext.X == -SnakeSize) || (differencePrev.X == -SnakeSize && differenceNext.X == SnakeSize))
                {
                    return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_horizontal.png", UriKind.Absolute));
                }
                if ((differencePrev.Y == SnakeSize && differenceNext.Y == -SnakeSize) || (differencePrev.Y == -SnakeSize && differenceNext.Y == SnakeSize))
                {
                    return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_vertical.png", UriKind.Absolute));
                }
                if ((differencePrev.X == SnakeSize && differenceNext.Y == SnakeSize) || (differencePrev.Y == SnakeSize && differenceNext.X == SnakeSize))
                {
                    return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_bottomright.png", UriKind.Absolute));
                }
                if ((differencePrev.X == -SnakeSize && differenceNext.Y == SnakeSize) || (differencePrev.Y == SnakeSize && differenceNext.X == -SnakeSize))
                {
                    return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_bottomleft.png", UriKind.Absolute));
                }
                if ((differencePrev.X == SnakeSize && differenceNext.Y == -SnakeSize) || (differencePrev.Y == -SnakeSize && differenceNext.X == SnakeSize))
                {
                    return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_topright.png", UriKind.Absolute));
                }
                if ((differencePrev.X == -SnakeSize && differenceNext.Y == -SnakeSize) || (differencePrev.Y == -SnakeSize && differenceNext.X == -SnakeSize))
                {
                    return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_topleft.png", UriKind.Absolute));
                }
            }

            return new BitmapImage(new Uri(@"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\SnakeImage\body_vertical.png", UriKind.Absolute));
        }

        private Point GetRandomPosition()
        {
            Random rand = new Random();
            int x = rand.Next(0, 1024 / SnakeSize) * SnakeSize;
            int y = rand.Next(0, 768 / SnakeSize) * SnakeSize;
            return new Point(x, y);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up when snakeDirection.Y == 0:
                    snakeDirection = new Point(0, -SnakeSize);
                    break;
                case Key.Down when snakeDirection.Y == 0:
                    snakeDirection = new Point(0, SnakeSize);
                    break;
                case Key.Left when snakeDirection.X == 0:
                    snakeDirection = new Point(-SnakeSize, 0);
                    break;
                case Key.Right when snakeDirection.X == 0:
                    snakeDirection = new Point(SnakeSize, 0);
                    break;
            }
        }

        private void EndGame()
        {
            gameTimer.Stop();
            MessageBoxResult result = MessageBox.Show($"Game Over! Your score: {score}\nDo you want to play again?", "Game Over", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                StartGame();
            }
            else
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
