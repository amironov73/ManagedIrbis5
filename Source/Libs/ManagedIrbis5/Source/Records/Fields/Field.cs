// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Field.cs -- поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

#endregion

namespace ManagedIrbis;

/*

 Поле записи. Состоит из метки, значения до первого разделителя
 и произвольного количества подполей.

 Поле записи характеризуется числовой меткой в диапазоне
 от 1 до 2147483647 (на практике встречаются коды от 1 до 9999)
 и содержит значение до первого разделителя (опционально)
 и произвольное количество подполей (см. класс `SubField`).

 Стандартом MARC у полей предусмотрены также два односимвольных
 индикатора, но ИРБИС вслед за ISIS их не поддерживает.

 Кроме того, стандарт MARC предусматривает т. наз. "фиксированные"
 поля с метками от 1 до 9 включительно, которые не должны содержать
 ни индикаторов, ни подполей, но имеют строго фиксированную структуру.
 ИРБИС такие поля обрабатывает особым образом только в ситуации
 импорта/экспорта в формат ISO2709, в остальном же он их трактует
 точно так же, как и прочие поля (которые стандарт называет
 полями переменной длины).

 Стандартом MARC предусмотрены метки в диапазоне от 1 до 999,
 все прочие являются самодеятельностью ИРБИС. Поля с нестандартными
 метками не могут быть выгружены в формат ISO2709.

 Хотя технически поле может содержать одновременно и значение
 до первого разделителя, и подполя, но стандартом такая ситуация
 не предусмотрена, на практике она означает сбой. В стандарте
 MARC поле содержит либо значение либо подполя.

 Начиная с версии 2018, ИРБИС64 резервирует метку 2147483647
 для поля GUID - уникального идентификатора записи.

 Порядок подполей в поле важен, т. к. на этот порядок завязана
 обработка т. наз. "вложенных полей".

 Стандартом MARC предусмотрено, что внутри поля могут повторяться
 подполя с одинаковым кодом, однако, ИРБИС вслед за ISIS очень
 ограниченно поддерживает эту ситуацию (см. форматный выход `&umarci`).

 */

