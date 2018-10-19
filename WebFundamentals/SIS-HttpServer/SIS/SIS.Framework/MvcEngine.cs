namespace SIS.Framework
{
    using Routers;
    using Services;
    using System;
    using System.Reflection;
    using WebServer;
    using WebServer.Api;

    public class MvcEngine
    {
        public static void Run(IDependencyContainer dependencyContainer)
        {
            IHttpHandler handler = new ControllerRouter(dependencyContainer);
            Server server = new Server(MvcContext.Get.Port, handler);

            RegisterAssemblyName();

            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void RegisterAssemblyName()
        {
            MvcContext.Get.AssemblyName =
                Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
