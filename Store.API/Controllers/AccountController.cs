using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.IdentityEntities;
using Store.Service.HandleResponses;
using Store.Service.Services.UserService;
using Store.Service.Services.UserService.Dtos;
using System.Security.Claims;

namespace Store.API.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IUserService _user;
        private readonly UserManager<AppUser> _manager;

        public AccountController(IUserService user, UserManager<AppUser> manager)
        {
            _user = user;
            _manager = manager;
        }
        [HttpPost]
        public async Task<ActionResult<UserDtos>> Login(LoginDtos input)
        {
            var user = await _user.Login(input);
            if (user is null)
                return Unauthorized(new CustomException(401));
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDtos>> Register(RegisterDtos register)
        {
            var user = await _user.Register(register);
            if (user is null)
                return BadRequest(new CustomException(400, "Email already exits"));
            return Ok(user);
        }
        [HttpGet]
        [Authorize]

        public async Task<ActionResult<UserDtos>> GetCurrentUserDetails()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var user = await _manager.FindByEmailAsync(email);
            return new UserDtos

            {
                Email = user.Email,
                DisplayName = user.DisplayName
             
            };
        }
    }
}
