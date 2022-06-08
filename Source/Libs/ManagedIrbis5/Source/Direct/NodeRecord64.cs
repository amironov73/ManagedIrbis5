// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* NodeRecord64.cs -- запись в файлах L01/N01
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

//
// Extract from official documentation:
// http://sntnarciss.ru/irbis/spravka/wtcp006005020.htm
//
// Файлы N01 и L01 содержат  в себе индексы словаря поисковых
// терминов и состоят из записей (блоков) постоянной длины.
// Записи состоят из трех частей: лидера, справочника
// и ключей переменной длины.
//
// Формат лидера записи
// Число бит Параметр
// 32        NUMBER – номер записи(начиная с 1;
//           в N01 файле номер первой записи равен
//           номеру корневой записи дерева);
// 32        PREV – номер предыдущей записи(если нет = -1);
// 32        NEXT – номер следующей записи(если нет = -1);
// 16        TERMS – число ключей в записи;
// 16        OFFSET_FREE – смещение на свободную позицию
//           в записи(от начала записи);
//
// Формат справочника
//
// На заметку: Справочник это таблица, определяющая поисковый термин.
//
// Каждый ключ переменной длины, который есть в записи,
// представлен в справочнике одним входом следующего формата:
// Число бит Параметр
// 16        LEN – длина ключа;
// 16        OFFSET_KEY – смещение на ключ(от начала записи);
// 32        LOW –
//             В N01 файле:
//             ссылка на запись файла N01(если LOW > 0) или файла L01
//             (если LOW < 0), у которых 1-й ключ равен данному.
//             Положительное значение LOW определяет ветку индекса
//             иерархически более низкого уровня.Самый низкий уровень
//             индекса (LOW < 0) соответствует ссылкам на записи
//             (листья) файла L01;
//             В L01 файле:
//             младшее слово 8 байтового смещения на ссылочную
//             запись в IFP;
// 32          HIGH –
//             В N01 файле:
//             всегда 0;
//             В L01 файле:
//             старшее слово 8 байтового смещения на ссылочную запись в IFP
//
// Ключи переменной длины
// записываются начиная с конца записи, так что порядок входов,
// соответствующих им, определяется алфавитным порядком ключей
//
// Сами ключи располагаются вплотную друг к другу без разделителей
// в порядке поступления на запись.
//
// Длина справочника 12*TERMS.
// Длина ключей = Размер записи – OFSET_FREE.
// Размер свободного места в записи = 16 + 12 * TERMS - длина ключей.
// Размер записи зависит от реализации и может быть равен в байтах:
// 512 ; 1024 ; 2048 ; 4096.
//

/// <summary>
/// Запись в файлах L01 и N01.
/// </summary>
[DebuggerDisplay ("Leader={" + nameof (Leader) + "}")]
public sealed class NodeRecord64
{
    #region Constants

    /// <summary>
    /// Длина записи в текущей реализации.
    /// </summary>
    public const int RecordSize = 2048;

    #endregion

    #region Properties

    /// <summary>
    /// Лист?
    /// </summary>
    public bool IsLeaf { get; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public NodeLeader64 Leader { get; set; }

    /// <summary>
    /// Ссылки.
    /// </summary>
    public List<NodeItem64> Items { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NodeRecord64()
    {
        Leader = new NodeLeader64();
        Items = new List<NodeItem64>();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NodeRecord64
        (
            bool isLeaf
        )
        : this()
    {
        IsLeaf = isLeaf;
    }

    #endregion

    #region Private members

    internal Stream? _stream;

    #endregion

    #region Public methods

    /// <summary>
    /// Дамп записи.
    /// </summary>
    public void Dump
        (
            TextWriter writer
        )
    {
        writer.WriteLine (ToString());
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        // TODO реализовать по-человечески

        var estimatedSize = 0;
        foreach (var item in Items)
        {
            estimatedSize += Environment.NewLine.Length;
            estimatedSize += item.Length;
        }

        var items = new ValueStringBuilder (stackalloc char[estimatedSize]);
        foreach (var item in Items)
        {
            items.Append (item.ToString());
            items.AppendLine();
        }

        return $"{Leader}{Environment.NewLine}{items.ToString()}";
    }

    #endregion
}
