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
        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            string storedGuid = HttpContext.Session.GetString("idUser");
            if (Guid.TryParse(storedGuid, out Guid profilUserId))
            {
                var projects = await profilDBContext.Project
                .Where(p => p.ProfileId == profilUserId)
                .ToListAsync();

                return View("Overview", projects);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View(); // This view should contain the form for creating a new project in a popup
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(Guid id)
        {
            var project = await profilDBContext.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project); // This view should contain the form for editing a project in a popup
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await profilDBContext.Project.FindAsync(id);

            if (project != null)
            {
                profilDBContext.Project.Remove(project);
                await profilDBContext.SaveChangesAsync();
                return RedirectToAction("Overview"); // Redirige vers la liste des projets après la suppression
            }

            return RedirectToAction("Overview"); // Redirection si le projet n'est pas trouvé
        }



        [HttpPost]
        public async Task<IActionResult> CreateProject(string name,string description)
        {
            string storedGuid = HttpContext.Session.GetString("idUser");
            if (Guid.TryParse(storedGuid, out Guid profilUserId))
            {
                var project = new Models.Domain.Project()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Description = description,
                    ProfileId = profilUserId
                };

                await profilDBContext.Project.AddAsync(project);
                await profilDBContext.SaveChangesAsync();
            
            return RedirectToAction("Overview"); // Redirect to the project list after creation
            }

            return RedirectToAction("Overview"); // Return to the create view if model state is invalid
        }

        [HttpPost]
        public async Task<IActionResult> EditProject(Guid id, string editName, string editDescription)
        {
            var project = await profilDBContext.Project.FindAsync(id);

            if (project != null)
            {
                project.Name = editName;
                project.Description = editDescription;

                profilDBContext.Entry(project).State = EntityState.Modified;
                await profilDBContext.SaveChangesAsync();

                return RedirectToAction("Overview"); // Rediriger vers la liste des projets après la modification
            }

            return RedirectToAction("Overview"); // Rediriger vers la liste des projets si le projet n'est pas trouvé
        }



    }
}
