using Microsoft.Extensions.DependencyInjection;
using ZeInjector.Interfaces;

namespace ZeInjector.Logic
{
    public static class Injector
    {
        internal static bool IsFactoryInterface(Type type)
        {
            return type.GetInterfaces().Any(i =>
                i.IsGenericType &&
                (i.GetGenericTypeDefinition() == typeof(IScopedInjector<,>) ||
                 i.GetGenericTypeDefinition() == typeof(ISingletonInjector<,>) ||
                 i.GetGenericTypeDefinition() == typeof(ITransientInjector<,>)));
        }

        internal static void RegisterFactoryInterface(IServiceCollection services, Type factoryInterface)
        {
            Type genericInterfaceType = GetFactoryInterface(factoryInterface);

            if (genericInterfaceType != null)
            {
                Type serviceType = genericInterfaceType.GetGenericArguments()[0];
                Type implementationType = genericInterfaceType.GetGenericArguments()[1];

                if (genericInterfaceType.GetGenericTypeDefinition() == typeof(IScopedInjector<,>))
                {
                    services.AddScoped(serviceType, implementationType);
                }
                else if (genericInterfaceType.GetGenericTypeDefinition() == typeof(ISingletonInjector<,>))
                {
                    services.AddSingleton(serviceType, implementationType);
                }
                else if (genericInterfaceType.GetGenericTypeDefinition() == typeof(ITransientInjector<,>))
                {
                    services.AddTransient(serviceType, implementationType);
                }
            }
        }

        private static Type GetFactoryInterface(Type serviceInterface)
        {
            // Check if the service interface implements any factory interface excluding IDependencyFactory
            foreach (Type iface in serviceInterface.GetInterfaces())
            {
                if (iface.IsGenericType &&
                    (iface.GetGenericTypeDefinition() == typeof(IScopedInjector<,>) ||
                     iface.GetGenericTypeDefinition() == typeof(ISingletonInjector<,>) ||
                     iface.GetGenericTypeDefinition() == typeof(ITransientInjector<,>)))
                {
                    return iface;
                }
            }
            return null;
        }
    }
}