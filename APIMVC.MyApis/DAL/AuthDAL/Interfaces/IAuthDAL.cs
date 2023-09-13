using APIMVC.MyApis.DAL.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.MyApis.DAL.AuthDAL.Interfaces
{
    public interface IAuthDAL
    {

        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);

    }
}
