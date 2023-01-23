using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

using DO;
namespace Dal;
//help class for working with XML files
public class XMLTools
{ 
    public static string dir = @"xml\";
    static XMLTools()
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }


    #region SaveLoadWithXMLSerializer
    /// <summary>
    /// saves a list in XML file  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"> the list for save</param>
    /// <param name="filePath"> the file path</param>
    /// <exception cref="DO.XMLFileLoadCreateException"></exception>
    public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
    {
        try
        {
            FileStream file = new FileStream(filePath, FileMode.Create);
            XmlSerializer x = new XmlSerializer(list.GetType());
            x.Serialize(file, list);
            file.Close();
        }
        catch (Exception ex)
        {
            throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
        }
    }
    /// <summary>
    /// reads and returns a list from the file  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="DO.XMLFileLoadCreateException"></exception>
    public static List<T> LoadListFromXMLSerializer<T>(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                List<T> list;
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                FileStream file = new FileStream(filePath, FileMode.Open);
                list = (List<T>)x.Deserialize(file);
                file.Close();
                return list;
            }
            else
                return new List<T>();
        }
        catch (Exception ex)
        {
           throw new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
        }
    }
    #endregion
}