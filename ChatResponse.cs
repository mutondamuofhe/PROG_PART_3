using System.Collections.Generic;

namespace PROG_PART_3.Models
{
    public class ChatResponse
    {
        private static readonly List<string> PositiveResponses = new List<string>
        {
            "That's great to hear!",
            "I'm glad things are going well!",
            "Excellent news!",
            "That sounds positive!",
            "Wonderful! Keep up the good work!"
        };

        private static readonly List<string> NegativeResponses = new List<string>
        {
            "I'm sorry to hear that.",
            "That sounds challenging. Let me help.",
            "Let's see how we can improve the situation.",
            "I understand your concern. Let me assist you.",
            "Don't worry, we'll work through this together."
        };

        private static readonly List<string> NeutralResponses = new List<string>
        {
            "I understand.",
            "I see what you mean.",
            "Let me help you with that.",
            "I'm here to assist you.",
            "Let's explore this further."
        };

        private static readonly List<string> GreetingKeywords = new List<string>
        {
            "hello", "hi", "hey", "greetings", "good morning", "good afternoon", "good evening"
        };

        private static readonly List<string> TaskKeywords = new List<string>
        {
            "task", "reminder", "todo", "to-do", "remember", "remind"
        };

        private static readonly List<string> QuizKeywords = new List<string>
        {
            "quiz", "test", "question", "knowledge", "assessment", "evaluate"
        };

        private static readonly List<string> LogKeywords = new List<string>
        {
            "log", "activity", "history", "record", "what have you done", "show activity"
        };

        private static readonly Dictionary<string, List<string>> CybersecurityKeywords = new Dictionary<string, List<string>>
        {
            { "password", new List<string> {
                "Use strong, unique passwords for each account.",
                "Consider using a password manager to generate and store complex passwords.",
                "A strong password should include upper and lowercase letters, numbers, and special characters."
              }
            },
            { "phishing", new List<string> {
                "Be cautious of emails asking for personal information.",
                "Verify the sender's email address before clicking on any links.",
                "Legitimate organizations will never ask for sensitive information via email."
              }
            },
            { "update", new List<string> {
                "Keep your software and operating systems updated.",
                "Updates often contain security patches for vulnerabilities.",
                "Enable automatic updates when possible."
              }
            },
            { "wifi", new List<string> {
                "Avoid using public Wi-Fi for sensitive transactions.",
                "Consider using a VPN when connecting to public networks.",
                "Ensure your home Wi-Fi is protected with a strong password."
              }
            },
            { "backup", new List<string> {
                "Regularly backup important data.",
                "Follow the 3-2-1 backup rule: 3 copies, 2 different media types, 1 offsite.",
                "Test your backups periodically to ensure they work."
              }
            },
            { "2fa", new List<string> {
                "Enable two-factor authentication whenever available.",
                "2FA adds an extra layer of security beyond just a password.",
                "Use authentication apps rather than SMS when possible for better security."
              }
            },
            { "privacy", new List<string> {
                "Regularly review your privacy settings on social media.",
                "Be mindful of what personal information you share online.",
                "Use privacy-focused browsers and search engines for sensitive searches."
              }
            }
        };

        public static string GetResponse(string input, string sentiment = "neutral")
        {
            string lowercaseInput = input.ToLower();

            // Check for greeting
            foreach (string keyword in GreetingKeywords)
            {
                if (lowercaseInput.Contains(keyword))
                {
                    return "Hello! How can I help you with cybersecurity today?";
                }
            }

            // Check for task-related input
            foreach (string keyword in TaskKeywords)
            {
                if (lowercaseInput.Contains(keyword))
                {
                    return "Would you like to manage your cybersecurity tasks? You can add new tasks or set reminders.";
                }
            }

            // Check for quiz-related input
            foreach (string keyword in QuizKeywords)
            {
                if (lowercaseInput.Contains(keyword))
                {
                    return "Ready to test your cybersecurity knowledge with our quiz?";
                }
            }

            // Check for log-related input
            foreach (string keyword in LogKeywords)
            {
                if (lowercaseInput.Contains(keyword))
                {
                    return "I can show you your recent activity log. Would you like to see it?";
                }
            }

            // Check for cybersecurity keywords
            foreach (var entry in CybersecurityKeywords)
            {
                if (lowercaseInput.Contains(entry.Key))
                {
                    Random random = new Random();
                    int index = random.Next(entry.Value.Count);
                    return entry.Value[index];
                }
            }

            // Fall back to sentiment-based responses if no keywords match
            List<string> responses;
            switch (sentiment.ToLower())
            {
                case "positive":
                    responses = PositiveResponses;
                    break;
                case "negative":
                    responses = NegativeResponses;
                    break;
                default:
                    responses = NeutralResponses;
                    break;
            }

            Random rnd = new Random();
            return responses[rnd.Next(responses.Count)];
        }

        public static bool ContainsTaskCommand(string input)
        {
            string lowercaseInput = input.ToLower();
            return lowercaseInput.Contains("add task") ||
                   lowercaseInput.Contains("create task") ||
                   lowercaseInput.Contains("new task") ||
                   lowercaseInput.Contains("set reminder") ||
                   lowercaseInput.Contains("remind me");
        }

        public static bool ContainsShowLogCommand(string input)
        {
            string lowercaseInput = input.ToLower();
            return lowercaseInput.Contains("show log") ||
                   lowercaseInput.Contains("activity log") ||
                   lowercaseInput.Contains("show activity") ||
                   lowercaseInput.Contains("what have you done");
        }
    }
}
