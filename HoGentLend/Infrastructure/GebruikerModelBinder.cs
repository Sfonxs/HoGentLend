using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using HoGentLend.Models.DAL;

namespace HoGentLend.Infrastructure
{
    public class GebruikerModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IGebruikerRepository repos = (IGebruikerRepository)DependencyResolver.Current.GetService(typeof(IGebruikerRepository));
                ApplicationUserManager userManager = controllerContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser appUser = userManager.FindByNameAsync(controllerContext.HttpContext.User.Identity.Name).Result;

                if (appUser != null)
                {
                    Gebruiker g = repos.FindByEmail(appUser.Email);
                    return g;
                }
                return null;
            }   
            return null;

        }
    }
}