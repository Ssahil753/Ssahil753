namespace Visitors_Management.Models
{
    public class VM_Visitor
    {
        public String? sr_no { get; set; }
        public Int32 Id { get; set; }
        public String? Visitor_Name { get; set; }
        public String? Email_Id { get; set; }
        public String? Mobile_No { get; set; }
        public String? Address { get; set; }
        public String? Whom_To_Meet { get; set; }
        public String? Dept_Id { get; set; }
        public String? Purpose_Of_Visit { get; set; }

        public String? Category { get; set; }
        public DateTime? Visitor_In_Time { get; set; }
        public DateTime? Visitor_Out_Time { get; set; }
       
        public String? Outing_Remark { get; set; }
        public String? Visitor_In_Time_New { get; set; }
        public String? Visitor_Out_Time_New { get; set; }
        public String? Electronic_Accessories { get; set; }
        public String? Make { get; set; }
        public String? Serial_Number { get; set; }
        public String? Photo { get; set; }
        public byte[]? PhotoData { get; set; }
        public String? QRCode { get; set; }
        public string? FilterType { get; set; }

    }
    public class VM_Visitor_Stats
    {
        

        public string Period { get; set; }
        public int Count { get; set; }
    }

}
