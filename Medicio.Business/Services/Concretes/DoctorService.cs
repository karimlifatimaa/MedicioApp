using Medicio.Business.Exceptions;
using Medicio.Business.Services.Abstracts;
using Medicio.Core.Models;
using Medicio.Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicio.Business.Services.Concretes
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DoctorService(IDoctorRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null) throw new EntityNullException("Entity not found");
            if(doctor.PhotoFile==null) throw new EntityNullException("Entity not found");
            if (!doctor.PhotoFile.ContentType.Contains("image/")) throw new FileContentTypeException("PhotoFile", "Content type error!!!");
            if (doctor.PhotoFile.Length > 2097152) throw new FileSIzeException("PhotoFile", "File size error!!!");
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(doctor.PhotoFile.FileName);
            string path = _webHostEnvironment.WebRootPath + @"\Uploads\Doctor\" + filename;
            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                doctor.PhotoFile.CopyTo(stream);
            }
            doctor.ImgUrl = filename;
            _repository.Add(doctor);
            _repository.Commit();
        }
        
        public void DeleteDoctor(int id)
        {
            var doctor=_repository.Get(x=>x.Id == id);
            if(doctor == null) throw new EntityNullException("Entity not found");
            string path = _webHostEnvironment.WebRootPath + @"\Uploads\Doctor\" + doctor.ImgUrl;
            if (!File.Exists(path)) throw new PhotoFileNotFoundException("ImgUrl", "File not found");
            File.Delete(path);
            _repository.Delete(doctor);
            _repository.Commit();
        }

        public List<Doctor> GetAllDoctors(Func<Doctor, bool>? func = null)
        {
            return _repository.GetAll(func);
        }

        public Doctor GetDoctor(Func<Doctor, bool>? func = null)
        {
            return _repository.Get(func);
        }

        public void UpdateDoctor(int id, Doctor doctor)
        {
            var oldDoctor = _repository.Get(x => x.Id == id);
            if (oldDoctor == null) throw new EntityNullException("Entity not found");
            if(doctor.PhotoFile != null)
            {
                if(!doctor.PhotoFile.ContentType.Contains("image/")) throw new FileContentTypeException("PhotoFile", "Content type error!!!");
                if(doctor.PhotoFile.Length> 2097152) throw new FileSIzeException("PhotoFile", "File size error!!!");
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(doctor.PhotoFile.FileName);
                string path= _webHostEnvironment.WebRootPath + @"\Uploads\Doctor\" + filename;
                using (FileStream stream=new FileStream(path, FileMode.Create))
                {
                    doctor.PhotoFile.CopyTo(stream);
                }
                oldDoctor.ImgUrl=filename; 

            }
            oldDoctor.Position = doctor.Position;
            oldDoctor.Name= doctor.Name;
            _repository.Commit();
        }
    }
}
