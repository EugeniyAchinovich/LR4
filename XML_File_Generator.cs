using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using DB_Table; // пользовательский namespace

namespace XML_Generator
{
    public class XMLGenerator
    {
        public void CreateXMLFile(FileModel obj, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileModel));

            using (var files = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(files, obj);
            }

            try
            {
                XmlReader reader = new XmlReader.Create(filePath);
            }
            catch
            {
                File.Create(@"C:\Users\Asus\Desktop\CreationError");
                // Индикатор ошибок создания файла. Нужен только для отладки...
            }

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();

            schemaSet = schema.InferSchema(reader);
            filePath = Path.ChangeExtension(filePath, ".xsd");

            foreach (XmlSchema s in schemaSet.Schemas())
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        s.Write(xmlWriter);
                    }
                    File.WriteAllText(filePath, stringWriter.ToString());
                }
            }


        }

    }
}
