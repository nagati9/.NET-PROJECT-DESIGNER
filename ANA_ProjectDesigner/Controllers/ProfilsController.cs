using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models;
using ANA_ProjectDesigner.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Controllers
{
    public class ProfilsController : Controller
    {
        private readonly ProfilDBContext profilDBContext;

        public ProfilsController(ProfilDBContext profilDBContext)
        {
            this.profilDBContext = profilDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> ListUserProfils()
        {
           var profils = await profilDBContext.Profils.ToListAsync();
            return View(profils);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProfilViewModel addProfilRequest)
        {
            var profil = new Profils()
            {
                Id = Guid.NewGuid(),
                FirstName = addProfilRequest.FirstName,
                LastName = addProfilRequest.LastName,
                PhoneNumber = addProfilRequest.PhoneNumber,
                Email = addProfilRequest.Email,
                Password = addProfilRequest.Password,
                Country = addProfilRequest.Country
                
            };

            await profilDBContext.Profils.AddAsync(profil);
            await profilDBContext.SaveChangesAsync();
            return RedirectToAction("ListUserProfils");
        }

        [HttpGet]
        public async Task<IActionResult> DataProfil(Guid id)
        {
            var profil = await profilDBContext.Profils.FirstOrDefaultAsync(x => x.Id == id);

            if (profil !=null)
            {
                var viewModel = new UpdateProfilViewModel()
                {
                    FirstName = profil.FirstName,
                    LastName = profil.LastName,
                    PhoneNumber = profil.PhoneNumber,
                    Email = profil.Email,
                    Password = profil.Password,
                    Country= profil.Country
                };
                return View(viewModel);
            }

            return RedirectToAction("ListUserProfils");
        }


        [HttpPost]
        public async Task<IActionResult> DataProfil(UpdateProfilViewModel model)
        {
            var profil = await profilDBContext.Profils.FindAsync(model.Id);

            if (profil != null)
            {
                profil.FirstName = model.FirstName;
                profil.LastName = model.LastName;
                profil.PhoneNumber = model.PhoneNumber;
                profil.Email = model.Email;
                profil.Password = model.Password;
                profil.Country = model.Country;

                await profilDBContext.SaveChangesAsync();

                return RedirectToAction("ListUserProfils");
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProfilViewModel model)
        {
            var profil = await profilDBContext.Profils.FindAsync(model.Id);

            if (profil != null)
            {
                profilDBContext.Profils.Remove(profil);
                await profilDBContext.SaveChangesAsync();

                return RedirectToAction("ListUserProfils");
            }
            return RedirectToAction("ListUserProfils");
        }

    }
}
