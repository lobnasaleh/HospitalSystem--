using HMS.DataAccess.Data;
using HMS.Entites.Interfaces;
using HMS.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataAccess.Repository
{
    public class PatientRepository:BaseRepository<Patient>,IPatientRepository
    {
        private readonly HospitalContext Context;
        public PatientRepository(HospitalContext Context):base(Context)
        {
            this.Context = Context;
        }
       
    }
}
