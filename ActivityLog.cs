using System;
using System.Collections.Generic;

namespace PROG_PART_3.Models
{
    public class ActivityLog
    {
        public DateTime Timestamp { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }

        public ActivityLog(string actionType, string description)
        {
            Timestamp = DateTime.Now;
            ActionType = actionType;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{Timestamp.ToString("g")}] {ActionType}: {Description}";
        }

        // Enum of possible action types
        public static class ActionTypes
        {
            public const string TaskAdded = "Task Added";
            public const string TaskCompleted = "Task Completed";
            public const string TaskRemoved = "Task Removed";
            public const string ReminderSet = "Reminder Set";
            public const string QuizStarted = "Quiz Started";
            public const string QuizCompleted = "Quiz Completed";
            public const string ChatInteraction = "Chat Interaction";
        }
    }
}