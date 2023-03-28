using Microsoft.AspNetCore.Mvc;
using MVCPracticaTiendaAEN.Models;
using MVCPracticaTiendaAEN.Repositories;

namespace MVCPracticaTiendaAEN.ViewComponents
{
    public class MenuGenerosViewComponent : ViewComponent
    {
        private RepositoryLibros repo;

        public MenuGenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGeneros();
            return View(generos); 
        }
    }
}
