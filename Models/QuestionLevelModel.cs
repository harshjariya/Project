namespace QuizManagementSystem.Models
{
    public class QuestionLevelModel
    {
       public int UserID { get; set; }

        public int QuestionLevelID { get; set; }
        public string Question_Level { get; set; }
    }
    public class QuestionLevelDropDownModel
    {
        public int QuestionLevelID { get; set; }
        public string Question_Level { get; set; }
    }
}