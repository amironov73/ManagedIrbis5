// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* SyntaxException.cs -- синтаксическая ошибка в скрипте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Синтаксическая ошибка в скрипте.
/// </summary>
public sealed class SyntaxException
    : ApplicationException
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

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SyntaxException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyntaxException
        (
            int line,
            int column
        )
    {
        Line = line;
        Column = column;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyntaxException
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        Line = navigator.Line;
        Column = navigator.Column;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyntaxException
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyntaxException
        (
            string? message
        )
        : base(message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyntaxException
        (
            string? message,
            int line,
            int column
        ) : base(message)
    {
        Line = line;
        Column = column;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyntaxException
        (
            string? message,
            Exception? innerException
        )
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Exception.ToString"/>
    public override string ToString()
    {
        var result = base.ToString();

        return Line > 0
            ? Column > 0
                ? $"[{Line}: {Column}]" + result
                : $"[{Line}]" + result
            : result;
    }

    #endregion
}
