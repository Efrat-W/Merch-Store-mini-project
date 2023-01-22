using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace DalApi;
using System.Reflection;
using static DalApi.DalConfig;

// Factory class is used to get the instance of IDal based on the DAL configuration
public static class Factory
{
    // Get method is used to get the instance of IDal based on the DAL configuration
    public static IDal? Get()
    {
        // Extracting DAL name from configuration
        string dalType = s_dalName
            ?? throw new DalConfigException($"DAL name is not extracted from the configuration");

        // Extracting package name from the packages list based on the DAL name
        string dal = s_dalPackages[dalType]
           ?? throw new DalConfigException($"Package for {dalType} is not found in packages list");

        try
        {
            // Loading the package assembly
            Assembly.Load(dal ?? throw new DalConfigException($"Package {dal} is null"));
        }
        catch (Exception)
        {
            throw new DalConfigException($"Failed to load {dal}.dll package");
        }

        // Getting the type of the class Dal.{dal} in the package
        Type? type = Type.GetType($"Dal.{dal}, {dal}")
            ?? throw new DalConfigException($"Class Dal.{dal} was not found in {dal}.dll");

        // Using reflection to get the value of the singleton Instance property of the class
        return type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?
                   .GetValue(null) as IDal
            ?? throw new DalConfigException($"Class {dal} is not singleton or Instance property not found");
    }
}


