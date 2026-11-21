using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BlueShell.Application.Helper
{
    public static class XmlHelper
    {
        public static T? ConvertXml<T>() where T : class, new()
        {
            if (!FindXml(typeof(T).Name, out string configFile))
                return new T();

            try
            {
                XmlSerializer serializer = new(typeof(T));
                using FileStream fs = new(configFile, FileMode.Open);
                return (T)serializer.Deserialize(fs)!;
            }
            catch (XmlException ex)
            {
                Console.WriteLine(ex.Message);
                return new T();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new T();
            }
        }

        private static bool FindXml(string configFile, out string configFilePath)
        {
            string configPath = Path.Combine(AppContext.BaseDirectory, "Configs");
            if (configFile.StartsWith("Xml"))
                configFile = configFile[3..];

            configFilePath = Path.Combine(configPath, configFile + ".xml");
            return File.Exists(configFilePath);
        }
    }
}
