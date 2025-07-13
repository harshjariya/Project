using System.ComponentModel.DataAnnotations;

namespace QuizManagementSystem.Models
{
    public class QuestionModel
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public string Question_Level { get; set; }

        [Required]
        public string OpationA { get; set; }

        [Required]
        public string OpationB { get; set; }

        [Required]
        public string OpationC { get; set; }

        [Required]
        public string OpationD { get; set; }

        [Required]
        public string Correct_Opation { get; set; }

        [Required]
        public string Mark { get; set; }
        [Required]
        public bool isActive { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int QuestionID { get; set; }
        [Required]
        public int QuestionLevelID { get; set; }
        
    }
    public class QuestionDropdownModel
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
    }
}
