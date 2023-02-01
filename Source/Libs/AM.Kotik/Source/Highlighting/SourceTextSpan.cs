// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SourceTextSpan.cs -- фрагмент исходного кода скрипта, размеченный для подсветки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Highlighting;

/// <summary>
/// Фрагмент исходного кода скрипта, размеченный для подсветки.
/// </summary>
public sealed class SourceTextSpan<THighlight>
{
    #region Properties

    /// <summary>
    /// Номер столбца.
    /// </summary>
    public int Column { get; init; }

    /// <summary>
    /// Фрагмент исходного кода.
    /// </summary>
    public required string Fragment { get; init; }
    
    /// <summary>
    /// Данные о подсветке.
    /// </summary>
    public required THighlight Highlight { get; init; }

    /// <summary>
    /// Номер строки.
    /// </summary>
    public int Line { get; init; }
    
    /// <summary>
    /// Смещение от начала текста (в символах).
    /// </summary>
    public int Offset { get; init; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Highlight} {Fragment}";

    #endregion
}
