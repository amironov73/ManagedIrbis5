// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* MenuUtility.cs -- методы для работы с ИРБИС-меню
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using AM;
using AM.Collections;

using ManagedIrbis.Trees;
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

        /// <summary>
        /// Построение строчки для TRE-файла.
        /// </summary>
        private static TreeLine _BuildLine
            (
                MenuEntry parent,
                MenuFile menu
            )
        {
            var value = $"{parent.Code}{TreeLine.Delimiter}{parent.Comment}";
            var result = new TreeLine(value);
            var children = menu.Entries
                .Where(v => ReferenceEquals(v.OtherEntry, parent))
                .OrderBy(v => v.Code)
                .ToArray();

            foreach (var child in children)
            {
                var subItem = _BuildLine(child, menu);
                result.Children.Add(subItem);
            }

            return result;
        } // method _BuildLine

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление в конец меню пары "код - значение".
        /// </summary>
        public static MenuFile Add<T>
            (
                this MenuFile menu,
                string code,
                T? value
            )
        {
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
        } // method Add

        /// <summary>
        /// Собирает значения для указанного кода.
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
        } // method CollectStrings

        /// <summary>
        /// Получает типизированное значение для указанного кода
        /// (без учета регистра символов).
        /// Если строк с таким кодом несколько, используется
        /// первая найденная.
        /// </summary>
        public static T? GetValue<T>
            (
                this MenuFile menu,
                string code,
                T? defaultValue = default
            )
        {
            var found = menu.FindEntry(code);

            return found is null
                ? defaultValue
                : Utility.ConvertTo<T>(found.Comment);
        } // method GetValue

        /// <summary>
        /// Получает типизированное значение для указанного кода
        /// (с учетом регистра символов).
        /// Если строк с таким кодом несколько, используется
        /// первая найденная.
        /// </summary>
        public static T? GetValueSensitive<T>
            (
                this MenuFile menu,
                string code,
                T? defaultValue = default
            )
        {
            var found = menu.FindEntrySensitive(code);

            return found is null
                ? defaultValue
                : Utility.ConvertTo<T>(found.Comment);
        } // method GetValueSensitive

        /// <summary>
        /// Converts the menu to JSON.
        /// </summary>
        public static string ToJson
            (
                this MenuFile menu
            )
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var result = JsonSerializer.Serialize(menu.Entries, options);

            return result;
        } // method ToJson

        /// <summary>
        /// Восстанавливает меню из JSON-текста.
        /// </summary>
        public static MenuFile FromJson
            (
                string text
            )
        {
            var entries = JsonSerializer
                .Deserialize<NonNullCollection<MenuEntry>>(text)
                .ThrowIfNull();
            var result = new MenuFile(entries);

            return result;
        } // method FromJson

        /// <summary>
        /// Saves the menu to local JSON file.
        /// </summary>
        public static void SaveLocalJsonFile
            (
                this MenuFile menu,
                string fileName
            )
        {
            var contents = JsonSerializer.Serialize(menu.Entries);

            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );
        } // method SaveLocalJsonFile

        /// <summary>
        /// Parses the local json file.
        /// </summary>
        public static MenuFile ParseLocalJsonFile
            (
                string fileName
            )
        {
            var text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            var result = FromJson(text);

            return result;
        } // method ParseLocalJsonFile

        /// <summary>
        /// Преобразует меню в дерево (TRE-файл).
        /// </summary>
        public static TreeFile ToTree
            (
                this MenuFile menu
            )
        {
            //
            // Здесь мы используем свойство OtherEntry
            // как указатель на вышестоящий элемент.
            // У рутовых элементов он равен null.
            //

            foreach (var entry in menu.Entries)
            {
                entry.Code = entry.Code.ThrowIfNull("entryCode").Trim();
            }

            foreach (var first in menu.Entries)
            {
                if (first is not null)
                {
                    var firstCode = first.Code;
                    if (string.IsNullOrEmpty(firstCode))
                    {
                        continue;
                    }

                    foreach (var second in menu.Entries)
                    {
                        if (second is not null)
                        {
                            var secondCode = second.Code;
                            if (string.IsNullOrEmpty(secondCode))
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
                                if (otherEntry is null)
                                {
                                    first.OtherEntry = second;
                                }
                                else
                                {
                                    var otherCode = otherEntry.Code;
                                    if (!string.IsNullOrEmpty(otherCode))
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
                }
            }

            var roots = menu.Entries
                .Where(entry => ReferenceEquals(entry.OtherEntry, null))
                .OrderBy(entry => entry.Code)
                .ToArray();

            var result = new TreeFile();
            foreach (var root in roots)
            {
                var item = _BuildLine(root, menu);
                result.Roots.Add(item);
            }

            return result;
        } // method ToTree

        /// <summary>
        /// Сериализация меню в XML.
        /// </summary>
        public static string ToXml
            (
                this MenuFile menu
            )
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NewLineHandling = NewLineHandling.None
            };
            var output = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using var writer = XmlWriter.Create(output, settings);
            var serializer = new XmlSerializer(typeof(MenuFile));
            serializer.Serialize(writer, menu, namespaces);

            return output.ToString();
        } // method ToXml

        /// <summary>
        /// Чтение MNU-файла с ИРБИС-сервера в синхронном режиме.
        /// </summary>
        /// <param name="connection">Подключение к ИРБИС-серверу.</param>
        /// <param name="specification">Спецификация файла.</param>
        /// <returns>Прочитанное меню либо <c>null</c></returns>
        public static MenuFile? ReadMenu
            (
                this ISyncProvider connection,
                FileSpecification specification
            )
        {
            var content = connection.ReadTextFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            using var reader = new StringReader(content);
            var result = MenuFile.ParseStream(reader);

            return result;
        } // method ReadMenu

        /// <summary>
        /// Чтение MNU-файла с ИРБИС-сервера в асинхронном режиме.
        /// </summary>
        /// <param name="connection">Подключение к ИРБИС-серверу.</param>
        /// <param name="specification">Спецификация файла.</param>
        /// <returns>Прочитанное меню либо <c>null</c></returns>
        public static async Task<MenuFile?> ReadMenuAsync
            (
                this IAsyncProvider connection,
                FileSpecification specification
            )
        {
            var content = await connection.ReadTextFileAsync(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            using var reader = new StringReader(content);
            var result = MenuFile.ParseStream(reader);

            return result;
        } // method ReadMenuAsync

        /// <summary>
        /// Чтение меню с ИРБИС-сервера в синхронном режиме.
        /// </summary>
        /// <param name="connection">Подключение к ИРБИС-серверу.</param>
        /// <param name="specification">Спецификация файла.</param>
        /// <returns>Прочитанное меню (либо выбрасывает исключение).</returns>
        public static MenuFile RequireMenu
            (
                this ISyncProvider connection,
                FileSpecification specification
            )
            => connection.ReadMenu(specification).ThrowIfNull("RequireMenu");

        /// <summary>
        /// Чтение меню с ИРБИС-сервера в асинхронном режиме.
        /// </summary>
        /// <param name="connection">Подключение к ИРБИС-серверу.</param>
        /// <param name="specification">Спецификация файла.</param>
        /// <returns>Прочитанное меню (либо выбрасывает исключение).</returns>
        public static async Task<MenuFile> RequireMenuAsync
            (
                this IAsyncProvider connection,
                FileSpecification specification
            )
        {
            var result = await connection.ReadMenuAsync(specification);
            return result.ThrowIfNull("RequireMenuAsync");
        } // method RequireMenuAsync

        #endregion

    } // class MenuUtility

} // namespace ManagedIrbis.Menus
