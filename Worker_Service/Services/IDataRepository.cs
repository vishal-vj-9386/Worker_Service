using Worker_Service.Model;

namespace Worker_Service.Services
{
    public interface IDataRepository
    {
        int TotalEmployeeCount();
        void AddEmployee(Employee employee);
    }
}
