using HMS.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        public IPatientRepository patientRepository { get; }
        public IMedicalHistoriesRepository MedicalHistoriesRepository {  get; }
        public IStaffRepository StaffRepository { get; }
        public IScheduleRepository ScheduleRepository { get; }
        public IStaffScheduleRepository StaffScheduleRepository { get; }
        public IAppointmentRepository AppointmentRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }

        public Task<int> completeAsync();
       
    }
}
