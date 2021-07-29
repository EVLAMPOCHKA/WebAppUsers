using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppUsers.Models;

namespace WebAppUsers.Services
{
    public class SecurityService
    {   
        UsersDAO userDao = new UsersDAO();

        public bool IsUserValid(UserModel user)
        {
            return userDao.FindUserByNameAndPassword(user);
           // return knowUsers.Any(x => x.name == user.name && x.password == user.password);
        }

        
    }
}
