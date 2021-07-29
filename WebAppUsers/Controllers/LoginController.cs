using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppUsers.Models;
using WebAppUsers.Services;

namespace WebAppUsers.Controllers
{
    public class LoginController : Controller
    {
        SecurityService securityService = new SecurityService();
        public static UserModel realUser = new UserModel();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginProcess(UserModel user)
        {
            if (securityService.IsUserValid(user))
            {
                List<UserModel> users = new List<UserModel>();
                UsersDAO usersDAO = new UsersDAO();
                users = usersDAO.GetAll();
                realUser=usersDAO.ReturnRealUser(user.name);
                if (realUser.status)
                {
                    realUser.lastLoginTime = DateTime.UtcNow;
                    usersDAO.CreateOrEdit(realUser);
                    return View("LoginSuccess", users);
                }

                else
                    return View("Index");
            }

            else
                return View("LoginFailed", user);

        }

        public IActionResult RegistrationProcess()
        {
            return View();
        }

        
        public IActionResult Registration(UserModel user)
        {
            
            UsersDAO usersDAO = new UsersDAO();
            List<UserModel> users = new List<UserModel>();
            users = usersDAO.GetAll();
            if (users.Any(x=>x.name.Trim()==user.name))
            {
                return View("RegistrationFailed", user);
            }
               
            else 
            {
                int id = usersDAO.CreateOrEdit(user);
                realUser = usersDAO.ReturnRealUser(user.name);
                users = usersDAO.GetAll();
                return View("LoginSuccess", users);
            }
            
        }
       

       
        public IActionResult Edit(int[] SelectedUsers)        
        {
            UsersDAO usersDAO = new UsersDAO();

            foreach (int id in SelectedUsers)
            {                
                int returnId = usersDAO.CreateOrEdit(usersDAO.GetOneUser(id).ChangeStatus());
            }
            List<UserModel> users = new List<UserModel>();
            users = usersDAO.GetAll();
            if (SelectedUsers.Contains(realUser.id))
                return View("Index");
            else
                return View("LoginSuccess", users);
        }

        public IActionResult Delete(int[] SelectedUsers)
        {
            UsersDAO usersDAO = new UsersDAO();

            foreach (int id in SelectedUsers)
            {
                int returnId = usersDAO.Delete(id);
            }
            List<UserModel> users = new List<UserModel>();

            users = usersDAO.GetAll();

            if (SelectedUsers.Contains(realUser.id))
                return View("Index");
            else
                return View("LoginSuccess", users);
        }

    }
}
