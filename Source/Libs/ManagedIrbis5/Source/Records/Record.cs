// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedParameter.Local

/* Record.cs -- библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Direct;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

using static ManagedIrbis.RecordStatus;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Библиографическая запись. Состоит из произвольного количества полей.
/// </summary>
[DebuggerDisplay ("[{" + nameof (Database) +
    "}] MFN={" + nameof (Mfn) + "} ({" + nameof (Version) + "})")]
public sealed class Record
    : IRecord,
    IEnumerable<Field>,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Запись удалена любым способом (логически или физически).
    /// </summary>
    internal const RecordStatus IsDeleted = LogicallyDeleted | PhysicallyDeleted;

    #endregion

    #region Properties

    /// <summary>
    /// База данных, в которой хранится запись.
    /// Для вновь созданных записей -- <c>null</c>.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// MFN (порядковый номер в базе данных) записи.
    /// Для вновь созданных записей равен <c>0</c>.
    /// Для хранящихся в базе записей нумерация начинается
    /// с <c>1</c>.
    /// </summary>
    public int Mfn { get; set; }

    /// <summary>
    /// Версия записи. Для вновь созданных записей равна <c>0</c>.
    /// Для хранящихся в базе записей нумерация версий начинается
    /// с <c>1</c>.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Статус записи. Для вновь созданных записей <c>None</c>.
    /// </summary>
    public RecordStatus Status { get; set; }

    /// <summary>
    /// Признак -- запись помечена как логически удаленная.
    /// </summary>
    public bool Deleted => (Status & IsDeleted) != 0;

    /// <summary>
    /// Список полей.
    /// </summary>
    public FieldCollection Fields { get; }

    /// <summary>
    /// Описание в произвольной форме (опциональное).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Признак того, что запись модифицирована.
    /// </summary>
    public bool Modified { get; internal set; }

    /// <summary>
    /// Индекс документа (поле 920).
    /// </summary>
    public string? Index { get; set; }

    /// <summary>
    /// Ключ для сортировки записей.
    /// </summary>
    public string? SortKey { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// Данное свойство используется, например,
    /// при построении отчета.
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public object? UserData { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Record()
    {
        Fields = new FieldCollection() { Record = this };
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление поля в конец списка полей.
    /// </summary>
    /// <param name="field">Добавляемое поле.</param>
    /// <returns>this</returns>
    public Record Add
        (
            Field field
        )
    {
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Установка статуса записи.
    /// </summary>
    /// <param name="status">Новый статус записи.</param>
    /// <returns>this</returns>
    public Record Add
        (
            RecordStatus status
        )
    {
        Status = status;

        return this;
    }

    /// <summary>
    /// Добавление поля в конец записи.
    /// </summary>
    /// <returns>
    /// Свежедобавленное поле (для цепочечных вызовов).
    /// </returns>
    public Record Add
        (
            int tag,
            string? value = default
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag);
        field.DecodeBody (value);
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление поля в конец записи.
    /// </summary>
    /// <returns>
    /// this (для цепочечных вызовов).
    /// </returns>
    public Record Add
        (
            int tag,
            SubField subfield1
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag) { subfield1 };
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление полей в конец записи.
    /// </summary>
    /// <returns>
    /// this (для цепочечных вызовов).
    /// </returns>
    public Record Add
        (
            int tag,
            SubField subfield1,
            SubField subfield2
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag) { subfield1, subfield2 };
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление полей в конец записи.
    /// </summary>
    /// <returns>
    /// this (для цепочечных вызовов).
    /// </returns>
    public Record Add
        (
            int tag,
            SubField subfield1,
            SubField subfield2,
            SubField subfield3
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag) { subfield1, subfield2, subfield3 };
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление полей в конец записи.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="subfields">Подполя.</param>
    /// <returns>this (для цепочечных вызовов).</returns>
    public Record Add
        (
            int tag,
            params SubField[] subfields
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag);
        field.Subfields.AddRange (subfields);
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление поля в конец записи.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя (опционально).</param>
    /// <returns>this</returns>
    public Record Add
        (
            int tag,
            char code,
            string? value = default
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag);
        field.Subfields.Add (new SubField (code, value));
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление поля в конец записи.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code1">Код подполя.</param>
    /// <param name="value1">Значение подполя.</param>
    /// <param name="code2">Код подполя.</param>
    /// <param name="value2">Значение подполя (опционально).</param>
    /// <returns>this</returns>
    public Record Add
        (
            int tag,
            char code1,
            string? value1,
            char code2,
            string? value2 = default
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag);
        field.Subfields.Add (new SubField (code1, value1));
        field.Subfields.Add (new SubField (code2, value2));
        Fields.Add (field);

        return this;
    }


    /// <summary>
    /// Добавление поля в конец записи.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code1">Код подполя.</param>
    /// <param name="value1">Значение подполя.</param>
    /// <param name="code2">Код подполя.</param>
    /// <param name="value2">Значение подполя.</param>
    /// <param name="code3">Код подполя.</param>
    /// <param name="value3">Значение подполя (опционально).</param>
    /// <returns>this</returns>
    public Record Add
        (
            int tag,
            char code1,
            string? value1,
            char code2,
            string? value2,
            char code3,
            string? value3 = default
        )
    {
        Sure.Positive (tag);

        var field = new Field (tag);
        field.Subfields.Add (new SubField (code1, value1));
        field.Subfields.Add (new SubField (code2, value2));
        field.Subfields.Add (new SubField (code3, value3));
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление поля в конец записи.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="subfields">Коды и значения подполей.</param>
    /// <returns>this (для цепочечных вызовов)</returns>
    public Record Add
        (
            int tag,
            string[] subfields
        )
    {
        Sure.Positive (tag);

        var field = Field.WithSubFields (tag, subfields);
        Fields.Add (field);

        return this;
    }

    /// <summary>
    /// Добавление в запись непустого поля.
    /// </summary>
    public Record AddNonEmptyField
        (
            int tag,
            string? value
        )
    {
        Sure.Positive (tag);

        if (!string.IsNullOrEmpty (value))
        {
            var field = new Field { Tag = tag };
            field.DecodeBody (value);
            Fields.Add (field);
        }

        return this;
    }

    /// <summary>
    /// Очистка записи (удаление всех полей).
    /// </summary>
    /// <returns>
    /// Ту же самую, но очищенную запись.
    /// </returns>
    public Record Clear()
    {
        Fields.Clear();

        return this;
    }

    /// <summary>
    /// Снятие признака "запись модифицирована".
    /// </summary>
    public void NotModified()
    {
        Modified = false;
    }

    /// <summary>
    /// Создание глубокой копии записи.
    /// </summary>
    public Record Clone()
    {
        var result = (Record)MemberwiseClone();

        for (var i = 0; i < result.Fields.Count; i++)
        {
            result.Fields[i] = result.Fields[i].Clone();
        }

        return result;
    }

    /// <summary>
    /// Вычисление числа повторений поля с указанной меткой.
    /// </summary>
    public int Count
        (
            int tag
        )
    {
        var result = 0;
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                ++result;
            }
        }

        return result;
    }

    /// <summary>
    /// Вычисление числа повторений поля с указанными меткой и кодом подполя.
    /// </summary>
    public int Count
        (
            int tag,
            char code
        )
    {
        var result = 0;
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                if (field.HaveSubField (code))
                {
                    ++result;
                }
            }
        }

        return result;
    }

    /// <inheritdoc cref="IRecord.Decode(Response)"/>
    public void Decode
        (
            Response response
        )
    {
        Sure.NotNull (response);

        try
        {
            var line = response.ReadUtf();

            var first = line.Split ('#');
            Sure.AssertState (first.Length is 1 or 2);
            Mfn = int.Parse (first[0]);
            Status = first.Length == 1
                ? None
                : (RecordStatus)first[1].SafeToInt32();

            line = response.ReadUtf();
            var second = line.Split ('#');
            Sure.AssertState (second.Length is 1 or 2);
            Version = second.Length == 1
                ? 0
                : int.Parse (second[1]);

            while (!response.EOT)
            {
                line = response.ReadUtf();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var field = new Field();
                field.Decode (line);
                Fields.Add (field);
            }
        }
        catch (Exception exception)
        {
            // response.DebugUtf(Console.Error);
            Magna.Logger.LogError
                (
                    exception,
                    nameof (Record) + "::" + nameof (Decode)
                );

            throw new IrbisException
                (
                    nameof (Record) + "::" + nameof (Decode),
                    exception
                );
        }
    }

    /// <inheritdoc cref="IRecord.Decode(MstRecord64)"/>
    public void Decode
        (
            MstRecord64 record
        )
    {
        Sure.NotNull (record);

        Mfn = record.Leader.Mfn;
        Status = (RecordStatus)record.Leader.Status;
        Version = record.Leader.Version;

        // result.Fields.BeginUpdate();
        // result.Fields.EnsureCapacity(Dictionary.Count);

        foreach (var entry in record.Dictionary)
        {
            var field = record.DecodeField (entry);
            Fields.Add (field);
        }

        // result.Fields.EndUpdate();
    }

    /// <summary>
    /// Декодирование ответа сервера.
    /// </summary>
    public void Decode
        (
            IList<string> lines
        )
    {
        Sure.NotNull (lines);

        try
        {
            var line = lines[0];

            var first = line.Split ('#');
            Mfn = int.Parse (first[0]);
            Status = first.Length == 1
                ? None
                : (RecordStatus)first[1].SafeToInt32();

            line = lines[1];
            var second = line.Split ('#');
            Version = second.Length == 1
                ? 0
                : int.Parse (second[1]);

            for (var i = 2; i < lines.Count; i++)
            {
                line = lines[i];
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var field = new Field();
                field.Decode (line);
                Fields.Add (field);
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (Record) + "::" + nameof (Decode)
                );

            Console.Error.WriteLine
                (
                    string.Join (Environment.NewLine, lines)
                );

            throw new IrbisException
                (
                    nameof (Record) + "::" + nameof (Decode),
                    exception
                );
        }
    }

    /// <inheritdoc cref="IRecord.Encode(string)"/>
    public string Encode
        (
            string? delimiter = IrbisText.IrbisDelimiter
        )
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (512);

        builder.Append (Mfn.ToInvariantString())
            .Append ('#')
            .Append (((int)Status).ToInvariantString())
            .Append (delimiter)
            .Append ("0#")
            .Append (Version.ToInvariantString())
            .Append (delimiter);

        foreach (var field in Fields)
        {
            builder.Append (field).Append (delimiter);
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <inheritdoc cref="IRecord.Encode(MstRecord64)"/>
    public void Encode
        (
            MstRecord64 record
        )
    {
        Sure.NotNull (record);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Получение текста поля до разделителей подполей
    /// первого повторения поля с указанной меткой.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <returns>Значение поля или <c>null</c>.</returns>
    public string? FM (int tag)
    {
        return GetField (tag)?.Value;
    }

    /// <summary>
    /// Получение текста поля до разделителей.
    /// </summary>
    public string? FM (int tag, int occurrence)
    {
        return GetField (tag, occurrence)?.Value;
    }

    /// <summary>
    /// Текст первого подполя с указанным тегом и кодом.
    /// </summary>
    public string? FM
        (
            int tag,
            char code
        )
    {
        Sure.Positive (tag);

        var field = GetField (tag);

        if (field is not null)
        {
            return code == '*'
                ? field.GetValueOrFirstSubField()
                : field.GetSubFieldValue (code);
        }

        return default;
    }

    /// <summary>
    /// Текст заданного подполя с указанным тегом и кодом.
    /// </summary>
    public string? FM
        (
            int tag,
            int occurrence,
            char code
        )
    {
        var index = 0;
        while (true)
        {
            var field = GetField (tag, index);
            if (field is null)
            {
                break;
            }

            var value = code == '*'
                ? field.GetValueOrFirstSubField()
                : field.GetSubFieldValue (code);

            if (!string.IsNullOrEmpty (value))
            {
                if (occurrence == 0)
                {
                    return value;
                }

                --occurrence;
            }

            ++index;
        }

        return null;
    }

    /// <summary>
    /// Текст всех полей с указанным тегом.
    /// </summary>
    public string[] FMA
        (
            int tag
        )
    {
        var result = new LocalList<string>();
        foreach (var field in Fields)
        {
            if (field.Tag == tag
                && !field.Value.IsEmpty())
            {
                result.Add (field.Value);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Текст всех подполей с указанным тегом и кодом.
    /// </summary>
    public string[] FMA
        (
            int tag,
            char code
        )
    {
        var result = new LocalList<string>();
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                // TODO: Value, если есть, всегда первое в списке подполей
                var value = code == '*'
                    ? field.GetValueOrFirstSubField()
                    : field.GetSubFieldValue (code);
                if (!value.IsEmpty())
                {
                    result.Add (value);
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Перечисление полей с указанной меткой.
    /// </summary>
    /// <param name="tag">Искомая метка поля.</param>
    public IEnumerable<Field> EnumerateField
        (
            int tag
        )
    {
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                yield return field;
            }
        }
    }

    /// <summary>
    /// Получение заданного повторения поля с указанной меткой.
    /// </summary>
    public Field? GetField
        (
            int tag,
            int occurrence = 0
        )
    {
        Sure.NonNegative (occurrence);

        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                if (occurrence == 0)
                {
                    return field;
                }

                --occurrence;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение поля с указанной меткой
    /// либо создание нового поля, если таковое отсутствует.
    /// </summary>
    /// <param name="tag">Искомая метка поля.</param>
    /// <returns>Найденное либо созданное поле.</returns>
    public Field GetOrAddField
        (
            int tag
        )
    {
        Sure.Positive (tag);

        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                return field;
            }
        }

        var result = new Field { Tag = tag };
        Fields.Add (result);

        return result;
    }

    /// <summary>
    /// Проверка, есть ли в записи поле с указанной меткой.
    /// </summary>
    public bool HaveField
        (
            int tag
        )
    {
        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Удаление из записи поля с указанной меткой.
    /// </summary>
    /// <param name="tag">Искомая метка.</param>
    /// <returns>this.</returns>
    public Record RemoveField
        (
            int tag
        )
    {
        Sure.Positive (tag);

        while (GetField (tag) is { } field)
        {
            Fields.Remove (field);
        }

        return this;
    }

    /// <summary>
    /// Формирует плоское текстовое представление записи.
    /// </summary>
    public string ToPlainText()
    {
        return PlainText.ToPlainText (this);
    }

    #endregion

    #region IEnumerable<Field> members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<Field> GetEnumerator()
    {
        return Fields.GetEnumerator();
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<Record> (this, throwOnError);

        // TODO: implement

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Encode ("\n");
    }

    #endregion
}
