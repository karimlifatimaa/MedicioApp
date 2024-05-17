using Medicio.Core.Models;
using Medicio.Core.RepositoryAbstracts;
using Medicio.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicio.Data.RepositoryConcretes
{
    public class DoctorREpository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorREpository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
