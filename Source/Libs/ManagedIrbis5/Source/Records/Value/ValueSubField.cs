// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* ValueSubField.cs -- подполе библиографической записи, оформленное как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Подполе библиографической записи, оформленное как структура.
/// </summary>
public readonly struct ValueSubField
    : IVerifiable
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
    public readonly char Code;

    /// <summary>
    /// Значение подполя.
    /// </summary>
    public readonly Memory<char> Value;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="code">Код подполя.</param>
    /// <param name="value">Значение подполя.</param>
    public ValueSubField
        (
            char code,
            Memory<char> value
        )
    {
        Code = code;
        Value = value;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Клонирование подполя.
    /// </summary>
    public ValueSubField Clone()
    {
        return new ValueSubField (Code, Value);
    }

    /// <summary>
    /// Сравнение двух подполей.
    /// </summary>
    public static int Compare
        (
            ValueSubField subField1,
            ValueSubField subField2
        )
    {
        // сравниваем коды подполей с точностью до регистра символов
        var result = char.ToUpperInvariant (subField1.Code)
            .CompareTo (char.ToUpperInvariant (subField2.Code));
        if (result != 0)
        {
            return result;
        }

        result = Utility.CompareOrdinal (subField1.Value, subField1.Value);

        return result;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ValueSubField> (this, throwOnError);

        verifier.Assert
            (
                Code is NoCode or > ' ',
                $"Wrong subfield code {Code}"
            );

        verifier.Assert
            (
                SubFieldValue.Verify (Value.Span, throwOnError)
            );

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Code == NoCode
            ? Value.ToString()
            : "^" + char.ToLowerInvariant (Code) + Value;
    }

    #endregion
}
