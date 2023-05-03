// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SourcePosition.cs -- позиция (строка, столбец) в исходном коде скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Позиция (строка, столбец) в исходном коде скрипта.
/// </summary>
[PublicAPI]
public sealed class SourcePosition
{
    #region Properties

    /// <summary>
    /// Номер строки.
    /// </summary>
    public int Line { get; }

    /// <summary>
    /// Номер столбца.
    /// </summary>
    public int Column { get; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SourcePosition
        (
            int line,
            int column
        )
    {
        Line = line;
        Column = column;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"[{Line}, {Column}]";

    #endregion
}
