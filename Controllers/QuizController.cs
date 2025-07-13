using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using QuizManagementSystem.Models;

namespace QuizManagementSystem.Controllers
{
    public class QuizController : Controller
    {
        private IConfiguration configuration;
        private object _configuration;

        public QuizController(IConfiguration _configuration)
        {
            configuration = _configuration;
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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddQuiz(int? QuizID)
        {

            UserDropDown();
            
            if (QuizID == null)
            {
                var m = new QuizModel
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
            command.CommandText = "PR_MST_Quiz_SelectByPK";
            //command.Parameters.AddWithValue("@CountryID", countryID);
            command.Parameters.AddWithValue("@QuizID", QuizID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuizModel model = new QuizModel();

            foreach (DataRow dataRow in table.Rows)
            {
                model.QuizID = Convert.ToInt32(QuizID);
                model.QuizName = dataRow["quizName"].ToString();
                model.TotalQuestions = (int)dataRow["totalQuestions"];
                model.QuizDate = (DateTime)dataRow["quizDate"];
                model.UserID = (int)dataRow["userID"];
            }
            
            return View(model);
        }
       
        public IActionResult QuizList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[PR_MST_Quiz_SelectAll]";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuizDelete(int QuizID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_Quiz_Delete";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Quiz deleted successfully.";
                return RedirectToAction("QuizList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Quiz: " + ex.Message;
                return RedirectToAction("QuizList");
            }
        }

        public IActionResult QuizAddEdit(Models.QuizModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizID == 0)
                {
                    command.CommandText = "[PR_MST_Quiz_Insert]";
                }
                else
                {
                    command.CommandText = "[PR_MST_Quiz_Update]";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                }
                command.Parameters.AddWithValue("@quizName", model.QuizName);
                command.Parameters.AddWithValue("@totalQuestions", model.TotalQuestions);
                command.Parameters.AddWithValue("@quizDate", model.QuizDate);
                command.Parameters.AddWithValue("@userID", model.UserID);

                command.ExecuteNonQuery();
                return RedirectToAction("QuizList");
            }

            UserDropDown();

            return View("QuizForm", model);
        }

       
     //Filter
     [HttpPost]
        public IActionResult QuizFilter(IFormCollection fc)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            Console.WriteLine("kkk" + fc["FromQuizDate"]);
            command.CommandText = "PR_QuizFilter_SelectAll";
            if (fc["QuizName"].ToString() == "")
            {
                command.Parameters.AddWithValue("@QuizName", fc["QuizName"].ToString());
            }
            else
            {
                command.Parameters.AddWithValue("@QuizName", fc["QuizName"].ToString());
            }

            if (fc["MinQuestion"].ToString() == "")
            {
                command.Parameters.AddWithValue("@MinQuestion", 0);
            }
            else
            {
                command.Parameters.AddWithValue("@MinQuestion", Convert.ToInt32(fc["MinQuestion"]));
            }

            if (fc["MaxQuestion"].ToString() == "")
            {
                command.Parameters.AddWithValue("@MaxQuestion", 100);
            }
            else
            {
                command.Parameters.AddWithValue("@MaxQuestion", Convert.ToInt32(fc["MaxQuestion"]));

            }

            if (fc["FromQuizDate"].ToString() == "")
            {
                command.Parameters.AddWithValue("@FromQuizDate", new DateTime(1753, 1, 1));
            }
            else
            {

                command.Parameters.AddWithValue("@FromQuizDate", Convert.ToDateTime(fc["FromQuizDate"]));

            }

            if (fc["ToQuizDate"].ToString() == "")
            {
                command.Parameters.AddWithValue("@ToQuizDate", new DateTime(9999, 12, 31));
            }
            else
            {

                command.Parameters.AddWithValue("@ToQuizDate", Convert.ToDateTime(fc["ToQuizDate"]));
            }



            using (SqlDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);

                // Pass the data to the view instead of RedirectToAction
                return View("QuizList", table);
            }
        }


    }
}
