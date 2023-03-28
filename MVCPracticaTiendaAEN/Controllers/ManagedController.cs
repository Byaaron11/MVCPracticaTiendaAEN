using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVCPracticaTiendaAEN.Models;
using MVCPracticaTiendaAEN.Repositories;
using System.Security.Claims;

namespace MVCPracticaTiendaAEN.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;

        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {
            Usuario user = await this.repo.Findusuario(email, pass);
            if(user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                Claim claimUsername =
                   new Claim(ClaimTypes.Name, user.Nombre);
                Claim claimRole =
                    new Claim(ClaimTypes.Role, "USUARIO");
                Claim claimIdUser =
                    new Claim(ClaimTypes.NameIdentifier, user.Idusuario.ToString());
                Claim claimFoto =
                    new Claim("FOTO", user.Foto.ToString());

                identity.AddClaim(claimUsername);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimIdUser);
                identity.AddClaim(claimFoto);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                return RedirectToAction(action, controller);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Perfil()
        {
            int idUser = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Usuario user = await this.repo.GetUsuario(idUser);
            return View(user);
        }
    }
}
