using System.ComponentModel.DataAnnotations;

namespace ThemeConversion.Models
{
    public class Register
    {
        [Required(ErrorMessage ="Please enter Username")]
        public string Name { get; set; }


        [EmailAddress(ErrorMessage = "Please enter valid Email")]
        public string Email { get; set; }


        [MaxLength(10,ErrorMessage = "Enter valid Number")]
        public string Mobile { get; set; }



        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }



        
    }
}
