// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SubField.cs -- подполе библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis;

/*

    Подполе является составной частью поля библиографической записи.
    Поле может состоять из одного или более подполей.

    > **ПОДПОЛЕ** (Subfield) – определенная единица информации в пределах поля.
    >
    > **ЭЛЕМЕНТ ДАННЫХ** (Data Element) – наименьшая единица библиографической
    > записи, подлежащая точной идентификации. В переменном поле элемент данных
    > идентифицируется идентификатором подполя, с помощью которого образуется
    > подполе. В маркере записи (Record Label), справочнике (Directory)
    > и в подполях фиксированной длины элементы данных, состоящие из кодов,
    > идентифицируются соответствующими позициями своих символов.
    >
    > **ИДЕНТИФИКАТОР ПОДПОЛЯ** (Subfield Identifier) или **КОД ПОДПОЛЯ**
    > (Subfield code) – код, идентифицирующий отдельные подполя внутри переменного
    > поля. Состоит из двух символов. Первый символ – разделитель (Delimiter),
    > всегда один и тот же уникальный символ, установленный по ISO 2709,
    > второй символ – код подполя (Subfield code), который может быть цифровым
    > или буквенным.
    >
    > **ПОДПОЛЕ ФИКСИРОВАННОЙ ДЛИНЫ** (Fixed Length Subfield) – подполе
    > постоянной длины, все случаи применения которого определены положениями
    > формата. Подполе фиксированной длины может быть определено как содержащее
    > один или более элементов данных. Подполя фиксированной длины могут
    > присутствовать в фиксированных полях, например, поле 100,
    > подполе `$a`, и в переменных полях, например, поле 200, подполе `$z`.
    >
    > *Стандарт RUSMARC*

    Подполя в составе поля отделяются друг от друга специальным символом-разделителем.
    В ИРБИС разделитель - крышка `^`.

    Подполе состоит из однобуквенного кода и значения произвольной длины.
    Технически возможны подполя с пустой строкой в качестве значения,
    однако, стандарт RUSMARC не допускает таких значений.

    Коды подполей нечувствительны к регистру символов.

    Технически возможны любые коды подполей, однако стандарт допускает
    лишь коды в дипапазоне от `\u0021` (восклицательный знак)
    до `\u007E` (тильда).

    Кириллические символы лучше не использовать в качестве кодов подполей - возникнут
    проблемы при экспорте записей в коммуникативные форматы. ManagedIrbis обрабатывает
    любые коды подполей без проблем.

    В стандарте RUSMARC принято ссылаться на подполя `$a`, `$b` и т. д.
    В документации ИРБИС64 принято обозначение `^a`, `^b` и т. д.
    Мы будем придерживаться последнего обозначения.

    Стандарт допускает лишь как правило алфавитно-цифровые код
    подполей `A-Z, 0-9`, но в ИРБИС64 бывают подполя с экзотическими
    кодами вроде `!`, `(` и др.

    ИРБИС64 трактует код подполя `*` как "данные до первого разделителя
    либо значение первого по порядку подполя" (смотря по тому,
    что присутствует в записи). ManagedIrbis поддерживает эту особенность.

    Подполе с кодом `\0` используется для хранения значения поля до первого разделителя.
    Это расширение ManagedIrbis.

    Значение подполя не может содержать символ разделителя подполей.
    Кроме того, значение подполя не должно содержать символов с кодами
    меньше `\u0020`.

    Никаких средств экранирования недопустимых символов в ИРБИС не предусмотрено.

    Код подполя может быть равен разделителю подполей.

 */

