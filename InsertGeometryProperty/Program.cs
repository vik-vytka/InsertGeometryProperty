using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace InsertGeometryProperty
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            ChangeXML(path);
            Console.WriteLine("Готово");
            Console.ReadKey();
        }
        
        static void ChangeXML(string path)
        {
            List<string> names = new List<string>() { "Point", "LineString", "Polygon" };
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;
            XNamespace space = root.Name.Namespace;
            var features = root.Elements(space.GetName("featureMember"));
            foreach(XElement feature in features)
            {
                foreach (XElement obj in feature.Elements())
                {
                    foreach (string name in names)
                    {
                        XElement el = obj.Element(space + name);
                        if (el != null)
                        {
                            XElement pr = new XElement(space.GetName("geometryProperty"));
                            pr.Add(el);
                            el.Remove();
                            obj.Add(pr);
                        }
                    }
                }
            }
            string newPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "1.gml");
            doc.Save(newPath);
        }
    }
}
