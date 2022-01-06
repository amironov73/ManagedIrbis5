// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* SourcePosition.cs -- позиция в коде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Позиция в коде.
/// </summary>
public readonly struct SourcePosition
{
    #region Properties

    /// <summary>
    /// Номер столбца (нумерация с 1).
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Номер строки (нумерация с 1).
    /// </summary>
    public int Line { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="line">Номер строки.</param>
    /// <param name="column">Номер столбца.</param>
    public SourcePosition
        (
            int line,
            int column
        )
    {
        Column = column;
        Line = line;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SourcePosition
        (
            SourcePos sourcePos
        )

    {
        Column = sourcePos.Col;
        Line = sourcePos.Line;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{Line}:{Column}";
    }

    #endregion
}
