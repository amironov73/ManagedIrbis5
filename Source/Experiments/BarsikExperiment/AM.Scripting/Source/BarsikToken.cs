// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* BarsikToken.cs -- токен языка Барсик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

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
    /// Вид токена.
    /// </summary>
    public TokenKind Kind { get; internal set; }

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
            TokenKind kind,
            ReadOnlyMemory<char> value = default
        )
    {
        Kind = kind;
        Value = value;
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
        return HashCode.Combine ((int) Kind, Value);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Kind}: {Value}";
    }

    #endregion
}
