using HMS.DataAccess.Data;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.DataAccess.Repository
{
    public class AppointmentRepository : BaseRepository<Appointment>,IAppointmentRepository
    {
       private readonly HospitalContext context;
        public AppointmentRepository(HospitalContext context):base(context) 
        {
            this.context = context;
        }

       
    }
}
