using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppUsers.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public DateTime registerTime { get; set; }
        public DateTime lastLoginTime { get; set; }
        public bool status { get; set; }

        public UserModel() { id = -1; name = ""; password = ""; status = true; }

        public UserModel ChangeStatus()
        {
            if (this.status == true)
            { 
                this.status = false; 
            }
            else 
            { 
                this.status = true; 
            }

            return this;
               
        }
    }
}
