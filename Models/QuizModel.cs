using System.ComponentModel.DataAnnotations;

namespace QuizManagementSystem.Models
{
    public class QuizModel
    {
        public int QuizID { get; set; }

        [Required]
        public string QuizName { get; set; }
        [Required]
        public int TotalQuestions { get; set; }
        [Required]
        public DateTime QuizDate { get; set; }
        public int UserID { get;  set; }
        
    }
    public class QuizDropDownModel
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
    }
}
