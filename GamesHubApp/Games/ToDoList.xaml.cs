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
            EditButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
        }

        // Adding a new task to the list
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string task = TaskInput.Text;
            if (!string.IsNullOrWhiteSpace(task))
            {
                // Create a StackPanel to hold the task and the checkbox
                StackPanel taskPanel = new StackPanel { Orientation = Orientation.Horizontal };

                // Create a checkbox to mark task as complete
                CheckBox completeCheckBox = new CheckBox();
                completeCheckBox.Margin = new Thickness(0, 0, 10, 0);
                completeCheckBox.Checked += MarkAsComplete_Click; // Add event handler for completing task

                // Create a TextBlock for the task name
                TextBlock taskText = new TextBlock { Text = task, VerticalAlignment = VerticalAlignment.Center };

                // Add checkbox and task text to the task panel
                taskPanel.Children.Add(completeCheckBox);
                taskPanel.Children.Add(taskText);

                // Create a ListBoxItem that contains the taskPanel and set the Tag property
                ListBoxItem taskItem = new ListBoxItem { Content = taskPanel, Tag = task };

                // Add the task item to the list
                TaskList.Items.Add(taskItem);

                // Clear the input field and reset placeholder visibility
                TaskInput.Clear();
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please enter a valid task.");
            }
        }

        // Edit selected task
        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is ListBoxItem selectedItem)
            {
                if (selectedItem.Content is StackPanel taskPanel)
                {
                    TextBlock? taskText = taskPanel.Children.OfType<TextBlock>().FirstOrDefault();
                    if (taskText != null)
                    {
                        string newTask = TaskInput.Text;
                        if (!string.IsNullOrWhiteSpace(newTask))
                        {
                            taskText.Text = newTask;
                            selectedItem.Tag = newTask;  // Update the task in the tag
                            TaskInput.Clear();
                            PlaceholderTextBlock.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid task.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task to edit.");
            }
        }

        // Mark task as complete
        private void MarkAsComplete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Parent is StackPanel taskPanel)
            {
                TextBlock? taskText = taskPanel.Children.OfType<TextBlock>().FirstOrDefault();
                if (taskText != null)
                {
                    if (!taskText.Text.EndsWith(" ✔"))
                    {
                        taskText.Text += " ✔"; // Add check mark when task is completed
                        checkBox.IsEnabled = false; // Disable the checkbox so it can't be checked multiple times
                    }
                }
            }
        }

        // Remove selected task
        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is ListBoxItem selectedItem)
            {
                TaskList.Items.Remove(selectedItem);
            }
            else
            {
                MessageBox.Show("Please select a task to remove.");
            }
        }

        // Handle the event when a task is selected
        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                EditButton.Visibility = Visibility.Visible;  // Show the edit button
                DeleteButton.Visibility = Visibility.Visible; // Show the delete button

                if (TaskList.SelectedItem is ListBoxItem selectedItem)
                {
                    // Display the task in the input box for editing
                    TaskInput.Text = selectedItem.Tag?.ToString() ?? string.Empty; // Check for null here
                }
            }
            else
            {
                EditButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
            }
        }

        private void TaskInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderTextBlock.Visibility = string.IsNullOrEmpty(TaskInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
