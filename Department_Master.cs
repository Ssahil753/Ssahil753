using System.Data;
using System.Data.SqlClient;
using Visitors_Management.IRepository;
using Visitors_Management.Models;
namespace Visitors_Management.Repository
{

    public class Department_Master : IDepartment_Master
    {
        private readonly string _connectionString;
        public Department_Master(IConfiguration? connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DBConnection");
        }
        public List<VM_Department_Master> Fill_Department(VM_Department_Master objModel)
        {
            List<VM_Department_Master> Fill_List = new List<VM_Department_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "SELECT id, dept_name FROM  department_master";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_Department_Master Data = new VM_Department_Master
                    {
                        Id = dr[0].ToString(),
                        Dept_Name = dr[1].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }

    }
}
