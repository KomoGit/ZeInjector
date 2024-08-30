using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ZeInjector.Logic
{
    public static class Loader
    {
        internal static void LoadModules(string modulesFolderPath, ILogger logger)
        {

            List<string> errFolders = [];
            // Check if the specified folder exists
            if (!Directory.Exists(modulesFolderPath))
            {
                throw new DirectoryNotFoundException($"The specified folder '{modulesFolderPath}' does not exist.");
            }

            // Get all subdirectories in the modules folder
            var moduleFolders = Directory.GetDirectories(modulesFolderPath);

            // Load assemblies from each folder
            foreach (var moduleFolder in moduleFolders)
            {
                try
                {
                    // Load the assembly by folder name
                    var assembly = Assembly.Load(new DirectoryInfo(moduleFolder).Name);

                    // Get the types from the assembly
                    var types = assembly.GetTypes();

                    // Find and instantiate classes with default constructor
                    foreach (var type in types)
                    {
                        if (!type.IsAbstract && !type.IsInterface && type.GetConstructor(Type.EmptyTypes) != null)
                        {
                            var module = Activator.CreateInstance(type);
                            // Optionally, you can call a method on the module here if needed
                        }
                    }
                }
                catch (Exception ex)
                {
                    errFolders.Add(moduleFolder);
                }
                finally
                {
                    logger.LogWarning($"Failed to load {moduleFolder.Length} folders.");
                    foreach (var folder in errFolders)
                    {
                        logger.LogWarning(message: $"Failed to load module from folder '{folder}', check if they are empty.");
                    }
                }
            }
        }
    }
}