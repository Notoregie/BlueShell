using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BlueShell.Helper
{
    public static class XmlHelper
    {
        public static T? ConvertXml<T>() where T: class, new()
        {
            if (!FindXml(typeof(T).Name, out string configFile)) return new T();

            try
            {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream fs = new FileStream(configFile, FileMode.Open))
                {
                    return (T)serializer.Deserialize(fs);
                }
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

        static bool FindXml(string configFile, out string configFilePath)
        {
            string configPath = @"C:\Users\kevin\source\repos\BlueShell\BlueShell\Configs\";
            if (configFile.StartsWith("Xml"))
                configFile = configFile.Remove(0, 3);

            configFilePath = Path.Combine(configPath, configFile + ".xml");
            return File.Exists(configFilePath);
        }
    }
}
