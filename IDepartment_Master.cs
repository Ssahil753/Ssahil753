using Visitors_Management.Models;

namespace Visitors_Management.IRepository
{
    public interface IDepartment_Master
    {
        List<VM_Department_Master> Fill_Department(VM_Department_Master objModel);
    }
}

