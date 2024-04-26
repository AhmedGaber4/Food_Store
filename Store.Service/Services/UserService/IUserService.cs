using Microsoft.Win32;

using Store.Service.Services.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService
{
    public interface IUserService
    {
        Task<UserDtos> Register(RegisterDtos input) ;
     

        Task<UserDtos>Login(LoginDtos input);
    }
}
