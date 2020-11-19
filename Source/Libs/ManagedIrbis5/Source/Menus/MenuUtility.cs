// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MenuUtility.cs -- методы для работы с ИРБИС-меню
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Методы для работы с ИРБИС-меню.
    /// </summary>
    public static class MenuUtility
    {
        #region Private members

        private static TreeFile.Item _BuildItem
            (
                MenuEntry parent,
                MenuFile menu
            )
        {
            string value = string.Format
                (
                    "{0}{1}{2}",
                    parent.Code,
                    TreeFile.Item.Delimiter,
                    parent.Comment
                );
            var result = new TreeFile.Item(value);
            MenuEntry[] children = menu.Entries
                .Where(v => ReferenceEquals(v.OtherEntry, parent))
                .OrderBy(v => v.Code)
                .ToArray();

            foreach (MenuEntry child in children)
            {
                var subItem = _BuildItem(child, menu);
                result.Children.Add(subItem);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the typed value with specified code.
        /// </summary>
        public static MenuFile Add<T>
            (
                this MenuFile menu,
                string code,
                T? value
            )
        {
            Sure.NotNull(menu, nameof(menu));
            Sure.NotNull(code, nameof(code));

            if (ReferenceEquals(value, null))
            {
                menu.Add(code, string.Empty);
            }
            else
            {
                var textValue = Utility.ConvertTo<string>(value);
                menu.Add(code, textValue);
            }

            return menu;
        }

        /// <summary>
        /// Collects the comments for code.
        /// </summary>
        public static string?[] CollectStrings
            (
                this MenuFile menu,
                string code
            )
        {
            return menu.Entries
                .Where
                    (
                        entry => entry.Code.SameString(code)
                                 || MenuFile.TrimCode(entry.Code.ThrowIfNull("entry.Code"))
                                     .SameString(code)
                    )
                .Select(entry => entry.Comment)
                .ToArray();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public static T? GetValue<T>
            (
                this MenuFile menu,
                string code,
                T? defaultValue
            )
        {
            Sure.NotNull(menu, nameof(menu));
            Sure.NotNull(code, nameof(code));

            var found = menu.FindEntry(code);

            return ReferenceEquals(found, null)
                ? defaultValue
                : Utility.ConvertTo<T>(found.Comment);
        }

        /// <summary>
        /// Gets the value (case sensitive).
        /// </summary>
        public static T? GetValueSensitive<T>
            (
                this MenuFile menu,
                string code,
                T? defaultValue
            )
        {
            var found = menu.FindEntrySensitive(code);

            return ReferenceEquals(found, null)
                ? defaultValue
                : Utility.ConvertTo<T>(found.Comment);
        }

        /// <summary>
        /// Converts the menu to JSON.
        /// </summary>
        public static string ToJson
            (
                this MenuFile menu
            )
        {
            Sure.NotNull(menu, nameof(menu));

            var result = JsonSerializer.Serialize(menu.Entries);

            return result;
        }

        /// <summary>
        /// Restores the menu from JSON.
        /// </summary>
        public static MenuFile FromJson
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            var entries = JsonSerializer
                .Deserialize<NonNullCollection<MenuEntry>>(text)
                .ThrowIfNull();
            MenuFile result = new MenuFile(entries);

            return result;
        }

        /// <summary>
        /// Saves the menu to local JSON file.
        /// </summary>
        public static void SaveLocalJsonFile
            (
                this MenuFile menu,
                string fileName
            )
        {
            Sure.NotNull(menu, nameof(menu));
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var contents = JsonSerializer.Serialize(menu.Entries);

            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );
        }

        /// <summary>
        /// Parses the local json file.
        /// </summary>
        public static MenuFile ParseLocalJsonFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            MenuFile result = FromJson(text);

            return result;
        }

        /// <summary>
        /// Convert MNU to TRE.
        /// </summary>
        public static TreeFile ToTree
            (
                this MenuFile menu
            )
        {
            foreach (var entry in menu.Entries)
            {
                entry.Code = entry.Code.ThrowIfNull("entryCode").Trim();
            }

            foreach (var first in menu.Entries)
            {
                if (ReferenceEquals(first, null))
                {
                    continue;
                }
                var firstCode = first.Code;
                if (ReferenceEquals(firstCode, null))
                {
                    continue;
                }
                foreach (var second in menu.Entries)
                {
                    var secondCode = second?.Code;
                    if (ReferenceEquals(secondCode, null))
                    {
                        continue;
                    }
                    if (ReferenceEquals(first, second))
                    {
                        continue;
                    }
                    if (firstCode.SameString(secondCode))
                    {
                        continue;
                    }

                    if (firstCode.StartsWith(secondCode))
                    {
                        var otherEntry = first.OtherEntry;
                        if (ReferenceEquals(otherEntry, null))
                        {
                            first.OtherEntry = second;
                        }
                        else
                        {
                            var otherCode = otherEntry.Code;
                            if (!ReferenceEquals(otherCode, null))
                            {
                                if (secondCode.Length > otherCode.Length)
                                {
                                    first.OtherEntry = second;
                                }
                            }
                        }
                    }
                }
            }

            MenuEntry[] roots = menu.Entries
                .Where(entry => ReferenceEquals(entry.OtherEntry, null))
                .OrderBy(entry => entry.Code)
                .ToArray();

            var result = new TreeFile();
            foreach (MenuEntry root in roots)
            {
                var item = _BuildItem(root, menu);
                result.Roots.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Converts the menu to XML.
        /// </summary>
        public static string ToXml
            (
                this MenuFile menu
            )
        {
            Sure.NotNull(menu, nameof(menu));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NewLineHandling = NewLineHandling.None
            };
            StringBuilder output = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MenuFile));
                serializer.Serialize(writer, menu, namespaces);
            }

            return output.ToString();
        }

        #endregion
    }
}
