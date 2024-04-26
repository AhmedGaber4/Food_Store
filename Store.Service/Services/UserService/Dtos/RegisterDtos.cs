using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService.Dtos
{
    public class RegisterDtos
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+=-]).{7,16}$",
            ErrorMessage = "Password is not valid. It must contain at least one lowercase letter, one uppercase letter, one digit, one special character, and be between 7 and 16 characters long.")]
        public string Passward { get; set; }

    }
}
