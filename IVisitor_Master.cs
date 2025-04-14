using Visitors_Management.Models;

namespace Visitors_Management.IRepository
{
    public interface IVisitor_Master
    {
        int Insert_Visitor(VM_Visitor Request);
        int Update_Visitor(VM_Visitor Request);
        List<VM_Visitor> Select_Visitor(VM_Visitor objModel);
        List<VM_Visitor_Stats> Select_Visitor_Stats(string filterType, DateTime? date);

    }
}