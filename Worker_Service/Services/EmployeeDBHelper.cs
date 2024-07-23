using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Worker_Service.Model;

namespace Worker_Service.Services
{
    public class EmployeeDBHelper : IDataRepository
    {
        private EmployeeDBContext _dbContext;
        private readonly ILogger<EmployeeDBHelper> _logger;
        private readonly IConfiguration _configuration;

        public EmployeeDBHelper(IConfiguration configuration, EmployeeDBContext employeeDBContext, ILogger<EmployeeDBHelper> logger)
        {
            _configuration = configuration;
            _dbContext = employeeDBContext;
            _logger = logger;
        }

        public int TotalEmployeeCount()
        {
            return _dbContext.Employees.ToList().Count;
        }

        public void AddEmployee(Employee employee)
        {
            try
            {
                _dbContext.Employees.Add(employee);
                _logger.LogInformation($"{employee.Name} is added successfully in database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in adding employee in database: {ex.Message}");
            }
           

            _dbContext.SaveChanges();
        }
    }
}
