using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PROG_PART_3.Models
{
    public class DataManager
    {
        private static DataManager _instance;
        private List<Task> _tasks;
        private List<ActivityLog> _activityLogs;
        private List<QuizQuestion> _quizQuestions;

        private DataManager()
        {
            _tasks = new List<Task>();
            _activityLogs = new List<ActivityLog>();
            InitializeQuizQuestions();
        }

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataManager();
                }
                return _instance;
            }
        }

        #region Task Management

        public List<Task> GetAllTasks()
        {
            return _tasks.ToList();
        }

        public Task AddTask(string title, string description, DateTime? reminderDate = null)
        {
            var task = new Task
            {
                Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1,
                Title = title,
                Description = description,
                CreatedDate = DateTime.Now,
                ReminderDate = reminderDate,
                IsCompleted = false
            };

            _tasks.Add(task);
            AddActivityLog(ActivityLog.ActionTypes.TaskAdded, $"Added task: {title}");

            if (reminderDate.HasValue)
            {
                AddActivityLog(ActivityLog.ActionTypes.ReminderSet, $"Set reminder for task '{title}' on {reminderDate.Value.ToShortDateString()}");
            }

            return task;
        }

        public void CompleteTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                AddActivityLog(ActivityLog.ActionTypes.TaskCompleted, $"Completed task: {task.Title}");
            }
        }

        public void DeleteTask(int taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                AddActivityLog(ActivityLog.ActionTypes.TaskRemoved, $"Removed task: {task.Title}");
            }
        }

        public List<Task> GetDueReminders()
        {
            return _tasks.Where(t => !t.IsCompleted && t.ReminderDate.HasValue && t.ReminderDate.Value.Date <= DateTime.Today).ToList();
        }

        #endregion

        #region Activity Log Management

        public void AddActivityLog(string actionType, string description)
        {
            var log = new ActivityLog(actionType, description);
            _activityLogs.Add(log);
        }

        public List<ActivityLog> GetRecentActivityLogs(int count = 10)
        {
            return _activityLogs.OrderByDescending(l => l.Timestamp).Take(count).ToList();
        }

        #endregion

        #region Quiz Management

        private void InitializeQuizQuestions()
        {
            _quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    2,
                    "Reporting phishing emails helps prevent scams and protects you and others."
                ),
                new QuizQuestion(
                    "Which of the following is the strongest password?",
                    new List<string> { "password123", "P@ssw0rd!", "iL0veMy$ecureP@ss2023", "qwerty" },
                    2,
                    "A strong password should be long and include a mix of uppercase, lowercase, numbers, and special characters."
                ),
                new QuizQuestion(
                    "What is two-factor authentication (2FA)?",
                    new List<string> {
                        "Using two different passwords",
                        "A method that requires two different forms of identification to access an account",
                        "Logging in from two different devices",
                        "Setting up two email accounts"
                    },
                    1,
                    "2FA adds an extra layer of security by requiring something you know (password) and something you have (like your phone)."
                ),
                new QuizQuestion(
                    "Public Wi-Fi networks are generally secure for banking and shopping.",
                    false,
                    "Public Wi-Fi networks are often unsecured, making it easy for attackers to intercept your data."
                ),
                new QuizQuestion(
                    "What is social engineering in cybersecurity?",
                    new List<string> {
                        "Using social media to improve security",
                        "Manipulating people into divulging confidential information",
                        "Creating secure social network accounts",
                        "Developing social networks with enhanced security"
                    },
                    1,
                    "Social engineering attacks use psychological manipulation to trick users into making security mistakes."
                ),
                new QuizQuestion(
                    "You should use the same password across multiple accounts for easier management.",
                    false,
                    "Using unique passwords for each account prevents hackers from accessing all your accounts if one password is compromised."
                ),
                new QuizQuestion(
                    "What is the primary purpose of antivirus software?",
                    new List<string> {
                        "Speed up your computer",
                        "Organize files",
                        "Detect and remove malware",
                        "Improve internet connection"
                    },
                    2,
                    "Antivirus software helps protect your system by detecting, preventing, and removing malicious software."
                ),
                new QuizQuestion(
                    "What should you check before clicking on a link in an email?",
                    new List<string> {
                        "The font of the email",
                        "The actual URL by hovering over the link",
                        "The time the email was sent",
                        "The number of recipients"
                    },
                    1,
                    "Always hover over links to preview the actual URL before clicking, to avoid phishing attempts."
                ),
                new QuizQuestion(
                    "What is a software update primarily used for?",
                    new List<string> {
                        "To add new features only",
                        "To fix security vulnerabilities and bugs",
                        "To make your computer run slower",
                        "To collect user data"
                    },
                    1,
                    "Software updates often contain important security patches to protect against new threats."
                ),
                new QuizQuestion(
                    "Backing up your data is an important cybersecurity practice.",
                    true,
                    "Regular backups help you recover your data in case of ransomware attacks or hardware failures."
                )
            };
        }

        public List<QuizQuestion> GetQuizQuestions()
        {
            return _quizQuestions.ToList();
        }

        public void LogQuizStarted()
        {
            AddActivityLog(ActivityLog.ActionTypes.QuizStarted, "Started the cybersecurity quiz");
        }

        public void LogQuizCompleted(int score, int total)
        {
            AddActivityLog(ActivityLog.ActionTypes.QuizCompleted, $"Completed quiz with score {score}/{total}");
        }

        #endregion
    }
}