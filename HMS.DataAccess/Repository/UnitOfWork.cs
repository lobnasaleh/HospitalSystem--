using HMS.DataAccess.Data;
using HMS.Entites.Interfaces;
using HMS.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HospitalContext _context;

        public UnitOfWork(HospitalContext _context)
        {
            this._context = _context;
            patientRepository = new PatientRepository(_context);
            MedicalHistories=new MedicalHistoriesRepository(_context);
            StaffRepository=new StaffRepository(_context);  
            ScheduleRepository=new ScheduleRepository(_context);
            StaffScheduleRepository=new StaffScheduleRepository(_context);
            AppointmentRepository=new AppointmentRepository(_context);
            DepartmentRepository=new DepartmentRepository(_context);
        }

        public IPatientRepository patientRepository { get; private set; }

        public IMedicalHistoriesRepository MedicalHistories { get; private set; }

        public IStaffRepository StaffRepository { get; private set; }

        public IScheduleRepository ScheduleRepository { get; private set; }

        public IStaffScheduleRepository StaffScheduleRepository { get; private set; }

        public IAppointmentRepository AppointmentRepository { get; private set; }

        public IDepartmentRepository DepartmentRepository { get; private set; }

        public async Task<int> completeAsync()
        {
          return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
