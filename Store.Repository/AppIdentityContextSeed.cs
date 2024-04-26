using Microsoft.AspNetCore.Identity;
using Store.Data.IdentityEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeeduserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmed Gaber",
                    Email = "mohamedmidoo103@gmail.com",
                    UserName="ahmedgaber",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "Gaber",
                        City = "Helwan",
                        State = "Cairo ",
                     
                        Street="19",
                        ZipCode= "12345"


                    }
                };
                await userManager.CreateAsync(user,"Passward123!");
            }
        }
    }
}
