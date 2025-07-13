using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizManagementSystem.Models;
//using QuizManagementSystem.Models.QuestionDropdownModel;

namespace QuizManagementSystem.Controllers
{
    public class QuizWiseQuestionController : Controller
    {
        private IConfiguration configuration;

        public int QuizWiseQuestionID { get; private set; }
        public object QuizWiseQuestionLevelID { get; private set; }
        public dynamic QuizList { get; private set; }

        public QuizWiseQuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public void QuizDropDown()
        //{
        //    string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command2 = connection.CreateCommand();
        //    command2.CommandType = System.Data.CommandType.StoredProcedure;
        //    command2.CommandText = "[PR_MST_QuizWiseQuestions_Fill_Quiz_Dropdown]";
        //    //command2.Parameters.Add("@quizID", SqlDbType.Int).Value = CommonVariable.QuizID();
        //    SqlDataReader reader2 = command2.ExecuteReader();
        //    DataTable dataTable2 = new DataTable();
        //    dataTable2.Load(reader2);
        //    List<QuizDropDownModel> countryList = new List<QuizDropDownModel>();
        //    foreach (DataRow data in dataTable2.Rows)
        //    {
        //        QuizDropDownModel model = new QuizDropDownModel();
        //        model.QuizID = Convert.ToInt32(data["QuizID"]);
        //        model.QuizName = data["QuizName"].ToString();
        //        QuizList.Add(model);
        //    }
        //    ViewBag.QuizList = QuizList;
        //}
        public IActionResult AddQuizWiseQuestion(int? QuizWiseQuestionsID)
        {
            //QuizDropDown();
           //QuestionDropDown();
            UserDropDown();
            if (QuizWiseQuestionsID == null)
            {
                var m = new QuizWiseQuestionModel
                {
                    //CreationDate = DateTime.Now
                };
                return View(m);
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[PR_MST_QuizWiseQuestions_SelectByPK]";
            //command.Parameters.AddWithValue("@CountryID", countryID);
            command.Parameters.AddWithValue("@quizWiseQuestionsID", QuizWiseQuestionsID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuizWiseQuestionModel model = new QuizWiseQuestionModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.QuizWiseQuestionID = Convert.ToInt32(QuizWiseQuestionsID);
                model.QuizID = (int)dataRow["quizID"];
                model.QuestionID = (int)dataRow["questionID"];
                model.UserID = (int)dataRow["userID"];
            }
            return View(model);

            
        }
        public IActionResult QuizWiseQuestionList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuizWiseQuestions_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuizWiseQuestionDelete(int QuizWiseQuestionsID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_QuizWiseQuestions_Delete";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuizWiseQuestion deleted successfully.";
                return RedirectToAction("QuizWiseQuestionList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the QuizWiseQuestion: " + ex.Message;
                return RedirectToAction("QuizWiseQuestionList");
            }
        }
        public IActionResult QuizWiseQuestionDetails()
        {
            return View();
        }

        public IActionResult QuizWiseQuestionAddEdit(QuizWiseQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (QuizWiseQuestionID == 0)
                {
                    command.CommandText = "PR_MST_QuizWiseQuestions_Insert";
                }
                else
                {
                    command.CommandText = "PR_MST_QuizWiseQuestions_Update";
                    command.Parameters.Add("@QuizWiseQuestionID", SqlDbType.Int).Value = model.QuizWiseQuestionID;
                }
                command.Parameters.AddWithValue("@quizID", model.QuizID);
                command.Parameters.AddWithValue("@questionID", model.QuestionID);
                command.Parameters.AddWithValue("@userId", model.UserID);


                command.ExecuteNonQuery();
                return RedirectToAction("QuizWiseQuestionList");
            }
            //QuizDropDown();
            //QuestionDropDown();
            UserDropDown();
            return View("AddQuizWiseQuestion");
        }
        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_Quiz_Dropdown";

            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<UserDropDownModel> UserList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable2.Rows)
            {
                UserDropDownModel model = new UserDropDownModel();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName = data["UserName"].ToString();
                UserList.Add(model);
            }
            ViewBag.UserList = UserList;
        }
    }
}
