using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAppUsers.Models;

namespace WebAppUsers.Services
{
    public class UsersDAO
    {
        string connectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=webAppUser;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public UserModel GetOneUser(int id)
        {
            UserModel user = new UserModel();
            string sqlStatement = "Select * from dbo.Users WHERE id=@id";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.BigInt).Value = id;

               
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            user.id = reader.GetInt32(0);
                            user.name = reader.GetString(1);
                            user.password = reader.GetString(2);
                            user.registerTime = reader.GetDateTime(3);
                            user.lastLoginTime = reader.GetDateTime(4);
                            user.status = reader.GetBoolean(5);
                        }

                    }                

                }
            return user;
        }

        public UserModel ReturnRealUser(string userName)
        {
            UserModel user = new UserModel();
            string sqlStatement = "Select * from dbo.Users WHERE name=@name";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar,120).Value = userName;


                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.id = reader.GetInt32(0);
                        user.name = reader.GetString(1);
                        user.password = reader.GetString(2);
                        user.registerTime = reader.GetDateTime(3);
                        user.lastLoginTime = reader.GetDateTime(4);
                        user.status = reader.GetBoolean(5);
                    }

                }

            }
            return user;
        }

        public bool FindUserByNameAndPassword(UserModel user)
        {
            bool success = false;

            string password;

            string sqlStatement = "Select * from dbo.Users WHERE name= @name";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 100).Value = user.name;
                //command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar,100).Value=BCrypt.Net.BCrypt.HashPassword
               
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            password = reader.GetString(2);
                            if (BCrypt.Net.BCrypt.HashPassword(user.password, password)==password.Trim())
                                success = true;
                        }
                       
                           
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }




            return success;
           
        }

        

        public List<UserModel> GetAll()
        {
            List<UserModel> allUser = new List<UserModel>();

            string sqlStatement = "Select * from dbo.Users";

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel();
                        user.id = reader.GetInt32(0);
                        user.name = reader.GetString(1);
                        user.password = reader.GetString(2);
                        user.registerTime = reader.GetDateTime(3);
                        user.lastLoginTime = reader.GetDateTime(4);
                        user.status = reader.GetBoolean(5);

                        allUser.Add(user);
                    }
                }

            }

            return allUser;
        }

        public int CreateOrEdit(UserModel user)
        {

            string sqlStatement = "";
            if (user.id < 1)
            {
                //create
                sqlStatement = "INSERT into dbo.Users Values(@name, @password, @registerDate, @lastLoginDate, @status)";
                user.registerTime = DateTime.Now;
                user.lastLoginTime = DateTime.Now;
                user.status = true;
                user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            }
            else
            {
                //update
                sqlStatement = "UPDATE dbo.Users SET Name=@name, Password=@password, RegisterDate=@registerDate, LastLoginDate=@lastLoginDate, Status=@status WHERE Id=@id";

            }

            

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.BigInt).Value = user.id;
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar, 40).Value = user.name;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 120).Value = user.password;
                command.Parameters.Add("@registerDate", System.Data.SqlDbType.DateTime).Value = user.registerTime;
                command.Parameters.Add("@lastLoginDate", System.Data.SqlDbType.DateTime).Value = user.lastLoginTime;
                command.Parameters.Add("@status", System.Data.SqlDbType.Bit).Value=user.status;

                connection.Open();
                int newId = command.ExecuteNonQuery();
                connection.Close();

                return newId;
            }

        }

        public int Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                string sqlStatement = "DElETE from dbo.Users WHERE Id=@id";
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.BigInt).Value = id;

                connection.Open();
                int deletedId = command.ExecuteNonQuery();
                connection.Close();

                return deletedId;
            }
        }
    }
}