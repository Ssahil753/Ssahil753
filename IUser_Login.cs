using Visitors_Management.Models;

namespace Visitors_Management.IRepository
{
    public interface IUser_Login
    {
        public List<VM_LoginModel> Select_User(VM_LoginModel objModel);
        List<VM_LoginModel> Insert_Token(VM_LoginModel objModel);
        List<VM_LoginModel> Select_Token(VM_LoginModel objModel);
        VM_LoginModel Select_User_Attributes(VM_LoginModel Request);
    }
}

