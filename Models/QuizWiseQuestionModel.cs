using System.ComponentModel.DataAnnotations;

namespace QuizManagementSystem.Models
{
    public class QuizWiseQuestionModel
    {
        [Required]

        public int QuizID { get; set; }

        [Required]
        public int QuestionID { get; set; }

        [Required]

        public int UserID { get; set; }
        public string? QuizName { get; set; }

        public string? QuestionText { get; set; }
        public string? UserName { get; set; }
        public DateTime Created { get; set; }

        public int QuizWiseQuestionID { get; set; }
    }
}
