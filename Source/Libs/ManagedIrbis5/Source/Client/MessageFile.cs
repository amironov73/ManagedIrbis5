// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MessageFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    //
    // Example
    //
    // (empty line)
    // Ассоциация ЭБНИТ
    // Система автоматизации библиотек
    // ИРБИС64
    // Copyright© 2006-2007
    // База данных:
    // Список разделов Электронного каталога или тематических БД
    // Вид поиска:
    // Список библиографических элементов, доступных для поиска
    // Словарь
    // Предыдущий
    // Прокрутка словаря к началу алфавита
    // Словарь - упорядоченный список терминов, соответствующих виду поиска
    // Следующий
    // Прокрутка словаря к концу алфавита
    // Ключ:
    // Установка начальной точки просмотра словаря
    // Отбор
    // Отбор термина из словаря для текущего запроса
    // Ссылка от:
    // Заголовок Рубрикатора
    // Тематический рубрикатор(ГРНТИ)
    // Раскрытие/Закрытие тематических рубрик
    // Переход к рубрикам, по ссылкам/отсылкам "Смотри..." и "Смотри также..."
    // Выделение фрагмента Тематического рубрикатора
    // Навигация
    // Таблица
    // Переключение формы представления Рубрикатора: Дерево/Таблица
    // Свободный поиск
    // Поиск с использованием базового языка запросов CDS/ISIS
    // Текущий запрос
    //

    /// <summary>
    ///
    /// </summary>
    public sealed class MessageFile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Default name of the file.
        /// </summary>
        public const string DefaultName = "irbismsg.txt";

        #endregion

        #region Properties

        /// <summary>
        /// Count of lines.
        /// </summary>
        public int LineCount => _list.Count;

        /// <summary>
        /// Name of the file.
        /// </summary>
        public string? Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageFile()
        {
            _list = new List<string>();
        }

        #endregion

        #region Private members

        private readonly List<string> _list;

        #endregion

        #region Public methods

        /// <summary>
        /// Get message by index.
        /// </summary>
        public string GetMessage
            (
                int index
            )
        {
            if (index < 0 || index >= _list.Count)
            {
                Magna.Error
                    (
                        "MessageFile::GetMessage: "
                        + "missing index="
                        + index
                    );

                return string.Format
                    (
                        "MISSING @" + index
                    );
            }

            return _list[index];
        }

        /// <summary>
        /// Read local file.
        /// </summary>
        public static MessageFile ReadLocalFile
            (
                string fileName,
                Encoding encoding
            )
        {
            var result = new MessageFile
            {
                Name = fileName
            };

            var lines = File.ReadAllLines(fileName, encoding);
            result._list.AddRange(lines);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            _list.Clear();
            Name = reader.ReadNullableString();
            _list.AddRange (reader.ReadStringArray());
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.WriteArray(_list.ToArray());
        }

        #endregion
    }
}
