/*
 * Простая утилита, обновляющая версию в MsBuild-проекте.
 * Командная строка:
 *
 * PatchVersion <file-name> <new-version>
 * 
 */

using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchVersion
{
    internal static class Program
    {
        private const string PropertyGroup = "PropertyGroup";

        /// <summary>
        /// Устанавливает значение элемента в PropertyGroup
        /// </summary>
        /// <param name="document">Документ проекта</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="value">Новое значение свойства</param>
        private static void SetValue
            (
                XDocument document, 
                string propertyName, 
                string value
            )
        {
            var root = document.Root;
            if (root is null)
            {
                throw new Exception("Root element not found");
            }

            var propertyGroup = root.Descendants(PropertyGroup).FirstOrDefault();
            if (propertyGroup is null)
            {
                propertyGroup = new XElement(PropertyGroup);
                root.Add(propertyGroup);
            }

            var sought = root.XPathSelectElement($"./{PropertyGroup}/{propertyName}");
            if (sought is null)
            {
                sought = new XElement(propertyName, value);
                propertyGroup.Add(sought);
            }

            sought.Value = value;

        } // method SetValue
        
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: PatchVersion <file-name> <new-version>");
                return 1;
            }

            var inputFile = args[0];
            var versionText = args[1];

            var document = XDocument.Load(inputFile);
            
            SetValue(document, "Version", versionText);
            SetValue(document, "AssemblyVersion", versionText);
            SetValue(document, "PackageVersion", versionText);
            SetValue(document, "AssemblyVersion", versionText);
            SetValue(document, "InformationalVersion", versionText);

            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true, 
                Indent = true
            };

            using var writer = XmlWriter.Create(inputFile, settings);
            document.Save(writer);

            return 0;
            
        } // method Main
        
    } // class Program
    
} // namespace PatchVersion
