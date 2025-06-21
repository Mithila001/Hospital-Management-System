using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync(); // Saves all changes in the current context
    }
}