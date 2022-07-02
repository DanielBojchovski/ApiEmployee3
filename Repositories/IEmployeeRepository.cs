using ApiEmployee3.Models;

namespace ApiEmployee3.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> Search(string name);
        Task<IEnumerable<Employee>> SearchBySkill(string skill);
        Task<IEnumerable<Employee>> Get();
        Task<Employee> Get(int employeeId);
        Task<Employee> GetEmployeeByEmail(string email);
        Task<Employee> Create(Employee employee);
        Task<Employee> Update(Employee employee);
        Task Delete(int employeeId);
    }
}
