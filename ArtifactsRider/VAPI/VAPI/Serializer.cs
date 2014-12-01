using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace VAPI
{
    public static class Serializer
    {
        public static void Serialize<T>(string file, T data)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(file, settings))
                {
                    ser.Serialize(writer, data);
                }
            }
            catch (Exception e)
            {
                Logger.Write(e.Message + " Plik : "+file);
            }
        }

        public static T Deserialize<T>(string file)
        {
            T data;
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (FileStream stream = new FileStream(file, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    data = (T)ser.Deserialize(reader);
                }
            }
            return data;
        }
    }
}
