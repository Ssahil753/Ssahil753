using Visitors_Management.Models;

namespace Visitors_Management.IRepository
{
    public interface IEmployee_Master
    {
        List<VM_Employee_Master> Fill_Employee(VM_Employee_Master objModel);
    }
}
