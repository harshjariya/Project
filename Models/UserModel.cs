using System.ComponentModel.DataAnnotations;

namespace QuizManagementSystem.Models
{
    public class UserModel
    {

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        public string Name { get; set; }


        [EmailAddress(ErrorMessage = "Please enter valid Email")]
        public string Email { get; set; }


        [MaxLength(10, ErrorMessage = "Enter valid Number")]
        public string Mobile { get; set; }



        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }
        [Required]
        public bool isActive { get; set; }
        [Required]
        public bool isAdmin { get; set; }
        public int QuestionLevelID { get; internal set; }
    }
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username / Email / MobileNo is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
    public class UserRegisterModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile No. is Required")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        //[Required]
        //public bool isActive { get; set; }
        //[Required]
        //public bool isAdmin { get; set; }
    }   

}
