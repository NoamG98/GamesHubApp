using System.Windows;

namespace GamesHubApp
{
    public partial class ToDoListLandingPage : Window
    {
        public ToDoListLandingPage()
        {
            InitializeComponent();
        }


        private void OpenToDoList(object sender, RoutedEventArgs e)
        {
            var todoListWindow = new TodoListProj.ToDoList();
            todoListWindow.ShowDialog();
        }

        private void BackToGamesPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
