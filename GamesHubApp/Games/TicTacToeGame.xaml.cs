using System;
using System.Windows;
using System.Windows.Controls;

namespace GamesHubApp
{
    public partial class TicTacToeGame : Window
    {
        private char[,] board = new char[3, 3];
        private char currentPlayer = 'X';

        public TicTacToeGame()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && string.IsNullOrEmpty(button.Content?.ToString()))
            {
                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);

                board[row, col] = currentPlayer;
                button.Content = currentPlayer.ToString();

                if (CheckWin())
                {
                    MessageBox.Show($"Player {currentPlayer} wins!");
                    ResetGame();
                }
                else if (CheckTie())
                {
                    MessageBox.Show("It's a tie!");
                    ResetGame();
                }
                else
                {
                    SwitchPlayer();
                    StatusLabel.Text = $"Player {currentPlayer}'s turn";
                }
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            InitializeBoard();
            foreach (var child in MainGrid.Children)
            {
                if (child is Button button && button.Name.StartsWith("Button"))
                {
                    button.Content = string.Empty;
                }
            }
            currentPlayer = 'X';
            StatusLabel.Text = "Player X's turn";
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
        }

        private bool CheckWin()
        {
            for (int i = 0; i < 3; i++)
            {
                if ((board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer) ||
                    (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer))
                {
                    return true;
                }
            }

            if ((board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) ||
                (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer))
            {
                return true;
            }

            return false;
        }

        private bool CheckTie()
        {
            foreach (char cell in board)
            {
                if (cell == ' ')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
