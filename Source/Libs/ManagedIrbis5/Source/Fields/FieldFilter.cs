// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable LoopCanBeConvertedToQuery

/* FieldFilter.cs -- динамическая фильтрация полей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Динамическая фильтрация полей записи.
/// </summary>
public sealed class FieldFilter
{
    #region Properties

    /// <summary>
    /// Провайдер.
    /// </summary>
    public ISyncProvider Provider { get; }

    /// <summary>
    /// Форматтер.
    /// </summary>
    public PftFormatter Formatter { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FieldFilter
        (
            ISyncProvider provider,
            string format
        )
    {
        Sure.NotNull (provider);
        Sure.NotNullNorEmpty (format);

        Provider = provider;

        Formatter = new PftFormatter();
        Formatter.SetProvider (provider);
        SetProgram (format);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Все ли поля среди перечисленных удовлетворяют условию?
    /// </summary>
    public bool AllFields
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull (fields);

        var result = false;
        foreach (var field in fields)
        {
            result = CheckField (field);
            if (!result)
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Хотя бы одно поле среди перечисленных удовлетворяет условию?
    /// </summary>
    public bool AnyField
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull (fields);

        var result = false;
        foreach (var field in fields)
        {
            result = CheckField (field);
            if (result)
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Проверка, удовлетворяет ли данное поле заданному условию.
    /// </summary>
    public bool CheckField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var record = new Record();
        var copy = field.Clone();
        record.Fields.Add (copy);

        Formatter.Context.AlternativeRecord = field.Record;
        var text = Formatter.FormatRecord (record);
        var result = text.SameString ("1");

        return result;
    }

    /// <summary>
    /// Фильтрация полей согласно заданному условию.
    /// </summary>
    public Field[] FilterFields
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull (fields);

        var result = new List<Field>();
        foreach (var field in fields)
        {
            if (CheckField (field))
            {
                result.Add (field);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Отбор записей, содержащих поля, удовлетворяющих заданному условию.
    /// </summary>
    public IEnumerable<Record> FilterRecords
        (
            IEnumerable<Record> records
        )
    {
        Sure.NotNull (records);

        foreach (var record in records)
        {
            if (AnyField (record.Fields))
            {
                yield return record;
            }
        }
    }

    /// <summary>
    /// Первое из полей, удовлетворяющих заданному условию.
    /// </summary>
    public Field? First
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull (fields);

        Field? result = null;
        foreach (var field in fields)
        {
            if (CheckField (field))
            {
                result = field;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Последнее из полей, удовлетворяющих заданному условию.
    /// </summary>
    public Field? Last
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull (fields);

        Field? result = null;
        foreach (var field in fields)
        {
            if (CheckField (field))
            {
                result = field;
            }
        }

        return result;
    }

    /// <summary>
    /// Установка условия в виде программы-формата.
    /// </summary>
    public void SetProgram
        (
            string format
        )
    {
        Sure.NotNull (format);

        var text = format.StartsWith ("if")
            ? format
            : $"if {format} then '1' else '0' fi";
        Formatter.ParseProgram (text);
    }

    #endregion
}
