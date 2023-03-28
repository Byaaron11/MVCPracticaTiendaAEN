using Microsoft.AspNetCore.Mvc;
using MVCPracticaTiendaAEN.Extensions;
using MVCPracticaTiendaAEN.Filter;
using MVCPracticaTiendaAEN.Models;
using MVCPracticaTiendaAEN.Repositories;
using System.Diagnostics;
using System.Security.Claims;

namespace MVCPracticaTiendaAEN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RepositoryLibros repo;
        public HomeController(ILogger<HomeController> logger, RepositoryLibros repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        public async Task<IActionResult> Index(int? idgenero, int? posicion)
        {
           
            if (idgenero == null)
            {
                List<Libro> listaLibros = await this.repo.GetAllLibros();
                return View(listaLibros);
            }
            else
            {
                List<Libro> listafiltrada = await this.repo.GetLibrosGenero(idgenero.Value);
                return View(listafiltrada);
            }
        }

        public async Task<IActionResult> Details(int idlibro, int? idlibrocarrito)
        {
            if(idlibrocarrito != null)
            {
                List<int> carrito;
                if(HttpContext.Session.GetObject<List<int>>("IDSLIBROS") == null)
                {
                    // si no hay session, declaramos la lista
                    carrito = new List<int>();
                }
                else
                {
                    //Si ya existe session lo guardamos en carrito
                    carrito =
                        HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
                }
                //y añadimos el elemento que seleccionamos para el carrito
                carrito.Add(idlibrocarrito.Value);
                HttpContext.Session.SetObject("IDSLIBROS", carrito);
                HttpContext.Session.SetString("CANTIDADCARRITO", carrito.Count.ToString());
                ViewData["MENSAJE"] = "Artículo añadido";
            }


            Libro libro = await this.repo.FindLibro(idlibro);
            return View(libro);
        }


        public async Task<IActionResult> Carrito(int? idEliminar)
        {
            List<int> idsLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            if(idsLibros == null)
            {
                ViewData["MENSAJE"] = "Carrito vacío";
                return View();
            }
            else
            {
                if(idEliminar != null)
                {
                    idsLibros.Remove(idEliminar.Value);
                    if(idsLibros.Count() == 0)
                    {
                        //SI LA LISTA QUEDA VACÍA PUES BORRAMOS SESSION
                        HttpContext.Session.Remove("IDSLIBROS");
                        ViewData["MENSAJE"] = "Carrito vacío";
                    }
                    else
                    {
                        //SI TODAVIA QUEDAN ARTICULOS, ACTUALIZAMOS SESSION
                        HttpContext.Session.SetObject("IDSLIBROS", idsLibros);
                    }
                }
                List<Libro> libros = await this.repo.GetLibrosFromCarrito(idsLibros);
                return View(libros);
            }
        }

        [AuthorizeUsers]
        public async Task<IActionResult> Pedidos()
        {
            string data = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int idUser = int.Parse(data);
            List<VistaPedido> pedidos = await this.repo.GetPedidosUsuario(idUser);

            return View(pedidos);
        }







        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}