using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ZeInjector.Logic;

namespace ZeInjector
{
    public static class AccessPoint
    {
        /// <summary>
        /// Used to load services tagged with either 3: IScopedInjector, ITransientInjector, ISingletonInjector
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly
                    .GetTypes()
                    .Where(type => type.IsInterface && Injector.IsFactoryInterface(type));

                foreach (Type type in types)
                {
                    Injector.RegisterFactoryInterface(services, type);
                }
            }
        }

        /// <summary>
        /// Used to load module class libraries.
        /// </summary>
        /// <param name="modulesFolderPath"></param>
        /// <param name="logger"></param>
        public static void LoadLibraries(string modulesFolderPath, ILogger logger)
        {
            Loader.LoadModules(modulesFolderPath, logger);
        }
    }
}