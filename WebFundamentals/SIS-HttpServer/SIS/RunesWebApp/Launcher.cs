namespace RunesWebApp
{
    using SIS.Framework;
    using SIS.Framework.Services;
    using SIS.Framework.Services.Implementations;

    public class Launcher
    {
        public static void Main()
        {
            var dependencyContainer = new DependencyContainer();
            dependencyContainer.RegisterDependency<IHashService, HashService>();

            MvcEngine.Run(dependencyContainer);
        }
    }
}