/// <summary>
/// Поле библиографической записи.
/// </summary>
[XmlRoot ("field")]
public class Field
    : IHandmadeSerializable,
    IReadOnly<Field>,
    IEnumerable<SubField>,
    IVerifiable,
    IResettable
{
    #region Constants

    /// <summary>
    /// Специальный код, зарезервированный для
    /// значения поля до первого разделителя.
    /// </summary>
    private const char ValueCode = '\0';

    /// <summary>
    /// Нет тега, т. е. тег ещё не присвоен.
    /// </summary>
    public const int NoTag = 0;

    /// <summary>
    /// Разделитель подполей.
    /// </summary>
    public const char Delimiter = '^';

    /// <summary>
    /// Количество индикаторов поля.
    /// </summary>
    public const int IndicatorCount = 2;

    #endregion

    #region Properties

    /// <summary>
    /// Метка поля.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    public int Tag { get; set; }

    /// <summary>
    /// Значение поля до первого разделителя.
    /// </summary>
    /// <remarks>
    /// Значение имитируется с помощью первого подполя,
    /// код которого должен быть равен '\0'.
    /// </remarks>
    public string? Value
    {
        get => GetValueSubField()?.Value;
        set
        {
            Clear();
            if (value.SafeContains (Delimiter))
            {
                DecodeBody (value);
            }
            else
            {
                if (!string.IsNullOrEmpty (value))
                {
                    CreateValueSubField().Value = value;
                }
            }
        }
    }

    /// <summary>
    /// Список подполей.
    /// </summary>
    [XmlArrayItem ("subfield")]
    [JsonPropertyName ("subfields")]
    public SubFieldCollection Subfields { get; }

    /// <summary>
    /// Номер повторения поля.
    /// </summary>
    /// <remarks>
    /// Формируется автоматически.
    /// </remarks>
    [XmlIgnore]
    [JsonIgnore]
    public int Repeat { get; internal set; }

    /// <summary>
    /// Запись, которой принадлежит поле.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public Record? Record { get; internal set; }

    /// <summary>
    /// Пустое ли поле?
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public bool IsEmpty => Subfields.Count == 0;

    /// <summary>
    /// Получение значения первого повторения подполя с указанным кодом.
    /// </summary>
    public string? this [char code]
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        get => this.GetFirstSubFieldValue (code);
    }

    /// <summary>
    /// Получение значения первого повторения подполя с любым
    /// из указанныъ кодов.
    /// </summary>
    public string? this [char code1, char code2]
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        get => GetSubFieldValue (code1, code2);
    }

    /// <summary>
    /// Получение значения первого повторения подполя с любым
    /// из указанныъ кодов.
    /// </summary>
    public string? this [char code1, char code2, char code3]
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        get => GetSubFieldValue (code1, code2, code3);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Field()
    {
        Subfields = new SubFieldCollection { Field = this };
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="value">Значение поля до первого разделителя
    /// (опционально).</param>
    public Field
        (
            int tag,
            string? value
        )
        : this()
    {
        Tag = tag;
        Value = value;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Field
        (
            int tag,
            SubField subfield1
        )
        : this()
    {
        Tag = tag;
        Add (subfield1);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Field
        (
            int tag,
            SubField subfield1,
            SubField subfield2
        )
        : this()
    {
        Tag = tag;
        Add (subfield1);
        Add (subfield2);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Field
        (
            int tag,
            SubField subfield1,
            SubField subfield2,
            SubField subfield3
        )
        : this()
    {
        Tag = tag;
        Add (subfield1);
        Add (subfield2);
        Add (subfield3);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="subfields">Массив подполей.</param>
    public Field
        (
            int tag,
            params SubField[] subfields
        )
        : this()
    {
        Tag = tag;
        Subfields.AddRange (subfields);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code1">Код подполя.</param>
    /// <param name="value1">Значение подполя (опционально).</param>
    public Field
        (
            int tag,
            char code1,
            string? value1 = default
        )
        : this()
    {
        Tag = tag;
        Add (new SubField (code1, value1));
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code1">Код подполя.</param>
    /// <param name="value1">Значение подполя.</param>
    /// <param name="code2">Код подполя.</param>
    /// <param name="value2">Значение подполя (опционально).</param>
    public Field
        (
            int tag,
            char code1,
            string? value1,
            char code2,
            string? value2 = default
        )
        : this()
    {
        Tag = tag;
        Add (new SubField (code1, value1));
        Add (new SubField (code2, value2));
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code1">Код подполя.</param>
    /// <param name="value1">Значение подполя.</param>
    /// <param name="code2">Код подполя.</param>
    /// <param name="value2">Значение подполя.</param>
    /// <param name="code3">Код подполя.</param>
    /// <param name="value3">Значение подполя (опционально).</param>
    public Field
        (
            int tag,
            char code1,
            string? value1,
            char code2,
            string? value2,
            char code3,
            string? value3 = default
        )
        : this()
    {
        Tag = tag;
        Add (new SubField (code1, value1));
        Add (new SubField (code2, value2));
        Add (new SubField (code3, value3));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Поле с подполями.
    /// </summary>
    public static Field WithSubFields
        (
            int tag,
            params string[] subfields
        )
    {
        var result = new Field (tag);
        for (var i = 0; i < subfields.Length; i += 2)
        {
            var code = subfields[i][0];
            var value = subfields[i + 1];
            result.Subfields.Add (new SubField (code, value));
        }

        return result;
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей.
    /// </summary>
    /// <param name="subfield">Добавляемое подполе.</param>
    /// <returns>this</returns>
    public Field Add
        (
            SubField subfield
        )
    {
        Subfields.Add (subfield);

        return this;
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя (опционально).</param>
    /// <returns>this</returns>
    public Field Add
        (
            char code,
            ReadOnlyMemory<char> value
        )
    {
        Subfields.Add (new SubField (code, value));

        return this;
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей.
    /// </summary>
    public Field Add
        (
            char code,
            bool value
        )
    {
        return Add (code, value ? "1" : "0");
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей.
    /// </summary>
    /// <remarks>Фиксируется текстовое представление объекта
    /// на момент добавления.</remarks>>
    public Field Add
        (
            char code,
            object? value
        )
    {
        if (code == ValueCode)
        {
            Value = value?.ToString();
            return this;
        }

        var text = value?.ToString();
        Add (code, text);

        return this;
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя (опционально).</param>
    /// <returns>this</returns>
    public Field Add
        (
            char code,
            string? value = default
        )
    {
        Subfields.Add (new SubField (code, value));

        return this;
    }

    /// <summary>
    /// Добавление поля, если переданное значение не равно 0.
    /// </summary>
    public Field AddNonEmpty
        (
            char code,
            int value
        )
    {
        if (value is not 0)
        {
            Add (code, value.ToInvariantString());
        }

        return this;
    }

    /// <summary>
    /// Добавление поля, если переданное значение не равно 0.
    /// </summary>
    public Field AddNonEmpty
        (
            char code,
            int? value
        )
    {
        if (value.HasValue && value.Value is not 0)
        {
            Add (code, value.Value.ToInvariantString());
        }

        return this;
    }

    /// <summary>
    /// Добавление поля, если переданное значение не равно 0.
    /// </summary>
    public Field AddNonEmpty
        (
            char code,
            long value
        )
    {
        if (value is not 0L)
        {
            Add (code, value.ToInvariantString());
        }

        return this;
    }

    /// <summary>
    /// Добавление поля, если переданная дата имеет смысл
    /// (не равна <see cref="DateTime.MinValue"/>).
    /// </summary>
    public Field AddNonEmpty
        (
            char code,
            DateTime? value
        )
    {
        if (value is not null)
        {
            if (value.Value != DateTime.MinValue)
            {
                Add (code, IrbisDate.ConvertDateToString (value.Value));
            }
        }

        return this;
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей
    /// при условии, что значение поля не пустое.
    /// </summary>
    public Field AddNonEmpty
        (
            char code,
            object? value
        )
    {
        if (value is not null)
        {
            if (code == ValueCode)
            {
                Value = value.ToString();
                return this;
            }

            var text = value.ToString();
            if (!string.IsNullOrEmpty (text))
            {
                Add (code, text);
            }
        }

        return this;
    }

    /// <summary>
    /// Добавление подполя в конец списка подполей
    /// при условии, что значение поля не пустое.
    /// </summary>
    public Field AddNonEmpty
        (
            char code,
            bool flag,
            object? value
        )
    {
        if (flag && value is not null)
        {
            if (code == ValueCode)
            {
                Value = value.ToString();
                return this;
            }

            var text = value.ToString();
            if (!string.IsNullOrEmpty (text))
            {
                Add (code, text);
            }
        }

        return this;
    }

    /// <summary>
    /// Добавление нескольких полей согласно спецификации кодов.
    /// </summary>
    public Field AddRange
        (
            ReadOnlySpan<char> codes,
            IEnumerable<string?>? values
        )
    {
        if (values is not null)
        {
            var index = 0;
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty (value))
                {
                    Add (codes[index], value);
                    ++index;
                }
            }
        }

        return this;
    }

    /// <summary>
    /// Добавление нескольких полей.
    /// </summary>
    public Field AddRange
        (
            IEnumerable<SubField?>? subFields
        )
    {
        if (subFields is not null)
        {
            foreach (var subField in subFields)
            {
                if (subField is not null)
                {
                    Add (subField);
                }
            }
        }

        return this;
    }

    /// <summary>
    /// Присваивание одного поля другому.
    /// </summary>
    public Field AssignFrom
        (
            Field source
        )
    {
        Subfields.Clear();
        foreach (var subField in source.Subfields)
        {
            Subfields.Add (subField.Clone());
        }

        return this;
    }

    /// <summary>
    /// Очистка списка подполей. Остальные свойства остаются.
    /// </summary>
    public Field Clear()
    {
        ThrowIfReadOnly();
        Subfields.Clear();

        return this;
    }

    /// <summary>
    /// Сравнение двух полей.
    /// </summary>
    public static int Compare
        (
            Field field1,
            Field field2
        )
    {
        var result = field1.Tag - field2.Tag;
        if (result != 0)
        {
            return result;
        }

        result = Utility.CompareOrdinal
            (
                field1.Value,
                field2.Value
            );
        if (result != 0)
        {
            return result;
        }

        result = field1.Subfields.Count - field2.Subfields.Count;
        if (result != 0)
        {
            return result;
        }

        for (var i = 0; i < field1.Subfields.Count; i++)
        {
            var subField1 = field1.Subfields[i];
            var subField2 = field2.Subfields[i];

            result = SubField.Compare
                (
                    subField1,
                    subField2
                );
            if (result != 0)
            {
                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Если нет подполя, выделенного для хранения
    /// значения поля до первого разделителя,
    /// создаем его (оно должно быть первым в списке подполей).
    /// </summary>
    public SubField CreateValueSubField()
    {
        SubField result;

        if (Subfields.Count == 0)
        {
            result = new SubField { Code = ValueCode };
            Subfields.Add (result);

            return result;
        }

        result = Subfields[0];
        if (result.Code != ValueCode)
        {
            result = new SubField { Code = ValueCode };
            Subfields.Insert (0, result);
        }

        return result;
    }

    /// <summary>
    /// Получаем подполе, выделенное для хранения
    /// значения поля до первого разделителя.
    /// </summary>
    public SubField? GetValueSubField()
    {
        if (Subfields.Count == 0)
        {
            return null;
        }

        var result = Subfields[0];
        if (result.Code == ValueCode)
        {
            return result;
        }

        return null;
    }

    /// <summary>
    /// Клонирование поля.
    /// </summary>
    public Field Clone()
    {
        var result = new Field (Tag);

        for (var i = 0; i < Subfields.Count; i++)
        {
            result.Subfields.Add (Subfields[i].Clone());
        }

        return result;
    }

    /// <summary>
    /// Декодирование строки.
    /// </summary>
    public void Decode
        (
            ReadOnlySpan<char> line
        )
    {
        var index = line.IndexOf ('#');
        if (index <= 0)
        {
            throw new ArgumentException ($"Bad field text: {line.ToString()}");
        }

        Tag = line.Slice (0, index).ParseInt32();
        line = line.Slice (index + 1);
        DecodeBody (line);
    }

    /// <summary>
    /// Декодирование тела поля.
    /// </summary>
    public void DecodeBody
        (
            ReadOnlySpan<char> line
        )
    {
        var index = line.IndexOf ('^');
        if (index < 0)
        {
            // TODO: реализовать оптимально
            Value = line.ToString();
            return;
        }

        if (index != 0)
        {
            // TODO: реализовать оптимально
            Value = line.Slice (0, index).ToString();
        }

        line = line.Slice (index + 1);

        try
        {
            while (!line.IsEmpty)
            {
                index = line.IndexOf ('^');
                if (index < 0)
                {
                    Add (line[0], line.Slice (1).ToString());
                    return;
                }

                if (index != 0)
                {
                    // если index == 0, мы попали на строку вида
                    // 910#^Ap^B1^C20061003^DЧЗ^^PЗ461/2006/ Подшивка № 746 сент.-окт^IП746
                    // пропускаем без сожаления

                    Add (line[0], line.Slice (1, index - 1).ToString());
                }

                line = line.Slice (index + 1);
            }
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (Field) + "::" + nameof (DecodeBody)
                );
            Magna.Logger.LogError
                (
                    nameof (Field) + "::" + nameof (DecodeBody)
                    + ": bad line: {Line}",
                    line.ToString()
                );

            throw;
        }
    }

    /// <summary>
    /// Получение первого подполя с указанным кодом.
    /// </summary>
    public SubField? GetFirstSubField
        (
            char code
        )
    {
        if (code == ValueCode)
        {
            var firstSubfield = Subfields.FirstOrDefault();
            if (firstSubfield?.Code == ValueCode)
            {
                return firstSubfield;
            }

            // дальше первого элемента искать не имеет смысла

            return null;
        }

        if (code == '*')
        {
            return Subfields.FirstOrDefault();
        }

        foreach (var subfield in Subfields)
        {
            if (subfield.Code.SameChar (code))
            {
                return subfield;
            }
        }

        return null;
    }

    /// <summary>
    /// Перечисление подполей с указанным кодом.
    /// </summary>
    public IEnumerable<SubField> EnumerateSubFields
        (
            char code
        )
    {
        if (code == ValueCode)
        {
            var value = GetValueSubField();
            if (value is not null)
            {
                yield return value;
            }

            yield break;
        }

        if (code == '*')
        {
            var first = Subfields.FirstOrDefault();
            if (first is not null)
            {
                yield return first;
            }

            yield break;
        }

        foreach (var subfield in Subfields)
        {
            if (subfield.Code.SameChar (code))
            {
                yield return subfield;
            }
        }
    }

    /// <summary>
    /// Получение всех подполей с указанным кодом.
    /// </summary>
    public SubField[] GetSubFields
        (
            char code
        )
    {
        var result = new List<SubField>();

        if (code == '*')
        {
            var first = Subfields.FirstOrDefault();
            if (first is not null)
            {
                result.Add (first);
            }
        }
        else
        {
            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar (code))
                {
                    result.Add (subfield);
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение всех подполей с указанными подполями.
    /// </summary>
    public SubField[] GetSubFields
        (
            char code1,
            char code2
        )
    {
        var result = new List<SubField>();

        foreach (var subfield in Subfields)
        {
            var code = subfield.Code;
            if (code.SameChar (code1) || code.SameChar (code2))
            {
                result.Add (subfield);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение всех подполей с указанными подполями.
    /// </summary>
    public SubField[] GetSubFields
        (
            char code1,
            char code2,
            char code3
        )
    {
        var result = new List<SubField>();

        foreach (var subfield in Subfields)
        {
            var code = subfield.Code;
            if (code.SameChar (code1)
                || code.SameChar (code2)
                || code.SameChar (code3))
            {
                result.Add (subfield);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение всех подполей с указанными подполями.
    /// </summary>
    public SubField[] GetSubFields
        (
            char code1,
            char code2,
            char code3,
            char code4
        )
    {
        var result = new List<SubField>();

        foreach (var subfield in Subfields)
        {
            var code = subfield.Code;
            if (code.SameChar (code1)
                || code.SameChar (code2)
                || code.SameChar (code3)
                || code.SameChar (code4))
            {
                result.Add (subfield);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение первого подполя с указанным кодом
    /// либо создание нового подполя, если таковое отсуствует.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <returns>Найденное или созданное подполе.</returns>
    public SubField GetOrAddSubField
        (
            char code
        )
    {
        if (code == ValueCode)
        {
            return GetValueSubField() ?? CreateValueSubField();
        }

        if (code == '*')
        {
            return Subfields.FirstOrDefault() ?? CreateValueSubField();
        }

        foreach (var subfield in Subfields)
        {
            if (subfield.Code.SameChar (code))
            {
                return subfield;
            }
        }

        var result = new SubField (code);
        Add (result);

        return result;
    }

    /// <summary>
    /// Выдает указанное повторение подполя с данным кодом.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="occurrence">Номер повторения.
    /// Нумерация начинается с нуля.
    /// Отрицательные индексы отсчитываются с конца массива.</param>
    /// <returns>Найденное подполе или <c>null</c>.</returns>
    public SubField? GetSubField
        (
            char code,
            int occurrence = 0
        )
    {
        if (code == ValueCode)
        {
            if (occurrence != 0)
            {
                return null;
            }

            return GetValueSubField();
        }

        if (code == '*')
        {
            if (occurrence != 0)
            {
                return null;
            }

            return Subfields.FirstOrDefault();
        }

        if (occurrence < 0)
        {
            // отрицательные индексы отсчитываются от конца
            occurrence = Subfields.Count (sf => sf.Code.SameChar (code)) + occurrence;
            if (occurrence < 0)
            {
                return null;
            }
        }

        foreach (var subfield in Subfields)
        {
            if (subfield.Code.SameChar (code))
            {
                if (occurrence == 0)
                {
                    return subfield;
                }

                --occurrence;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение текста указанного подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="occurrence">Номер повторения.
    /// Нумерация начинается с нуля.
    /// Отрицательные индексы отсчитываются с конца массива.</param>
    /// <returns>Текст найденного подполя или <c>null</c>.</returns>
    public string? GetSubFieldValue
        (
            char code,
            int occurrence = 0
        )
    {
        return GetSubField (code, occurrence)?.Value;
    }

    /// <summary>
    /// Получение текста первого из указанных подполей.
    /// </summary>
    public string? GetSubFieldValue
        (
            char code1,
            char code2
        )
    {
        return (GetSubField (code1) ?? GetSubField (code2))?.Value;
    }

    /// <summary>
    /// Получение текста первого из указанных подполей.
    /// </summary>
    public string? GetSubFieldValue
        (
            char code1,
            char code2,
            char code3
        )
    {
        return (GetSubField (code1) ?? GetSubField (code2) ?? GetSubField (code3))?.Value;
    }

    /// <summary>
    /// Поиск подполя '*' (имитация ИРБИС).
    /// </summary>
    public string? GetValueOrFirstSubField()
    {
        return Subfields.FirstOrDefault()?.Value;
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            string? value
        )
    {
        if (code == ValueCode)
        {
            Value = value;
        }
        else if (code == '*')
        {
            var first = Subfields.FirstOrDefault() ?? CreateValueSubField();
            first.Value = value;
        }
        else
        {
            if (string.IsNullOrEmpty (value))
            {
                RemoveSubField (code);
            }
            else
            {
                GetOrAddSubField (code).Value = value;
            }
        }

        return this;
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            int value
        )
    {
        return SetSubFieldValue (code, value.ToInvariantString());
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            long value
        )
    {
        return SetSubFieldValue (code, value.ToInvariantString());
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            DateTime value
        )
    {
        return SetSubFieldValue (code, IrbisDate.ConvertDateToString (value));
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            int? value
        )
    {
        return SetSubFieldValue (code, value?.ToInvariantString());
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            long? value
        )
    {
        return SetSubFieldValue (code, value?.ToInvariantString());
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            DateTime? value
        )
    {
        return SetSubFieldValue (code, value.HasValue ? IrbisDate.ConvertDateToString (value.Value) : null);
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            object? value
        )
    {
        var text = value is IFormattable formattable
            ? formattable.ToString (null, CultureInfo.InvariantCulture)
            : value?.ToString();

        return SetSubFieldValue (code, text);
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="flag">Флаг: добавление подполя (<c>true</c>) или его удаление.</param>
    /// <param name="value">Новое значение подполя.</param>
    /// <returns>this</returns>
    public Field SetSubFieldValue
        (
            char code,
            bool flag,
            object? value
        )
    {
        string? text = null;

        if (flag)
        {
            if (value is IFormattable formattable)
            {
                text = formattable.ToString (null, CultureInfo.InvariantCulture);
            }
            else
            {
                text = value?.ToString();
            }
        }

        if (code == ValueCode)
        {
            Value = text;
        }
        else
        {
            if (string.IsNullOrEmpty (text))
            {
                RemoveSubField (code);
            }
            else
            {
                GetOrAddSubField (code).Value = text;
            }
        }

        return this;
    }

    /// <summary>
    /// Установка значений подполей скопом (согласно предлагаемым кодам).
    /// </summary>
    public Field SetSubFieldValue
        (
            ReadOnlySpan<char> codes,
            IEnumerable<string?>? values
        )
    {
        foreach (var code in codes)
        {
            RemoveSubField (code);
        }

        if (values is not null)
        {
            var index = 0;
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty (value))
                {
                    SetSubFieldValue (codes[index], value);
                    ++index;
                }
            }
        }

        return this;
    }

    /// <summary>
    /// Удаление подполей с указанным кодом.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <returns>this</returns>
    public Field RemoveSubField
        (
            char code
        )
    {
        while (GetFirstSubField (code) is { } subfield)
        {
            Subfields.Remove (subfield);
        }

        return this;
    }

    /// <summary>
    /// Текстовое представление только значимой части поля.
    /// Метка поля не выводится.
    /// </summary>
    public string ToText()
    {
        var length = Subfields.Sum
            (
                sf => (sf.Value!.Length)
                      + (sf.Code == ValueCode ? 1 : 2)
            );
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (length);

        foreach (var subField in Subfields)
        {
            var subText = subField.ToString();
            builder.Append (subText);
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion

    #region Operators

    /// <summary>
    /// Добавление подполя к полю.
    /// </summary>
    public static Field operator +
        (
            Field field,
            SubField? subField
        )
    {
        Sure.NotNull (field);

        if (subField is not null)
        {
            field.Subfields.Add (subField);
        }

        return field;
    }

    /// <summary>
    /// Добавление подполя к полю.
    /// </summary>
    public static Field operator +
        (
            Field field,
            string? text
        )
    {
        Sure.NotNull (field);

        if (!string.IsNullOrEmpty (text))
        {
            var subField = text[0] == SubField.Delimiter
                ? new SubField (text[1], text[2..])
                : new SubField (text[0], text[1..]);
            field.Subfields.Add (subField);
        }

        return field;
    }

    /// <summary>
    /// Удаление подполя.
    /// </summary>
    public static Field operator -
        (
            Field field,
            SubField? subField
        )
    {
        Sure.NotNull (field);

        if (subField is not null)
        {
            field.Subfields.Remove (subField);
        }

        return field;
    }

    /// <summary>
    /// Удаление подполя.
    /// </summary>
    public static Field operator -
        (
            Field field,
            char code
        )
    {
        Sure.NotNull (field);

        field.RemoveSubField (code);

        return field;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Tag = reader.ReadPackedInt32();
        Subfields.RestoreFromStream (reader);
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer, nameof (writer));

        writer.WritePackedInt32 (Tag);
        Subfields.SaveToStream (writer);
    }

    #endregion

    #region IReadOnly<T> members

    /// <inheritdoc cref="IReadOnly{T}.AsReadOnly"/>
    public Field AsReadOnly()
    {
        var result = Clone();
        result.SetReadOnly();

        return result;
    }

    /// <inheritdoc cref="IReadOnly{T}.ReadOnly"/>
    public bool ReadOnly { get; internal set; }

    /// <inheritdoc cref="IReadOnly{T}.SetReadOnly"/>
    public void SetReadOnly()
    {
        ReadOnly = true;
        Subfields.ReadOnly = true;

        foreach (var subfield in Subfields)
        {
            subfield.SetReadOnly();
        }
    }

    /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly"/>
    public void ThrowIfReadOnly()
    {
        if (ReadOnly)
        {
            Magna.Logger.LogError (nameof (Field) + "::" + nameof (ThrowIfReadOnly));

            throw new ReadOnlyException();
        }
    }

    #endregion

    #region IEnumerable<SubField> members

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    [ExcludeFromCodeCoverage]
    public IEnumerator<SubField> GetEnumerator() => Subfields.GetEnumerator();

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<Field> (this, throwOnError);

        verifier.Positive (Tag);
        verifier.Positive (Subfields.Count);
        foreach (var subfield in Subfields)
        {
            verifier.VerifySubObject (subfield);
        }

        if (verifier.Result)
        {
            for (var i = 1; i < Subfields.Count; i++)
            {
                if (Subfields[i].Code == ValueCode)
                {
                    verifier.Failure ("Value field is not first");
                }
            }
        }

        return verifier.Result;
    }

    #endregion

    #region IResettable members

    /// <inheritdoc cref="IResettable.TryReset"/>
    public bool TryReset()
    {
        Tag = NoTag;
        Value = default;
        Subfields.Clear();

        return true;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var length = 4 + Subfields.Sum
            (
                sf => (sf.Value?.Length ?? 0)
                      + (sf.Code == ValueCode ? 0 : 2)
            );
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (length);
        builder.Append (Tag.ToInvariantString())
            .Append ('#');
        foreach (var subfield in Subfields)
        {
            builder.Append (subfield);
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
