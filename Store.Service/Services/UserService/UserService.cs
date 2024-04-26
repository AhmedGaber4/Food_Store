using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Store.Data.IdentityEntities;
using Store.Service.Services.TokenService;

using Store.Service.Services.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _user;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(
            UserManager<AppUser> user, SignInManager<AppUser> signInManager,
ITokenService tokenService)
        {
            _user = user;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public  async Task<UserDtos> Login(LoginDtos input)
        {
            var user = await _user.FindByEmailAsync(input.Email);
            if (user is null)
               return null;
            var result= await _signInManager.CheckPasswordSignInAsync(user, input.Passward, false);

            if (!result.Succeeded)
                throw new Exception("Login Faild");
            return new UserDtos

            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.GenerateToken(user)
            };
        }

        public async Task<UserDtos> Register(RegisterDtos input)
        {
            var user = await _user.FindByEmailAsync(input.Email);
            if (user is not null)
                return null;
            var appUser = new AppUser

            {
                DisplayName = input.DisplayName,

                Email = input.Email,

                UserName = input.DisplayName
            };
            var result = await _user.CreateAsync(appUser, input.Passward);
            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(x=> x.Description).FirstOrDefault());
            return new UserDtos

            {
                Email = input.Email,
                DisplayName = input.DisplayName,
                Token = _tokenService.GenerateToken(appUser)
            };
        }
    }
}
