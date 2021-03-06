﻿using HoGentLend.Infrastructure;
using HoGentLend.Models.DAL;
using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HoGentLend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HoGentLendContext.Init();
            ModelBinders.Binders.Add(typeof(Gebruiker), new GebruikerModelBinder());
        }
    }
}