/// <summary>
/// Подполе библиографической записи.
/// </summary>
[XmlRoot ("subfield")]
public sealed class SubField
    : IVerifiable,
    IXmlSerializable,
    IHandmadeSerializable,
    IReadOnly<SubField>
{
    #region Constants

    /// <summary>
    /// Нет кода подполя, т. е. код пока не задан.
    /// Также используется для обозначения, что подполе
    /// используется для хранения значения поля
    /// до первого разделителя.
    /// </summary>
    public const char NoCode = '\0';

    /// <summary>
    /// Разделитель подполей.
    /// </summary>
    public const char Delimiter = '^';

    #endregion

    #region Properties

    /// <summary>
    /// Код подполя.
    /// </summary>
    public char Code { get; set; } = NoCode;

    /// <summary>
    /// Значение подполя.
    /// </summary>
    public string? Value
    {
        get => _value;
        set => SetValue (value);
    }

    /// <summary>
    /// Подполе хранит значение поля до первого разделителя.
    /// </summary>
    public bool RepresentsValue => Code == NoCode;

    /// <summary>
    /// Ссылка на поле.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public Field? Field { get; internal set; }

    /// <summary>
    /// Флаг: удалять начальные и конечные пробелы в значении поля.
    /// </summary>

    // ReSharper disable InconsistentNaming
    public static bool TrimValue;

    // ReSharper restore InconsistentNaming

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SubField()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя.</param>
    public SubField
        (
            char code,
            ReadOnlySpan<char> value
        )
    {
        Code = code;
        Value = value.EmptyToNull();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя.</param>
    public SubField
        (
            char code,
            ReadOnlyMemory<char> value
        )
    {
        Code = code;
        Value = value.EmptyToNull();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя (опционально).</param>
    public SubField
        (
            char code,
            string? value = default
        )
    {
        Code = code;
        Value = value;
    }

    #endregion

    #region Private members

    private string? _value;

    #endregion

    #region Public methods

    /// <summary>
    /// Клонирование подполя.
    /// </summary>
    public SubField Clone() => (SubField) MemberwiseClone();

    /// <summary>
    /// Сравнение двух подполей.
    /// </summary>
    public static int Compare
        (
            SubField subField1,
            SubField subField2
        )
    {
        // сравниваем коды подполей с точностью до регистра символов
        var result = char.ToUpperInvariant (subField1.Code)
            .CompareTo (char.ToUpperInvariant (subField2.Code));
        if (result != 0)
        {
            return result;
        }

        result = string.CompareOrdinal (subField1.Value, subField2.Value);

        return result;
    }

    /// <summary>
    /// Декодирование строки.
    /// </summary>
    public void Decode
        (
            ReadOnlySpan<char> text
        )
    {
        if (!text.IsEmpty)
        {
            var code = char.ToLowerInvariant (text[0]);
            SubFieldCode.Verify (code, true);
            Code = code;
            var value = text[1..];
            SubFieldValue.Verify (value, true);
            Value = value.EmptyToNull();
        }
    }

    /// <summary>
    /// Установка нового значения подполя.
    /// </summary>
    public void SetValue
        (
            ReadOnlySpan<char> value
        )
    {
        ThrowIfReadOnly();
        if (TrimValue)
        {
            value = value.Trim();
        }

        SubFieldValue.Verify (value, true);
        _value = value.ToString();
    }

    /// <summary>
    /// Установка нового значения подполя.
    /// </summary>
    public void SetValue
        (
            string? value
        )
    {
        ThrowIfReadOnly();
        value = value.EmptyToNull();
        if (TrimValue && value is not null)
        {
            value = value.Trim().EmptyToNull();
        }

        value = Utility.ReplaceControlCharacters (value);
        SubFieldValue.Verify (value, true);
        _value = value;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Code = reader.ReadChar();
        Value = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        writer.Write (Code);
        writer.WriteNullable (Value);
    }

    #endregion

    #region IXmlSerializable members

    /// <inheritdoc cref="IXmlSerializable.GetSchema"/>
    [ExcludeFromCodeCoverage]
    XmlSchema? IXmlSerializable.GetSchema() => null;

    /// <inheritdoc cref="IXmlSerializable.ReadXml"/>
    void IXmlSerializable.ReadXml
        (
            XmlReader reader
        )
    {
        Code = reader.GetAttribute ("code").FirstChar();
        Value = reader.GetAttribute ("value");
    }

    /// <inheritdoc cref="IXmlSerializable.WriteXml"/>
    void IXmlSerializable.WriteXml
        (
            XmlWriter writer
        )
    {
        writer.WriteAttributeString ("code", Code.ToString());
        writer.WriteAttributeString ("value", Value);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<SubField> (this, throwOnError);

        verifier.Assert
            (
                Code is NoCode or > ' ',
                $"Wrong subfield code {Code}"
            );

        verifier.Assert
            (
                SubFieldValue.Verify (Value, throwOnError)
            );

        return verifier.Result;
    }

    #endregion

    #region IReadOnly<T> members

    /// <inheritdoc cref="IReadOnly{T}.AsReadOnly"/>
    public SubField AsReadOnly()
    {
        var result = Clone();
        result.SetReadOnly();

        return result;
    }

    /// <inheritdoc cref="IReadOnly{T}.ReadOnly"/>
    [XmlIgnore]
    [JsonIgnore]
    public bool ReadOnly { get; private set; }

    /// <inheritdoc cref="IReadOnly{T}.SetReadOnly"/>
    public void SetReadOnly()
    {
        ReadOnly = true;
    }

    /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly"/>
    public void ThrowIfReadOnly()
    {
        if (ReadOnly)
        {
            throw new ReadOnlyException();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Code == NoCode
            ? Value ?? string.Empty
            : "^" + char.ToLowerInvariant (Code) + Value;
    }

    #endregion
}
