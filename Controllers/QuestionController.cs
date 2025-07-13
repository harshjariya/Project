using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuizManagementSystem.Models;

namespace QuizManagementSystem.Controllers
{
    public class QuestionController : Controller
    {
        private IConfiguration configuration;

        public QuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddQuestion(int? QuestionID)
        {

            QuestionLevelDropDown();
            UserDropDown();
            if (QuestionID == null)
            {
                var m = new QuestionModel
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
            command.CommandText = "PR_MST_Question_SelectByPK";
            //command.Parameters.AddWithValue("@CountryID", countryID);
            command.Parameters.AddWithValue("@questionID", QuestionID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuestionModel model = new QuestionModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.QuestionID = Convert.ToInt32(QuestionID);
                model.Question = dataRow["questionText"].ToString();
                model.Question_Level = dataRow["questionlevelID"].ToString();
                model.OpationA = dataRow["optionA"].ToString();
                model.OpationB = dataRow["optionB"].ToString();
                model.OpationC = dataRow["optionC"].ToString();
                model.OpationD = dataRow["optionD"].ToString();
                model.Correct_Opation = dataRow["correctOption"].ToString();
                model.Mark = dataRow["questionMarks"].ToString();
                model.isActive = (bool)dataRow["isActive"];
                model.UserID = (int)dataRow["userID"];
            }

            return View(model);
           
        }
        public void QuestionLevelDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_QuestionLevel_FillDropdown";

            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<QuestionLevelDropDownModel> QuestionLevelList = new List<QuestionLevelDropDownModel>();
            foreach (DataRow data in dataTable2.Rows)
            {
                QuestionLevelDropDownModel model = new QuestionLevelDropDownModel();
                model.QuestionLevelID = Convert.ToInt32(data["QuestionLevelID"]);
                model.Question_Level = data["QuestionLevel"].ToString();
                QuestionLevelList.Add(model);
            }
            ViewBag.QuestionLevelList = QuestionLevelList;
        }
        public IActionResult QuestionList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[PR_MST_Question_SelectAll]";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_Question_Delete";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Question deleted successfully.";
                return RedirectToAction("QuestionList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Question: " + ex.Message;
                return RedirectToAction("QuestionList");
            }
        }
        public IActionResult QuestionAddEdit(QuestionModel model)
        {
            QuestionLevelDropDown();
            UserDropDown();
            if (ModelState.IsValid)
            {
                string connectionString =this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuestionID == 0)
                {
                    command.CommandText = "PR_MST_Question_Insert";
                }
                else
                {
                    command.CommandText = "[PR_MST_Question_Update]";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                }
                command.Parameters.AddWithValue("@questionText", model.Question);
                command.Parameters.AddWithValue("@questionlevelID", model.Question_Level);
                command.Parameters.AddWithValue("@optionA", model.OpationA);
                command.Parameters.AddWithValue("@optionB", model.OpationB);
                command.Parameters.AddWithValue("@optionC", model.OpationC);
                command.Parameters.AddWithValue("@optionD", model.OpationD);
                command.Parameters.AddWithValue("@correctOption", model.Correct_Opation);
                command.Parameters.AddWithValue("@questionMarks", model.Mark);
                command.Parameters.AddWithValue("@isActive", model.isActive);
                command.Parameters.AddWithValue("@userID", model.UserID);


                command.ExecuteNonQuery();
                return RedirectToAction("QuestionList");
            }

            return View("AddQuestion");
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
