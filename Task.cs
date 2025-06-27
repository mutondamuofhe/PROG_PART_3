using System;

namespace PROG_PART_3.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public Task()
        {
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }

        public Task(string title, string description, DateTime? reminderDate = null)
        {
            Title = title;
            Description = description;
            CreatedDate = DateTime.Now;
            ReminderDate = reminderDate;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "Completed" : "Pending";
            string reminder = ReminderDate.HasValue ? $", Reminder: {ReminderDate.Value.ToShortDateString()}" : "";
            return $"{Title} ({status}{reminder})";
        }
    }
}