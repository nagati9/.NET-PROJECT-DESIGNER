using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models;
using ANA_ProjectDesigner.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Controllers
{
    public class ProfilController : Controller
    {
        private readonly MyDBContext profilDBContext;

        public ProfilController(MyDBContext profilDBContext)
        {
            this.profilDBContext = profilDBContext;
        }

       
        [HttpGet]
        public async Task<IActionResult> ListUserProfils(Guid profilUserId)
        {
            ViewBag.profilUserId = profilUserId;
            var profils = await profilDBContext.Profil.ToListAsync();
            return View(profils);
          //  return RedirectToAction("Welcome");
        }
        [HttpGet]
        public IActionResult Welcome()
        {
            string storedGuid = HttpContext.Session.GetString("idUser");
            if (Guid.TryParse(storedGuid, out Guid profilUserId))
            {
                ViewBag.profilUserId = profilUserId;
            }
            return View(profilUserId);
        }
        [HttpGet]
        public IActionResult Register(string error)
        {
            ViewBag.Error = error;
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> loginUser(string Email, string Password)
        {
            var user = await profilDBContext.Profil.FirstOrDefaultAsync(x => x.Email == Email);

            if (user != null && (Password == user.Password))
            {
                HttpContext.Session.SetString("idUser", user.Id.ToString());
                return RedirectToAction("Welcome");
            }

            return RedirectToAction("Register", "Profil", new { error = "Identifiants incorrects" });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProfilViewModel addProfilRequest)
        {
            var profil = new Profil()
            {
                Id = Guid.NewGuid(),
                FirstName = addProfilRequest.FirstName,
                LastName = addProfilRequest.LastName,
                PhoneNumber = addProfilRequest.PhoneNumber,
                Email = addProfilRequest.Email,
                Password = addProfilRequest.Password,
                Country = addProfilRequest.Country
                
            };

           

            await profilDBContext.Profil.AddAsync(profil);
            await profilDBContext.SaveChangesAsync();
            HttpContext.Session.SetString("idUser", profil.Id.ToString());
            return RedirectToAction("Welcome", "Profil");
           
        }

        [HttpGet]
        public async Task<IActionResult> DataProfil(Guid id)
        {
            var profil = await profilDBContext.Profil.FirstOrDefaultAsync(x => x.Id == id);

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
            var profil = await profilDBContext.Profil.FindAsync(model.Id);

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
            var profil = await profilDBContext.Profil.FindAsync(model.Id);

            if (profil != null)
            {
                profilDBContext.Profil.Remove(profil);
                await profilDBContext.SaveChangesAsync();

                return RedirectToAction("ListUserProfils");
            }
            return RedirectToAction("ListUserProfils");
        }

    }
}
