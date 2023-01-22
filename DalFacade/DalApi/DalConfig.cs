using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;
using System.Xml.Linq;

// DalConfig class is used to load and store the DAL configuration from an XML file
static class DalConfig
{
    // Field to store the DAL name
    internal static string? s_dalName;
    // Field to store the DAL package name
    internal static Dictionary<string, string> s_dalPackages;

    // Constructor to load the DAL configuration from the XML file
    static DalConfig()
    {
        // Loading the DAL configuration XML file
        XElement dalConfig = XElement.Load(@"..\xml\dal-config.xml")
            ?? throw new DalConfigException("dal-config.xml file is not found");

        // Extracting the DAL name from the XML
        s_dalName = dalConfig?.Element("dal")?.Value
            ?? throw new DalConfigException("<dal> element is missing");

        // Extracting the DAL package names from the XML
        var packages = dalConfig?.Element("dal-packages")?.Elements()
            ?? throw new DalConfigException("<dal-packages> element is missing");

        // Storing the DAL package names in a dictionary
        s_dalPackages = packages.ToDictionary(p => "" + p.Name, p => p.Value);
    }
}


[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}

