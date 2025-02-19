using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Utilities.BackgroundServices
{
    public class AppointmentStatusUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppointmentStatusUpdater(IServiceProvider _serviceProvider)
        {
            this._serviceProvider = _serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {

                using (var scope=_serviceProvider.CreateScope()) {

                    var unitofwork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                
                    //ageeb el appointments el upcoming ely el date beta3ha 22al men enharda
                    var appointments=await unitofwork.AppointmentRepository.getAllAsync(a=> a.AppointmentDateTime <= DateTime.Now && a.Status==AppointmentStatus.UPCOMING);
                    Console.WriteLine("Checking appointments...");

                    foreach (var appointment in appointments)
                    {
                        Console.WriteLine(appointment.AppointmentDateTime);

                        appointment.Status = AppointmentStatus.COMPLETED;
                        unitofwork.AppointmentRepository.Update(appointment);
                    }
                    await unitofwork.completeAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);//Waits Before Running Again runs every 1 hour


            }
        }
    }
}
