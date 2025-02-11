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
    public class MedicalHistoriesRepository:BaseRepository<MedicalHistory>,IMedicalHistoriesRepository
    {
        private readonly HospitalContext _context;
        public MedicalHistoriesRepository(HospitalContext _context):base(_context)
        {
            this._context = _context;
        }
    }
}
