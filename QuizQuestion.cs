using System.Collections.Generic;

namespace PROG_PART_3.Models
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
        public QuestionType Type { get; set; }

        public enum QuestionType
        {
            MultipleChoice,
            TrueFalse
        }

        public QuizQuestion(string question, List<string> options, int correctAnswerIndex, string explanation, QuestionType type = QuestionType.MultipleChoice)
        {
            Question = question;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            Explanation = explanation;
            Type = type;
        }

        // Constructor specifically for true/false questions
        public QuizQuestion(string question, bool correctAnswer, string explanation)
        {
            Question = question;
            Options = new List<string> { "True", "False" };
            CorrectAnswerIndex = correctAnswer ? 0 : 1;
            Explanation = explanation;
            Type = QuestionType.TrueFalse;
        }
    }
}