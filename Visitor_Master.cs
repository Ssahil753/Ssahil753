using QRCoder;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using Visitors_Management.IRepository;
using Visitors_Management.Models;
using static QRCoder.PayloadGenerator;

namespace Visitors_Management.Repository
{
    public class Visitor_Master : IVisitor_Master
    {
        private readonly string _connectionString;
        public Visitor_Master(IConfiguration? connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DBConnection");
        }
        public int Insert_Visitor(VM_Visitor Request)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Check if mobile number is empty, if so, assign an empty string
                string mobile_no = string.IsNullOrWhiteSpace(Request.Mobile_No) ? "" : Request.Mobile_No;

                // Prepare SQL query with GETDATE() to insert the current date/time
                string Sql = "INSERT INTO Visitor (Visitor_Name, Email_Id, Mobile_No, Address, Whom_To_Meet, Dept_Id, Purpose_Of_Visit, Category , Electronic_Accessories, Make, Serial_Number, Photo, Visitor_In_Time) " +
                             "VALUES ('" + Request.Visitor_Name.Trim() + "', '" + Request.Email_Id.Trim() + "', '" + mobile_no + "', '" + Request.Address.Trim() + "', '" + Request.Whom_To_Meet + "', '" + Request.Dept_Id + "', '" + Request.Purpose_Of_Visit.Trim() + "', '"+ Request.Category + "', '" + Request.Electronic_Accessories.Trim() + "', '" + Request.Make.Trim() + "', '" + Request.Serial_Number.Trim() + "', '" + Request.Photo + "', GETDATE())";

                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };

                // Execute the command (you can also use ExecuteNonQuery() instead of ExecuteReader() if you're not reading data)
                SqlDataReader dr = cmd.ExecuteReader();

                // Optionally, you can check the data inserted and get the number of rows affected, or return some other result
                i = 1; // assuming one row is inserted
            }

            return i;
        }


        public int Update_Visitor(VM_Visitor Request)
        {
            int i = 0;
            List<VM_Visitor> Fill_List = new List<VM_Visitor>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                String Sql = "update visitor set  visitor_out_time = getdate(), Outing_Remark = '" + Request.Outing_Remark.Trim() + "' where id = " + Request.Id;
                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };
                SqlDataReader dr = cmd.ExecuteReader();
                i = 0;
            }
            return i;
        }


        public List<VM_Visitor> Select_Visitor(VM_Visitor objModel)
        {
            List<VM_Visitor> Fill_List = new List<VM_Visitor>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string Sql = "";
                if (objModel.Id == 0)
                {
                    Sql = "SELECT Id, Visitor_Name, Email_Id, Mobile_No, Address, dbo.Get_Employee(Whom_To_Meet) as Whom_To_Meet, dbo.Get_Department(Dept_Id) as Dept_Id, Purpose_Of_Visit, Category, FORMAT(Visitor_In_Time, 'dd-MM-yy hh:mm tt') as Visitor_In_Time, Outing_Remark, FORMAT(Visitor_Out_Time, 'dd-MM-yy hh:mm tt') as Visitor_Out_Time, Electronic_Accessories, Make, Serial_Number, photo FROM Visitor ";

                    // Add date filtering conditions
                    if (objModel.Visitor_In_Time != null && objModel.Visitor_Out_Time != null)
                    {
                        Sql += "WHERE CAST(Visitor_In_Time AS DATE) BETWEEN @StartDate AND @EndDate ";
                    }

                    Sql += "ORDER BY id DESC";
                }

                SqlCommand cmd = new SqlCommand(Sql, con)
                {
                    CommandType = CommandType.Text
                };

                // Add parameters for date filtering
                if (objModel.Visitor_In_Time != null && objModel.Visitor_Out_Time != null)
                {
                    cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = objModel.Visitor_In_Time;
                    cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = objModel.Visitor_Out_Time;
                }

                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    VM_Visitor Data = new VM_Visitor
                    {
                        sr_no = i.ToString(),
                        Id = Convert.ToInt32(dr[0]),
                        Visitor_Name = dr[1].ToString(),
                        Email_Id = dr[2].ToString(),
                        Mobile_No = dr[3].ToString(),
                        Address = dr[4].ToString(),
                        Whom_To_Meet = dr[5].ToString(),
                        Dept_Id = dr[6].ToString(),
                        Purpose_Of_Visit = dr[7].ToString(),
                        Category = dr.IsDBNull(8) ? null : dr[8].ToString(),  // Handle possible null value for Category
                        Visitor_In_Time_New = dr[9].ToString(),
                        Outing_Remark = dr[10].ToString(),
                        Visitor_Out_Time_New = dr[11].ToString(),
                        Electronic_Accessories = dr[12].ToString(),
                        Make = dr[13].ToString(),
                        Serial_Number = dr[14].ToString(),
                        Photo = dr[15].ToString(),
                    };

                    Payload? payload = null;

                    var visit = "Visitor Name : " + Data.Visitor_Name + "\n" + "Purpose : For Official Work " + "\n" + "Visit Date :" + DateTime.Now + "\n Approval Status: Approved";
                    payload = new SMS(Data.Mobile_No, visit);

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
                    QRCode qrCode = new QRCode(qrCodeData);
                    var qrCodeAsBitmap = qrCode.GetGraphic(20);
                    string base64String = Convert.ToBase64String(BitmapToByteArray(qrCodeAsBitmap));
                    objModel.QRCode = "data:image/png;base64," + base64String;

                    Data.QRCode = objModel.QRCode;

                    i++;
                    Fill_List.Add(Data);
                }
            }
            return Fill_List;
        }
        public List<VM_Visitor_Stats> Select_Visitor_Stats(string filterType, DateTime? date)
        {
            var list = new List<VM_Visitor_Stats>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string sql = "";

                switch (filterType.ToLower())
                {
                    case "day":
                        sql = @"
            SELECT 
                CONVERT(DATE, Visitor_In_Time AT TIME ZONE 'UTC') AS Period,
                COUNT(*) AS Count
            FROM Visitor
            WHERE 
                Visitor_In_Time IS NOT NULL
                AND MONTH(Visitor_In_Time AT TIME ZONE 'UTC') = MONTH(SYSDATETIMEOFFSET() AT TIME ZONE 'UTC')
                AND YEAR(Visitor_In_Time AT TIME ZONE 'UTC') = YEAR(SYSDATETIMEOFFSET() AT TIME ZONE 'UTC')
            GROUP BY CONVERT(DATE, Visitor_In_Time AT TIME ZONE 'UTC')
            ORDER BY Period DESC";
                        break;

                    case "month":
                        sql = @"
            SELECT 
                FORMAT(Visitor_In_Time, 'yyyy-MM') AS Period,
                COUNT(*) AS Count
            FROM Visitor
            WHERE 
                Visitor_In_Time IS NOT NULL
            GROUP BY FORMAT(Visitor_In_Time, 'yyyy-MM')
            ORDER BY Period DESC";
                        break;

                    case "year":
                        sql = @"
            SELECT 
                YEAR(Visitor_In_Time) AS Period,
                COUNT(*) AS Count
            FROM Visitor
            WHERE 
                Visitor_In_Time IS NOT NULL
            GROUP BY YEAR(Visitor_In_Time)
            ORDER BY Period DESC";
                        break;
                }


                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new VM_Visitor_Stats
                    {
                        Period = dr["Period"].ToString(),
                        Count = Convert.ToInt32(dr["Count"])
                    });
                }
            }
            return list;
        }




        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}