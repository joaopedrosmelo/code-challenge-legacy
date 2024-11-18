using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static LegacyRefactoring.Controllers.UserController;

namespace LegacyRefactoring.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;


        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckUser(string username)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                using SqlConnection connection = new SqlConnection(connectionString);
                var command = new SqlCommand("SELECT * FROM Users WHERE Username = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var name = reader["Name"].ToString();
                        ViewData["Message"] = $"Hello, {name}";
                    }
                    else
                    {
                        ViewData["Message"] = "User not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "An error occurred while processing your request.";
                ViewData["Error"] = ex.Message;
            }

            return View("Index");
        }
        public class User
        {
            public string Username { get; set; }
            public string Name { get; set; }
        }
    }
}