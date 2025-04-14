
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visitors_Management.Models;
using Visitors_Management.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static QRCoder.PayloadGenerator;
using QRCoder;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Linq;
using Visitors_Management.Repository;


namespace Visitors_Management.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Visitor_MasterController : ControllerBase
    {
        private readonly IDepartment_Master _IDepartment_Master;
        private readonly IEmployee_Master _IEmployee_Master;
        private readonly IVisitor_Master _IVisitor_Master;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public byte[] Photo { get; private set; }

        public Visitor_MasterController(IWebHostEnvironment webHostEnvironment, IDepartment_Master IDepartment_Master, IEmployee_Master IEmployee_Master, IVisitor_Master IVisitor_Master)
        {
            _IDepartment_Master = IDepartment_Master;
            _IEmployee_Master = IEmployee_Master;
            _IVisitor_Master = IVisitor_Master;
            _webHostEnvironment = webHostEnvironment;
        }
        public List<VM_Department_Master> Fill_Department(VM_Department_Master objModel)
        {
            var Result = _IDepartment_Master.Fill_Department(objModel);
            return Result;
        }
        public List<VM_Employee_Master> Fill_Employee(VM_Employee_Master objModel)
        {
            var Result = _IEmployee_Master.Fill_Employee(objModel);
            return Result;
        }
        public int Insert_Visitor(VM_Visitor objModel)
        {
            byte[] imageBytes = Convert.FromBase64String(objModel.Photo.Split(',')[1]);
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "Photo");
            string fileName = filePath + "\\" + objModel.Mobile_No + ".jpg";

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    image.Save(fileName, ImageFormat.Jpeg); // Change format if needed
                }
            }
            var Result = _IVisitor_Master.Insert_Visitor(objModel);
            return Result;
        }
        public List<VM_Visitor> Select_Visitor(VM_Visitor objModel)
        {


            var Result = _IVisitor_Master.Select_Visitor(objModel);
            return Result;
        }
        [HttpPost]
        public IActionResult Select_Visitor_Stats([FromBody] VM_Visitor model)
        {
            string filterType = model.FilterType;
            DateTime? date = model.Visitor_In_Time; // You can pass only one date
            var stats = _IVisitor_Master.Select_Visitor_Stats(filterType, date);

            return Ok(stats);
        }


        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public int Update_Visitor(VM_Visitor objModel)
        {
            var Result = _IVisitor_Master.Update_Visitor(objModel);
            return Result;
        }

    }
}
