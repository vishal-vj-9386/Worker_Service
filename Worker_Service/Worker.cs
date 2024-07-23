using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Worker_Service.Model;
using Worker_Service.Services;

namespace Worker_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        private List<string> names = new List<string> { "Vishal", "Aman", "Vignesh", "Shivam", "Shiva", "Ashish" };

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Application is started.\n");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var scopped = _serviceProvider.CreateScope();
                var employeeDbHelpder = scopped.ServiceProvider.GetRequiredService<IDataRepository>();
                Random random = new Random();
                int idx = random.Next(names.Count);
                Employee employee = new Employee { 
                    EmployeeId = Guid.NewGuid(),
                    Name = names[idx],
                    DateOfBirth = DateTime.Now.Date
                };
                employeeDbHelpder.AddEmployee(employee);

                int recordCount = employeeDbHelpder.TotalEmployeeCount();

                _logger.LogInformation($"Total number of employees present in database: {recordCount} \n");

                await Task.Delay(5000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Application is closed.");
            await base.StopAsync(cancellationToken);
        }
    }
}
