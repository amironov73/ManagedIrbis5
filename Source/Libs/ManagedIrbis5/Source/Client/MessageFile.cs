// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MessageFile.cs -- файл с сообщениями, выдаваемыми клиентами.
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

namespace ManagedIrbis.Client;

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
/// Файл с сообщениями, выдаваемыми клиентами.
/// </summary>
public sealed class MessageFile
    : IHandmadeSerializable
{
    #region Constants

    /// <summary>
    /// Имя файла с сообщению по умолчанию.
    /// </summary>
    public const string DefaultName = "irbismsg.txt";

    #endregion

    #region Properties

    /// <summary>
    /// Общее количество сообщений.
    /// </summary>
    public int LineCount => _list.Count;

    /// <summary>
    /// Имя файла, из которого считаны сообщения.
    /// </summary>
    public string? Name { get; set; }

    #endregion

    #region Private members

    // список сообщений
    private readonly List<string> _list = new();

    #endregion

    #region Public methods

    /// <summary>
    /// Получение текста сообщения по его индексу.
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
                    nameof (MessageFile) + "::" + nameof (GetMessage)
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
    /// Чтение списка сообщений из локальной файловой системы.
    /// </summary>
    public static MessageFile ReadLocalFile
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (encoding);

        var result = new MessageFile
        {
            Name = fileName
        };

        var lines = Unix.ReadAllLines (fileName, encoding);
        result._list.AddRange (lines);

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
        Sure.NotNull (reader);

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
        Sure.NotNull (writer);

        writer.WriteNullable (Name);
        writer.WriteArray (_list.ToArray());
    }

    #endregion
}
