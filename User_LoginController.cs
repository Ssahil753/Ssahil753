using Microsoft.AspNetCore.Mvc;
using Visitors_Management.IRepository;
using Visitors_Management.Models;

namespace Visitors_Management.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class User_LoginController : ControllerBase
    {
        private readonly IUser_Login _IUser_Login;
        public User_LoginController(IUser_Login IUser_Login)
        {
            _IUser_Login = IUser_Login;
        }
        public VM_LoginModel Select_User_Attributes(VM_LoginModel objModel)
        {
            var Result = _IUser_Login.Select_User_Attributes(objModel);
            return Result;
        }
    }
}


