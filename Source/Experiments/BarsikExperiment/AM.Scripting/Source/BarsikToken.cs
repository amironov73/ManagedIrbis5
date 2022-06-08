// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* BarsikToken.cs -- токен языка Барсик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Токен языка Барсик.
/// </summary>
public sealed class BarsikToken
{
    #region Properties

    /// <summary>
    /// Токен-признак конца текста.
    /// </summary>
    public static readonly BarsikToken Eot = new (EOT);

    #endregion

    #region Constants

    /// <summary>
    /// Признак конца текста.
    /// </summary>
    public const string? EOT = null;

    /// <summary>
    /// Ключевое слово, например, "if".
    /// </summary>
    public const string Keyword = "keyword";

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public const string Identifier = "idenfier";

    /// <summary>
    /// Один символ в одиночных кавычках.
    /// </summary>
    public const string Char = "char";

    /// <summary>
    /// Произвольное количество символов в двойных кавычках.
    /// </summary>
    public const string String = "string";

    /// <summary>
    /// Целое 32-битное число со знаком без префикса и суффикса.
    /// </summary>
    public const string Int32 = "int32";

    /// <summary>
    /// Целое 64-битное число зе знака без префикса и суффикса.
    /// </summary>
    public const string Int64 = "int64";

    /// <summary>
    /// Целое 32-битное число без знака без префикса и суффикса.
    /// </summary>
    public const string UInt32 = "uint32";

    /// <summary>
    /// Целое 64-битное число без знака без префикса и суффикса.
    /// </summary>
    public const string UInt64 = "uint64";

    /// <summary>
    /// Число с плавающей точкой с одинарной точностью.
    /// </summary>
    public const string Single = "single";

    /// <summary>
    /// Число с плавающей точкой с двойной точностью.
    /// </summary>
    public const string Double = "double";

    #endregion

    #region Properties

    /// <summary>
    /// Вид токена.
    /// </summary>
    public string? Kind { get; internal set; }

    /// <summary>
    /// Значение.
    /// </summary>
    public ReadOnlyMemory<char> Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BarsikToken
        (
            string? kind,
            ReadOnlyMemory<char> value = default
        )
    {
        Kind = kind;
        Value = value;
    }

    #endregion

    #region Public methods

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

    #endregion

    #region Operators

    /// <summary>
    /// Оператор сравнения со строкой.
    /// </summary>
    public static bool operator ==
        (
            BarsikToken token,
            string? kind
        )
    {
        return string.CompareOrdinal (token.Kind, kind) == 0;
    }

    /// <summary>
    /// Оператор сравнения со строкой.
    /// </summary>
    public static bool operator !=
        (
            BarsikToken token,
            string? kind
        )
    {
        return string.CompareOrdinal (token.Kind, kind) != 0;
    }

    #endregion

    #region Object members

    private bool Equals
        (
            BarsikToken other
        )
    {
        return Kind == other.Kind && Value.Equals (other.Value);
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return ReferenceEquals (this, obj)
               || obj is BarsikToken other && Equals (other);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine (Kind, Value);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Kind.ToVisibleString()}: {Value.ToVisibleString()}";
    }

    #endregion
}
