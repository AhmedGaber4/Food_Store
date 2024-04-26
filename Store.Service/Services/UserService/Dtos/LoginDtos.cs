using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.UserService.Dtos
{
    public class LoginDtos
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
       public string Passward { get; set; }
    }
}
