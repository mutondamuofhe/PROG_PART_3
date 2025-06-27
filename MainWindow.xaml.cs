using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PROG_PART_3.Models;

namespace PROG_PART_3
{
    public partial class MainWindow : Window
    {
        private readonly DataManager _dataManager;
        private List<QuizQuestion> _quizQuestions;
        private int _currentQuestionIndex;
        private int _quizScore;
        private bool _quizInProgress;

        public MainWindow()
        {
            InitializeComponent();
            _dataManager = DataManager.Instance;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Play greeting sound when the window loads
            play_sound greeting = new play_sound();

            // Initialize UI components
            InitializeChatPanel();
            InitializeTaskPanel();
            InitializeQuizPanel();
            InitializeLogPanel();

            // Add welcome message to chat
            AddChatMessage("Assistant", "Welcome to the Cybersecurity Awareness Assistant! How can I help you today?");
        }

        #region Navigation

        private void ChatTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Visible;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;
        }

        private void TaskTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Visible;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;

            // Refresh task list when switching to task tab
            RefreshTaskList();
        }

        private void QuizTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Visible;
            LogPanel.Visibility = Visibility.Collapsed;
        }

        private void LogTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Visible;

            // Refresh activity log when switching to log tab
            RefreshActivityLog();
        }

        #endregion

        #region Chat Panel Implementation

        private void InitializeChatPanel()
        {
            ChatInputTextBox.Focus();
        }

        private void AddChatMessage(string sender, string message)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm");
            string formattedMessage = "";

            if (sender == "User")
            {
                formattedMessage = $"[{timeStamp}] You: {message}\n";
                ChatHistoryTextBlock.Inlines.Add(new System.Windows.Documents.Run(formattedMessage) { FontWeight = FontWeights.Bold });
            }
            else
            {
                formattedMessage = $"[{timeStamp}] Assistant: {message}\n\n";
                ChatHistoryTextBlock.Inlines.Add(new System.Windows.Documents.Run(formattedMessage));

                // Log assistant response to activity log if it's a substantive interaction
                if (message.Length > 20 && !message.Contains("Welcome to"))
                {
                    _dataManager.AddActivityLog(ActivityLog.ActionTypes.ChatInteraction,
                        $"Chatbot provided information: {message.Substring(0, Math.Min(50, message.Length))}...");
                }
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void ChatInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void SendMessage()
        {
            string userInput = ChatInputTextBox.Text.Trim();

            if (string.IsNullOrEmpty(userInput))
                return;

            // Add user message to chat history
            AddChatMessage("User", userInput);

            // Process user input using NLP simulation
            ProcessUserInput(userInput);

            // Clear input box
            ChatInputTextBox.Text = string.Empty;
            ChatInputTextBox.Focus();
        }

        private void ProcessUserInput(string userInput)
        {
            // Check if user is requesting to add a task
            if (ChatResponse.ContainsTaskCommand(userInput))
            {
                // Extract task from message
                string taskTitle = ExtractTaskFromMessage(userInput);
                if (!string.IsNullOrEmpty(taskTitle))
                {
                    // Switch to the task panel
                    TaskTab_Click(this, new RoutedEventArgs());

                    // Populate the task input fields
                    TaskTitleTextBox.Text = taskTitle;
                    TaskDescriptionTextBox.Text = "Cybersecurity task";

                    // Respond to the user
                    AddChatMessage("Assistant", $"I've prepared a task with the title '{taskTitle}'. Please add any additional details in the Task Manager tab.");
                    return;
                }
            }

            // Check if user wants to see activity log
            if (ChatResponse.ContainsShowLogCommand(userInput))
            {
                // Switch to the log panel
                LogTab_Click(this, new RoutedEventArgs());

                // Respond to the user
                AddChatMessage("Assistant", "I've opened the activity log for you to review.");
                return;
            }

            // Check for mentions of quiz
            if (userInput.ToLower().Contains("quiz") || userInput.ToLower().Contains("test"))
            {
                // Switch to quiz panel
                QuizTab_Click(this, new RoutedEventArgs());

                // Respond to the user
                AddChatMessage("Assistant", "I've opened the Cybersecurity Knowledge Quiz for you. Click 'Start Quiz' when you're ready!");
                return;
            }

            // For all other inputs, get an appropriate response based on keywords
            string response = ChatResponse.GetResponse(userInput);
            AddChatMessage("Assistant", response);
        }

        private string ExtractTaskFromMessage(string message)
        {
            string lowerMessage = message.ToLower();

            string[] taskPrefixes = { "add task", "create task", "new task", "set reminder", "remind me" };

            foreach (string prefix in taskPrefixes)
            {
                if (lowerMessage.Contains(prefix))
                {
                    int startIdx = lowerMessage.IndexOf(prefix) + prefix.Length;
                    string taskPart = message.Substring(startIdx).Trim();

                    // If the task starts with certain words, remove them
                    string[] removeWords = { "to", "about", "for", ":", "-" };
                    foreach (string word in removeWords)
                    {
                        if (taskPart.StartsWith(word, StringComparison.OrdinalIgnoreCase))
                        {
                            taskPart = taskPart.Substring(word.Length).Trim();
                        }
                    }

                    // If we have a non-empty task, return it
                    if (!string.IsNullOrWhiteSpace(taskPart))
                    {
                        // Capitalize first letter for better presentation
                        return char.ToUpper(taskPart[0]) + taskPart.Substring(1);
                    }
                }
            }

            return string.Empty;
        }

        #endregion

        #region Task Panel Implementation

        private void InitializeTaskPanel()
        {
            // Initialize the task panel with current tasks
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = _dataManager.GetAllTasks();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleTextBox.Text.Trim();
            string description = TaskDescriptionTextBox.Text.Trim();
            DateTime? reminderDate = null;

            if (TaskReminderDatePicker.SelectedDate != null)
            {
                reminderDate = TaskReminderDatePicker.SelectedDate;
            }

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Add the task
            _dataManager.AddTask(title, description, reminderDate);

            // Clear the inputs
            TaskTitleTextBox.Text = string.Empty;
            TaskDescriptionTextBox.Text = string.Empty;
            TaskReminderDatePicker.SelectedDate = null;

            // Refresh the list
            RefreshTaskList();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int taskId)
            {
                _dataManager.CompleteTask(taskId);
                RefreshTaskList();
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int taskId)
            {
                _dataManager.DeleteTask(taskId);
                RefreshTaskList();
            }
        }

        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Deselect items once selection changes - this prevents the blue selection highlighting
            if (TaskListView.SelectedItem != null)
            {
                TaskListView.SelectedItem = null;
            }
        }

        #endregion

        #region Quiz Panel Implementation

        private void InitializeQuizPanel()
        {
            // Get quiz questions from data manager
            _quizQuestions = _dataManager.GetQuizQuestions();
            _currentQuestionIndex = 0;
            _quizScore = 0;
            _quizInProgress = false;

            // Set the total questions count
            TotalQuestionsTextBlock.Text = _quizQuestions.Count.ToString();

            // Make sure the start button is visible and question panel is hidden initially
            StartQuizButton.Visibility = Visibility.Visible;
            QuizQuestionPanel.Visibility = Visibility.Collapsed;
            QuizResultsPanel.Visibility = Visibility.Collapsed;
            FeedbackPanel.Visibility = Visibility.Collapsed;
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide start button, show question panel
            StartQuizButton.Visibility = Visibility.Collapsed;
            QuizQuestionPanel.Visibility = Visibility.Visible;
            QuizResultsPanel.Visibility = Visibility.Collapsed;

            // Reset quiz state
            _quizScore = 0;
            _currentQuestionIndex = 0;
            _quizInProgress = true;

            // Log quiz started
            _dataManager.LogQuizStarted();

            // Load first question
            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            // Update question number display
            CurrentQuestionNumberTextBlock.Text = (_currentQuestionIndex + 1).ToString();

            // Hide feedback panel
            FeedbackPanel.Visibility = Visibility.Collapsed;

            // Get current question
            QuizQuestion question = _quizQuestions[_currentQuestionIndex];

            // Set question text
            QuizQuestionTextBlock.Text = question.Question;

            // Clear previous options
            QuizOptionsPanel.Children.Clear();

            // Add options as RadioButtons
            for (int i = 0; i < question.Options.Count; i++)
            {
                RadioButton radioButton = new RadioButton
                {
                    Content = question.Options[i],
                    Tag = i,  // Store the index as the Tag
                    Margin = new Thickness(0, 5, 0, 5),
                    FontSize = 14
                };

                radioButton.Checked += Option_Selected;
                QuizOptionsPanel.Children.Add(radioButton);
            }
        }

        private void Option_Selected(object sender, RoutedEventArgs e)
        {
            if (!_quizInProgress) return;

            RadioButton selectedOption = (RadioButton)sender;
            int selectedIndex = (int)selectedOption.Tag;

            // Disable all options
            foreach (var child in QuizOptionsPanel.Children)
            {
                if (child is RadioButton rb)
                {
                    rb.IsEnabled = false;
                }
            }

            // Check if answer is correct
            QuizQuestion currentQuestion = _quizQuestions[_currentQuestionIndex];
            bool isCorrect = (selectedIndex == currentQuestion.CorrectAnswerIndex);

            // Update score if correct
            if (isCorrect)
            {
                _quizScore++;
            }

            // Show feedback
            FeedbackTextBlock.Text = isCorrect ?
                $"Correct! {currentQuestion.Explanation}" :
                $"Incorrect. The correct answer is: {currentQuestion.Options[currentQuestion.CorrectAnswerIndex]}. {currentQuestion.Explanation}";

            FeedbackPanel.Background = isCorrect ?
                new SolidColorBrush(Color.FromRgb(220, 255, 220)) : // Light green
                new SolidColorBrush(Color.FromRgb(255, 230, 230)); // Light red

            FeedbackPanel.Visibility = Visibility.Visible;
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestionIndex++;

            // Check if quiz is complete
            if (_currentQuestionIndex >= _quizQuestions.Count)
            {
                ShowQuizResults();
                return;
            }

            // Load next question
            LoadCurrentQuestion();
        }

        private void ShowQuizResults()
        {
            // Mark quiz as finished
            _quizInProgress = false;

            // Hide question panel, show results panel
            QuizQuestionPanel.Visibility = Visibility.Collapsed;
            QuizResultsPanel.Visibility = Visibility.Visible;

            // Set score text
            ScoreTextBlock.Text = $"{_quizScore}/{_quizQuestions.Count}";

            // Set feedback based on score percentage
            double scorePercentage = (double)_quizScore / _quizQuestions.Count * 100;

            string feedbackMessage;
            if (scorePercentage >= 90)
            {
                feedbackMessage = "Excellent! You're a cybersecurity expert!";
            }
            else if (scorePercentage >= 70)
            {
                feedbackMessage = "Good job! You have a solid understanding of cybersecurity principles.";
            }
            else if (scorePercentage >= 50)
            {
                feedbackMessage = "Not bad, but there's room for improvement. Review the areas where you made mistakes.";
            }
            else
            {
                feedbackMessage = "You should spend more time learning about cybersecurity basics. Try again after reviewing the material.";
            }

            ScoreFeedbackTextBlock.Text = feedbackMessage;

            // Log quiz completion
            _dataManager.LogQuizCompleted(_quizScore, _quizQuestions.Count);
        }

        private void RestartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset quiz and start over
            StartQuizButton_Click(sender, e);
        }

        #endregion

        #region Activity Log Panel Implementation

        private void InitializeLogPanel()
        {
            // Initialize the activity log
            RefreshActivityLog();
        }

        private void RefreshActivityLog()
        {
            ActivityLogListView.ItemsSource = null;
            ActivityLogListView.ItemsSource = _dataManager.GetRecentActivityLogs();
        }

        #endregion
    }
}
