using Visitors_Management.Models;

namespace Visitors_Management.IRepository
{
    public interface IUser_Master
    {
        List<VM_User_Master> Select_User(VM_User_Master objModel);
        List<VM_User_Master> Select_User_Dashboad(VM_User_Master objModel);
        List<VM_User_Master> Select_User_Roles_Dashboad(VM_User_Master objModel);
        List<VM_User_Master> Insert_User(VM_User_Master Request);
        List<VM_User_Master> Update_User(VM_User_Master Request);
        string Select_User_Exist(VM_User_Master objModel);


    }
}


