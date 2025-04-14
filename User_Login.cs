using System.Data;
using System.Data.SqlClient;
using Visitors_Management.IRepository;
using Visitors_Management.Models;

namespace Visitors_Management.Repository
{
    public class User_Login : IUser_Login
    {
        private readonly string _connectionString;
        public User_Login(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DBConnection");
        }
        public List<VM_LoginModel> Insert_Token(VM_LoginModel Request)
        {
            List<VM_LoginModel> Fill_List = new List<VM_LoginModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "update user_master set token  = '" + Request.TOKEN + "' where  user_name  = '" + Request.USERNAME + "'";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 0;
                while (dr.Read())
                {
                    VM_LoginModel Data = new VM_LoginModel
                    {
                        USERNAME = dr[0].ToString(),
                        TOKEN = dr[1].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;

        }
        public List<VM_LoginModel> Select_User(VM_LoginModel Request)
        {
            List<VM_LoginModel> Fill_List = new List<VM_LoginModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";
                Sql = "SELECT  user_name,user_id,  user_password,emp_name,role_name FROM user_master where user_name = '" + Request.USERNAME + "'";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_LoginModel Data = new VM_LoginModel
                    {
                        USERNAME = dr[0].ToString(),
                        USER_ID = dr[0].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public List<VM_LoginModel> Select_Token(VM_LoginModel Request)
        {
            List<VM_LoginModel> Fill_List = new List<VM_LoginModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";
                Sql = "SELECT token FROM user_master where user_name = '" + Request.USERNAME + "'";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_LoginModel Data = new VM_LoginModel
                    {
                        TOKEN = dr[0].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public VM_LoginModel Select_User_Attributes(VM_LoginModel? Request)
        {
            VM_LoginModel Fill_List = new VM_LoginModel();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";

                Request.TOKEN = Request.TOKEN.Substring(9, Request.TOKEN.Length - 9);

                Request.TOKEN = Request.TOKEN.Substring(1, Request.TOKEN.Length - 3);

                Sql = "SELECT user_id,user_name,role_name,emp_name FROM user_master where TOKEN = '" + Request.TOKEN + "'";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_LoginModel Data = new VM_LoginModel
                    {
                        USER_ID = dr[0].ToString(),
                        USERNAME = dr[1].ToString(),
                        role_name = dr[2].ToString(),
                        emp_name = dr[3].ToString(),
                    };
                    i++;
                    Fill_List = Data;
                }
            }
            return Fill_List;
        }
    }
}