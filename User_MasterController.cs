using Microsoft.AspNetCore.Mvc;
using Visitors_Management.IRepository;
using Visitors_Management.Models;

namespace Visitors_Management.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class User_MasterController : ControllerBase
    {
        private readonly IUser_Master _IUser_Master;
        public User_MasterController(IUser_Master IUser_Master)
        {
            _IUser_Master = IUser_Master;
        }
        public List<VM_User_Master> Insert_User(VM_User_Master objModel)
        {
            var Result = _IUser_Master.Insert_User(objModel);
            return Result;
        }
        public List<VM_User_Master> Update_User(VM_User_Master objModel)
        {
            var Result = _IUser_Master.Update_User(objModel);
            return Result;
        }
        // [//Authorize(Roles = "Administrator")]

        public List<VM_User_Master> Select_User(VM_User_Master objModel)
        {
            var Result = _IUser_Master.Select_User(objModel);
            return Result;
        }
        public string Select_User_Exist(VM_User_Master objModel)
        {
            var Result = _IUser_Master.Select_User_Exist(objModel);
            return Result;
        }
        public List<VM_User_Master> Select_User_Roles_Dashboad(VM_User_Master objModel)
        {
            var Result = _IUser_Master.Select_User_Roles_Dashboad(objModel);
            return Result;
        }
        public List<VM_User_Master> Select_User_Dashboad(VM_User_Master objModel)
        {
            var Result = _IUser_Master.Select_User_Dashboad(objModel);
            return Result;
        }

    }
}