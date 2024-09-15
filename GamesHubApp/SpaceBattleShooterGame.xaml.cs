using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GamesHubApp
{
    public partial class SpaceBattleShooterGame : Window
    {
        DispatcherTimer gametime = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemRemove = new List<Rectangle>();

        Random rand = new Random();
        int enemySpriteCounter = 0;
        int enemyCounter = 150;
        int playerSpeed = 10;
        int limit = 50;
        int score = 0;
        int damage = 0;

        Rect playerHitBox;
        private Rectangle player;

        public SpaceBattleShooterGame()
        {
            InitializeComponent();

            player = new Rectangle
            {
                Width = 50,
                Height = 50
            };
            Canvas.SetTop(player, 600);
            Canvas.SetLeft(player, 400);
            MyCanvas.Children.Add(player);

            gametime.Interval = TimeSpan.FromMilliseconds(15);
            gametime.Tick += GameLoop;
            gametime.Start();

            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("C:\\Users\\noam1\\OneDrive\\שולחן העבודה\\GamesHubApp\\image\\imageBattleShooter\\wallpaper.png", UriKind.Absolute)),
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 0.15, 0.15),
                ViewportUnits = BrushMappingMode.RelativeToBoundingBox
            };

            MyCanvas.Background = bg;

            ImageBrush playerImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("C:\\Users\\noam1\\OneDrive\\שולחן העבודה\\GamesHubApp\\image\\imageBattleShooter\\player.png", UriKind.Absolute))
            };

            player.Fill = playerImage;
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            gameEngine(sender, e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };

                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);

                MyCanvas.Children.Add(newBullet);
            }
        }

        private void makeEnemies()
        {
            try
            {
                ImageBrush enemySprite = new ImageBrush();
                enemySpriteCounter = rand.Next(1, 5);

                string imagePath = System.IO.Path.Combine(
                    @"C:\Users\noam1\OneDrive\שולחן העבודה\GamesHubApp\image\imageBattleShooter",
                    $"{enemySpriteCounter}.png");

                enemySprite.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 50,
                    Width = 56,
                    Fill = enemySprite
                };
                Canvas.SetTop(newEnemy, -100);
                Canvas.SetLeft(newEnemy, rand.Next(30, (int)MyCanvas.ActualWidth - 60));
                MyCanvas.Children.Add(newEnemy);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create enemy: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void gameEngine(object? sender, EventArgs e)
        {
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            enemyCounter--;
            scoreText.Content = "Score: " + score;
            damageText.Content = "Damaged: " + damage;

            if (enemyCounter < 0)
            {
                makeEnemies();
                enemyCounter = limit;
            }

            if (moveLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight && Canvas.GetLeft(player) + player.Width < MyCanvas.ActualWidth)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x.Tag?.ToString() == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemove.Add(x);
                    }

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y.Tag?.ToString() == "enemy")
                        {
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bullet.IntersectsWith(enemy))
                            {
                                itemRemove.Add(x);
                                itemRemove.Add(y);
                                score++;
                            }
                        }
                    }
                }

                if (x.Tag?.ToString() == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5 + score / 10);
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) + x.Height > MyCanvas.ActualHeight)
                    {
                        itemRemove.Add(x);
                        damage += 10;
                    }

                    if (playerHitBox.IntersectsWith(enemy))
                    {
                        damage += 5;
                        itemRemove.Add(x);
                    }
                }
            }

            if (damage > 99)
            {
                gametime.Stop();
                damageText.Content = "Damaged: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Well Done Star Captain!" + Environment.NewLine + "You have destroyed " + score + " Alien ships");
                PlayAgainButton.Visibility = Visibility.Visible;
            }

            foreach (Rectangle y in itemRemove)
            {
                MyCanvas.Children.Remove(y);
            }
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            damage = 0;
            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(player);
            MyCanvas.Children.Add(scoreText);
            MyCanvas.Children.Add(damageText);
            PlayAgainButton.Visibility = Visibility.Hidden;
            gametime.Start();
        }
    }
}
