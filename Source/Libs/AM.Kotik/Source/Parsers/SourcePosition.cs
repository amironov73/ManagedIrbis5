// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SourcePosition.cs -- позиция (строка, столбец) в исходном коде скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Позиция (строка, столбец) в исходном коде скрипта.
/// </summary>
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
