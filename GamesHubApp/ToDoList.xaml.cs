using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TodoListProj
{
    public partial class ToDoList : Window
    {
        public ToDoList()
        {
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string task = TaskInput.Text;
            if (!string.IsNullOrWhiteSpace(task))
            {
                TaskList.Items.Add(new ListBoxItem { Content = task });
                TaskInput.Clear();
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please enter a valid task.");
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is ListBoxItem selectedItem)
            {
                string newTask = TaskInput.Text;
                if (!string.IsNullOrWhiteSpace(newTask))
                {
                    selectedItem.Content = newTask;
                    TaskInput.Clear();
                    PlaceholderTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please enter a valid task.");
                }
            }
            else
            {
                MessageBox.Show("Please select a task to edit.");
            }
        }

        private void MarkAsComplete_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is ListBoxItem selectedItem)
            {
                selectedItem.Content += " ✔";
                TaskList.Items.Remove(selectedItem);
                TaskList.Items.Add(selectedItem);
            }
            else
            {
                MessageBox.Show("Please select a task to mark as complete.");
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                TaskList.Items.Remove(TaskList.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a task to remove.");
            }
        }

        private void TaskInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderTextBlock.Visibility = string.IsNullOrEmpty(TaskInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
