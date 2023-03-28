using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MVCPracticaTiendaAEN.Filter
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute,
        IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //PRIMERO DE NADA HARÉ QUE LA REDIRECCION SEA DINÁMICA
            //Así si pincha en carrito, que al iniciar sesion le lleve al carrito, y no a Index de Home
            string controller =
                context.RouteData.Values["controller"].ToString();
            string action =
                context.RouteData.Values["action"].ToString();

            ITempDataProvider provider =
                context.HttpContext.RequestServices
                .GetService<ITempDataProvider>();

            var TempData = provider.LoadTempData(context.HttpContext);

            TempData["controller"] = controller;
            TempData["action"] = action;

            provider.SaveTempData(context.HttpContext, TempData);


            var user = context.HttpContext.User;
            if(user.Identity.IsAuthenticated == false)
            {
                //No esta autentificado pues me lo llevo a login
                RouteValueDictionary rutaLogin =
                   new RouteValueDictionary(
                           new { controller = "Managed", action = "Login" }
                       );
                context.Result =
                    new RedirectToRouteResult(rutaLogin);
            }
        }
    }
}
