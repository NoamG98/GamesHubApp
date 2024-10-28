using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FlappyBirdGame
{
    public partial class FlappyBirdGameWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        int gravity = 5;
        double score = 0;
        double pipeSpeed = 3;
        Rect flappyBirdRect;
        bool gameover = false;
        double[] initialPipePositions; // מיקום התחלתי של כל צינור
        const double pipeSpacing = 300; // המרחק בין הצינורות

        public FlappyBirdGameWindow()
        {
            InitializeComponent();
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += gameEngine;
            initialPipePositions = MyCanvas.Children.OfType<Image>()
                                 .Where(x => (string)x.Tag == "obs")
                                 .Select(x => Canvas.GetLeft(x))
                                 .ToArray();
            startGame();
        }

        private void Canvas_KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                gravity = -8;
            }
        }

        private void Canvas_KeyisUp(object sender, KeyEventArgs e)
        {
            gravity = 5;
        }

        private void startGame()
        {
            score = 0;
            pipeSpeed = 3;
            Canvas.SetTop(flappyBird, 100);
            gameover = false;

            // מיקומי צינורות התחלתיים לפי ההגדרה המקדמית במערך
            var pipes = MyCanvas.Children.OfType<Image>().Where(x => (string)x.Tag == "obs").ToArray();
            for (int i = 0; i < pipes.Length; i++)
            {
                Canvas.SetLeft(pipes[i], initialPipePositions[i]);
            }

            gameTimer.Start();
        }

        private void gameEngine(object sender, EventArgs e)
        {
            scoreText.Content = "Score: " + score;
            flappyBirdRect = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width, flappyBird.Height);

            // עדכון מיקום הציפור לפי כוח הכבידה
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);

            if (Canvas.GetTop(flappyBird) + flappyBird.Height > 490 || Canvas.GetTop(flappyBird) < -30)
            {
                EndGame("Press R to Try Again");
            }

            // תנועה של הצינורות שמאלה ומחזוריות קבועה
            var pipes = MyCanvas.Children.OfType<Image>().Where(x => (string)x.Tag == "obs").ToArray();
            for (int i = 0; i < pipes.Length; i += 2) // מוודאים שכל זוג צינורות נע יחד
            {
                // עדכון מיקום של זוג הצינורות
                Canvas.SetLeft(pipes[i], Canvas.GetLeft(pipes[i]) - pipeSpeed);
                Canvas.SetLeft(pipes[i + 1], Canvas.GetLeft(pipes[i + 1]) - pipeSpeed);

                // יצירת מלבן לבדיקת התנגשות עם הצינורות
                Rect pipeRect = new Rect(Canvas.GetLeft(pipes[i]), Canvas.GetTop(pipes[i]), pipes[i].Width, pipes[i].Height);
                Rect pipeRect2 = new Rect(Canvas.GetLeft(pipes[i + 1]), Canvas.GetTop(pipes[i + 1]), pipes[i + 1].Width, pipes[i + 1].Height);

                // בדיקת התנגשות של הציפור עם כל אחד מהצינורות
                if (flappyBirdRect.IntersectsWith(pipeRect) || flappyBirdRect.IntersectsWith(pipeRect2))
                {
                    EndGame("Game Over! Press R to Try Again");
                }

                // בדיקה אם הצינור יצא מהמסך והחזרתו לנקודת התחלה
                if (Canvas.GetLeft(pipes[i]) < -100)
                {
                    double rightmostPipePosition = pipes.Max(x => Canvas.GetLeft(x)) + pipeSpacing;
                    Canvas.SetLeft(pipes[i], rightmostPipePosition);
                    Canvas.SetLeft(pipes[i + 1], rightmostPipePosition);
                    score += 0.5;
                }
            }
        }

        private void EndGame(string message)
        {
            gameTimer.Stop();
            gameover = true;
            scoreText.Content += "   " + message;
        }
    }
}
