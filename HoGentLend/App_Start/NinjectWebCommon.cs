using HoGentLend.Models.DAL;
using HoGentLend.Models.Domain;
using HoGentLend.Models.Domain.DAL;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HoGentLend.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(HoGentLend.App_Start.NinjectWebCommon), "Stop")]

namespace HoGentLend.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Models.Domain.HoGentApi;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<HoGentLendContext>().ToSelf().InRequestScope();

            kernel.Bind<IMateriaalRepository>().To<MateriaalRepository>().InRequestScope();
            kernel.Bind<IGroepRepository>().To<GroepRepository>().InRequestScope();
            kernel.Bind<IGebruikerRepository>().To<GebruikerRepository>().InRequestScope();
            kernel.Bind<IReservatieRepository>().To<ReservatieRepository>().InRequestScope();
            kernel.Bind<IConfigWrapper>().To<ConfigWrapper>().InRequestScope();

            kernel.Bind<IHoGentApiLookupProvider>().To<HoGentApiLookupProvider>().InRequestScope();
          //  kernel.Bind<IHoGentApiLookupProvider>().To<HoGentApiLookupProvider>().InRequestScope();
        }        
    }
}
