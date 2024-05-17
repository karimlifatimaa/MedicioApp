using Medicio.Business.Exceptions;
using Medicio.Business.Services.Abstracts;
using Medicio.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicioApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            var doctors=_doctorService.GetAllDoctors();
            return View(doctors);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Doctor doctor)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _doctorService.AddDoctor(doctor);
            }
            catch (EntityNullException ex)
            {
                ModelState.AddModelError("",ex.Message);
              
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(FileSIzeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var doctor=_doctorService.GetDoctor(x=>x.Id == id);
            if (doctor == null) throw new EntityNullException("Entity not found");
            try
            {
                _doctorService.DeleteDoctor(doctor.Id);
            }
            catch (PhotoFileNotFoundException ex)
            {

                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return RedirectToAction("Index");
            }
            catch (EntityNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var doctor= _doctorService.GetDoctor(x=> x.Id == id);
            if(doctor == null) throw new EntityNullException("Entity not found");
           
            return View(doctor);
        }
        [HttpPost]
        public IActionResult Update(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _doctorService.UpdateDoctor(doctor.Id, doctor);
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileSIzeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("Index");
        }
    }
}
