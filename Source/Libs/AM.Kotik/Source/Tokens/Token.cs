// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Token.cs -- токен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токен состоит из вида и значения.
/// </summary>
public sealed class Token
    : IEquatable<Token>
{
    #region Properties

    /// <summary>
    /// Номер строки, начинается с 1.
    /// </summary>
    public int Line { get; }

    /// <summary>
    /// Номер столбца, начинается с 1.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Вид токена.
    /// </summary>
    public string Kind { get; }

    /// <summary>
    /// Собственно значение токена.
    /// </summary>
    public string? Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Token
        (
            string kind,
            string? value,
            int line = 0,
            int column = 0
        )
    {
        Sure.NotNullNorEmpty (kind);

        Kind = kind;
        Value = value;
        Line = line;
        Column = column;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Число (неважно со знаком или без)?
    /// </summary>
    public bool IsNumber() => Kind is TokenKind.Int32 or TokenKind.Int64
            or TokenKind.Single or TokenKind.Double or TokenKind.Decimal
            or TokenKind.UInt32 or TokenKind.UInt64;

    /// <summary>
    /// Число со знаком?
    /// </summary>
    public bool IsSignedNumber() => Kind is TokenKind.Int32 or TokenKind.Int64
            or TokenKind.Single or TokenKind.Double or TokenKind.Decimal;

    /// <summary>
    /// Число без знака?
    /// </summary>
    public bool IsUnsignedNumber() => Kind is TokenKind.UInt32 or TokenKind.UInt64;

    /// <summary>
    /// Конец текста?
    /// </summary>
    public bool IsEot()
    {
        return string.IsNullOrEmpty (Kind);
    }

    /// <summary>
    /// Сравнение с заданной строкой.
    /// </summary>
    public bool IsOneOf
        (
            string kind1
        )
    {
        Sure.NotNull (kind1);

        return string.CompareOrdinal (Kind, kind1) == 0;
    }

    /// <summary>
    /// Сравнение с заданными строками.
    /// </summary>
    public bool IsOneOf
        (
            string kind1,
            string kind2
        )
    {
        Sure.NotNull (kind1);
        Sure.NotNull (kind2);

        return string.CompareOrdinal (Kind, kind1) == 0
            || string.CompareOrdinal (Kind, kind2) == 0;
    }

    /// <summary>
    /// Сравнение с заданными строками.
    /// </summary>
    public bool IsOneOf
        (
            string kind1,
            string kind2,
            string kind3
        )
    {
        Sure.NotNull (kind1);
        Sure.NotNull (kind2);
        Sure.NotNull (kind3);

        return string.CompareOrdinal (Kind, kind1) == 0
            || string.CompareOrdinal (Kind, kind2) == 0
            || string.CompareOrdinal (Kind, kind3) == 0;
    }

    /// <summary>
    /// Сравнение с заданными строками.
    /// </summary>
    public bool IsOneOf
        (
            IEnumerable<string> kinds
        )
    {
        Sure.NotNull ((object?) kinds);

        foreach (var kind in kinds)
        {
            Sure.NotNull (kind);
            if (string.CompareOrdinal (Kind, kind) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Сравнение с заданными строками.
    /// </summary>
    public bool IsOneOf
        (
            params string[] kinds
        )
    {
        Sure.NotNull ((object?) kinds);

        foreach (var kind in kinds)
        {
            Sure.NotNull (kind);
            if (string.CompareOrdinal (Kind, kind) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Токен с новым значением.
    /// </summary>
    public Token WithNewValue (string? value) => new (Kind, value, Line, Column);

    #endregion

    #region Operators

    /// <summary>
    /// Оператор сравнения со строкой.
    /// </summary>
    public static bool operator ==
        (
            Token? token,
            string? kind
        )
    {
        if (token is null || kind is null)
        {
            return false;
        }

        return string.CompareOrdinal (token.Kind, kind) == 0;
    }

    /// <summary>
    /// Оператор сравнения со строкой.
    /// </summary>
    public static bool operator !=
        (
            Token? token,
            string? kind
        )
    {
        if (token is null || kind is null)
        {
            return true;
        }

        return string.CompareOrdinal (token.Kind, kind) != 0;
    }

    #endregion

    #region IEquatable<T> members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals
        (
            Token? other
        )
    {
        return other is not null
            && string.CompareOrdinal (Kind, other.Kind) == 0
            && string.CompareOrdinal (Value, other.Value) == 0;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return ReferenceEquals (this, obj)
               || obj is Token other && Equals (other);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine (Kind, Value);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() =>
        $"[{Line}, {Column}] {Kind.ToVisibleString()}: {Value.ToVisibleString()}";

    #endregion
}
