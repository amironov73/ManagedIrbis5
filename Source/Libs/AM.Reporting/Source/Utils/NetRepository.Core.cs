// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

#endregion

#nullable enable

// From SyntaxParsers/NetRepository.cs
namespace Editor.Syntax.Parsers.ReflectionRepository
{
    public class DescriptionHelper
    {
        private static Hashtable assemblies = new Hashtable();
        private static string systemAssemblyFolder = string.Empty;
        private static Regex regex = new Regex (@"((\r\n)|(\n)|(\r))(\s)+", RegexOptions.Multiline);
        private static bool enabled = true;

        private static string GetInnerText (XmlNode node)
        {
            var result = string.Empty;
            if (node is XmlText)
            {
                result = node.InnerText;
                var p = result.IndexOf (":");
                if (p >= 0)
                {
                    result = result.Substring (p + 1);
                }

                p = result.IndexOf (".#ctor");
                if (p >= 0)
                {
                    result = result.Substring (0, p);
                }
            }
            else
            {
                if (node.Attributes != null)
                {
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        result = result + GetInnerText (attr);
                    }
                }

                // TZ: param name and description was displayed in one word
                if (node.Name == "param" && !string.IsNullOrEmpty (result))
                {
                    result = "<b>" + result + "</b>:\t";
                }

                //
                if (node.HasChildNodes)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        result = result + GetInnerText (child);
                    }
                }
            }

            return result;
        }

        private static XmlNode GetNodeByName (XmlNode node, string nodeName)
        {
            if (node != null)
            {
                var result = node.Name.Equals (nodeName) ? node : null;
                if (result != null)
                {
                    return result;
                }

                if (node.HasChildNodes)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        result = GetNodeByName (child, nodeName);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        private static string GetNodeName (XmlNode node)
        {
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (attr.Name == "name")
                    {
                        return attr.InnerText;
                    }
                }
            }

            return string.Empty;
        }

        private static string GetNodeSummary (XmlNode node)
        {
            var result = GetNodeByName (node, "summary");
            return (result != null) ? GetInnerText (result) : string.Empty;
        }

        private static void LoadParameters (string name, XmlNode node, Hashtable descriptions)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name.Equals ("param"))
                    {
                        var attr = child.Attributes["name"];
                        if (attr != null)
                        {
                            descriptions[name + ".param:" + attr.Value] = GetInnerText (child);
                        }
                    }
                }
            }
        }

        private static void LoadXmlFile (string fileName, Hashtable descriptions)
        {
            var doc = new XmlDocument();
            doc.Load (fileName);
            var root = GetNodeByName (doc, "members");
            if (root != null)
            {
                var node = root.FirstChild;
                while (node != null)
                {
                    var name = GetNodeName (node);
                    if (name.IndexOf (".Clear") >= 0)
                    {
                        descriptions[name] = GetNodeSummary (node);
                    }
                    else
                    {
                        descriptions[name] = GetNodeSummary (node);
                    }

                    if (node.HasChildNodes)
                    {
                        LoadParameters (name, node, descriptions);
                    }

                    node = node.NextSibling;
                }
            }
        }

        internal static object LoadAssembly (Assembly assembly)
        {
            var result = assemblies[assembly];
            if (result == null)
            {
                result = new Hashtable();
                if ((assembly.Location != null) && (assembly.Location != string.Empty))
                {
                    try
                    {
                        var fileInfo = new FileInfo (Path.ChangeExtension (assembly.Location, "xml"));
                        if (!fileInfo.Exists && SystemAssemblyFolder != string.Empty)
                        {
                            fileInfo = new FileInfo (SystemAssemblyFolder +
                                                     Path.ChangeExtension (Path.GetFileName (assembly.Location),
                                                         "xml"));
                        }

                        if (fileInfo.Exists)
                        {
                            LoadXmlFile (fileInfo.FullName, (Hashtable)result);
                        }
                    }
                    catch
                    {
                        //
                    }
                }
            }

            assemblies[assembly] = result;
            return result;
        }

        internal protected static object GetDescriptions (Assembly assembly)
        {
            var result = assemblies[assembly];
            if (result == null)
            {
                result = LoadAssembly (assembly);
            }

            return result;
        }

        protected static string GetDescription (string prefix, Type type, string postfix, string paramName)
        {
            if (type.Assembly == null)
            {
                return null;
            }

            // TZ: search descriptions in base members
            do
            {
                var descriptions = (Hashtable)GetDescriptions (type.Assembly);
                var result =
                    descriptions[
                        prefix + ":" + type.FullName + ((postfix != string.Empty) ? "." + postfix : string.Empty) +
                        paramName];
                if (result != null && !string.IsNullOrEmpty (result.ToString()))
                {
                    return regex.Replace (result.ToString().Trim(), " ");
                }

                type = type.BaseType;
            } while (type != null);

            return string.Empty;

            // TZ
        }

        protected static string GetDescription (MemberInfo info, string paramName)
        {
            if (info.Name == null)
            {
                return string.Empty;
            }

            var prefix = string.Empty;
            var postfix = info.Name.Replace (".ctor", "#ctor");
            switch (info.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                {
                    prefix = "M";
                    break;
                }
                case MemberTypes.Field:
                {
                    prefix = "F";
                    break;
                }
                case MemberTypes.Event:
                {
                    prefix = "E";
                    break;
                }
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                {
                    prefix = "T";
                    postfix = string.Empty;
                    break;
                }
                case MemberTypes.Property:
                {
                    prefix = "P";
                    break;
                }
                default:
                {
                    return string.Empty;
                }
            }

            if (info is MethodInfo methodInfo)
            {
                ParameterInfo[] paramsInfo = methodInfo.GetParameters();
                if (paramsInfo.Length > 0)
                {
                    var pars = string.Empty;
                    for (var i = 0; i < paramsInfo.Length; i++)
                    {
                        var pInfo = paramsInfo[i];
                        var s = pInfo.ParameterType.ToString().Replace ('&', '@');
                        pars = pars == string.Empty ? s : pars + "," + s;
                    }

                    postfix += "(" + pars + ")";
                }
            }

            object result = null;

            var type = (info is Type type1) ? type1 : info.ReflectedType;

            if (type != null)
            {
                result = GetDescription (prefix, type, postfix, paramName);
            }

            if ((result == null) && (info.DeclaringType != null))
            {
                result = GetDescription (prefix, info.DeclaringType, postfix, paramName);
            }

            if (result == null)
            {
                while (type != null)
                {
                    type = type.BaseType;
                    if (type != null)
                    {
                        result = GetDescription (prefix, info.DeclaringType, postfix, paramName);
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
            }

            return (result != null) ? result.ToString() : string.Empty;
        }

        public static string GetDescription (ParameterInfo pinfo)
        {
            return (enabled && pinfo.Member != null)
                ? GetDescription (pinfo.Member, ".param:" + pinfo.Name)
                : string.Empty;
        }

        public static string GetDescription (MemberInfo info)
        {
            return enabled ? GetDescription (info, string.Empty) : string.Empty;
        }

        public static string SystemAssemblyFolder
        {
            get
            {
                if (systemAssemblyFolder == string.Empty)
                {
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var name = assembly.GetName();
                        if ((name != null) && string.Compare (name.Name, "mscorlib", true) == 0)
                        {
                            systemAssemblyFolder = string.Format ("{0}\\", Path.GetDirectoryName (assembly.Location));
                        }
                    }
                }

                return systemAssemblyFolder;
            }
        }
    }
}
