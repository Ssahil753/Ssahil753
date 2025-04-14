using System.Data;
using System.Data.SqlClient;
using Visitors_Management.IRepository;
using Visitors_Management.Models;

namespace Visitors_Management.Repository
{
    public class User_Master : IUser_Master
    {
        private readonly string _connectionString;
        public User_Master(IConfiguration? connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DBConnection");
        }
        public List<VM_User_Master> Insert_User(VM_User_Master Request)
        {
            List<VM_User_Master> Fill_List = new List<VM_User_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                Int32? mobile_no = 0;
                if (Request.mobile_no.Trim() == "")
                {
                    mobile_no = 0;
                }
                String Sql = "insert into user_master(user_name, user_password, emp_name,mobile_no, email_id,role_name,user_type,Active_Status,designation) values('" + Request.user_name.Trim() + "', '" + Request.user_password.Trim() + "', '" + Request.emp_name.Trim() + "','" + mobile_no + "','" + Request.email_id.Trim() + "','" + Request.role_name.Trim() + "', '" + Request.user_type.Trim() + "',  '" + Request.active_status.Trim() + "', '" + Request.designation.Trim() + "')";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 0;
                while (dr.Read())
                {
                    VM_User_Master Data = new VM_User_Master
                    {
                        user_id = dr["user_id"].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public List<VM_User_Master> Select_User(VM_User_Master objModel)
        {
            List<VM_User_Master> Fill_List = new List<VM_User_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";
                if (objModel.user_id == "0")
                {
                    Sql = "SELECT user_id, user_name, user_password, emp_name,mobile_no, email_id,role_name,active_status,designation FROM user_master where role_name <> 'Super Admin' order by user_name";
                }
                else
                {
                    Sql = "SELECT user_id, user_name, user_password, emp_name,mobile_no, email_id,role_name,active_status,designation FROM user_master where role_name <> 'Super Admin'   and user_id = " + objModel.user_id + " order by user_name";

                }
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_User_Master Data = new VM_User_Master
                    {
                        sr_no = i.ToString(),
                        user_id = dr[0].ToString(),
                        user_name = dr[1].ToString(),
                        user_password = dr[2].ToString(),
                        emp_name = dr[3].ToString(),
                        mobile_no = dr[4].ToString(),
                        email_id = dr[5].ToString(),
                        role_name = dr[6].ToString(),
                        active_status = dr[7].ToString(),
                        designation = dr[8].ToString(),
                        //user_type = dr[9].ToString(),                      
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public List<VM_User_Master> Update_User(VM_User_Master Request)
        {
            List<VM_User_Master> Fill_List = new List<VM_User_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                Int32? mobile_no = 0;
                if (Request.mobile_no.Trim() == "")
                {
                    mobile_no = 0;
                }

                String Sql = "update user_Master set user_password  = '" + Request.user_password.Trim() + "', emp_name = '" + Request.emp_name.Trim() + "',mobile_no =  " + Request.mobile_no.Trim() + ",email_id = '" + Request.email_id + "', role_name = '" + Request.role_name.Trim() + "', active_status = '" + Request.active_status.Trim() + "' , designation = '" + Request.designation.Trim() + "'  where user_name = '" + Request.user_name.Trim() + "'";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 0;
                while (dr.Read())
                {
                    VM_User_Master Data = new VM_User_Master
                    {
                        user_id = dr["user_id"].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public string Select_User_Exist(VM_User_Master objModel)
        {
            String user_name_new = "";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";
                Sql = "SELECT count(*) as user_name_new FROM user_master where user_name = '" + objModel.user_name + "'";

                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                //cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    user_name_new = dr[0].ToString();
                    i++;
                }
            }
            return user_name_new;
        }
        public List<VM_User_Master> Select_User_Dashboad(VM_User_Master objModel)
        {
            List<VM_User_Master> Fill_List = new List<VM_User_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";
                Sql = "SELECT    coalesce(Active_status, 'Active')  AS status, count(user_id) as status_count FROM user_master where role_name <> 'Super Admin'    group by active_status";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_User_Master Data = new VM_User_Master
                    {
                        sr_no = i.ToString(),
                        status = dr[0].ToString().Trim(),
                        status_count = dr[1].ToString().Trim(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public List<VM_User_Master> Select_User_Roles_Dashboad(VM_User_Master objModel)
        {
            List<VM_User_Master> Fill_List = new List<VM_User_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "";
                Sql = "SELECT    coalesce(Role_Name, 'Active')  AS status, count(user_id) as status_count   FROM user_master where role_name <> 'Super Admin'    group by role_name ";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_User_Master Data = new VM_User_Master
                    {
                        sr_no = i.ToString(),
                        status = dr[0].ToString(),
                        status_count = dr[1].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
    }
}