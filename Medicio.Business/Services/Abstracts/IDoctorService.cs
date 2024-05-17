using Medicio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicio.Business.Services.Abstracts
{
    public interface IDoctorService
    {
        void AddDoctor(Doctor doctor);
        void DeleteDoctor(int id);
        void UpdateDoctor(int id,Doctor doctor);
        Doctor GetDoctor(Func<Doctor,bool>? func=null);
        List<Doctor> GetAllDoctors(Func<Doctor, bool>? func = null);
    }
}
