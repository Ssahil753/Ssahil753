using System.Data;
using System.Data.SqlClient;
using Visitors_Management.IRepository;
using Visitors_Management.Models;

namespace Visitors_Management.Repository
{
    public class Employee_Master : IEmployee_Master
    {
        private readonly string _connectionString;
        public Employee_Master(IConfiguration? connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DBConnection");
        }
        public List<VM_Employee_Master> Fill_Employee(VM_Employee_Master objModel)
        {
            List<VM_Employee_Master> Fill_List = new List<VM_Employee_Master>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                String Sql = "SELECT Emp_ID,Emp_Name FROM  employee_master";
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_Employee_Master Data = new VM_Employee_Master
                    {
                        Emp_Id = dr[0].ToString(),
                        Emp_Name = dr[1].ToString(),
                    };
                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }

    }
}