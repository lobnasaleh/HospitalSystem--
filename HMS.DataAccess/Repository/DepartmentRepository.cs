using HMS.DataAccess.Data;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;


namespace HMS.DataAccess.Repository
{
    public class DepartmentRepository : BaseRepository<Department>,IDepartmentRepository
    {
      private readonly  HospitalContext Context;

        public DepartmentRepository(HospitalContext Context):base(Context)
        {
            this.Context = Context;
        }
    
    }
}
