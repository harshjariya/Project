using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizManagementSystem.Models;

namespace QuizManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration _configuration)
        {
            _logger = logger;
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command1 = connection.CreateCommand();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = "PR_MST_Quiz_SelectAll";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable quiztable = new DataTable();
            quiztable.Load(reader1);

            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_Question_SelectAll";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable questiontable = new DataTable();
            questiontable.Load(reader2);

            SqlCommand command3 = connection.CreateCommand();
            command3.CommandType = CommandType.StoredProcedure;
            command3.CommandText = "PR_MST_QuestionLevel_SelectAll";
            SqlDataReader reader3 = command3.ExecuteReader();
            DataTable questionleveltable = new DataTable();
            questionleveltable.Load(reader3);

            connection.Close();
            return View((quiztable, questiontable, questionleveltable));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
