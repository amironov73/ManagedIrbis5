// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IFormatDriver.cs -- драйвер для украшения расформатированного текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Formatting;

/// <summary>
/// Драйвер для украшения расформатированного текста.
/// </summary>
public interface IFormatDriver
{
    /// <summary>
    /// Полужирное начертание.
    /// </summary>
    void Bold (StringBuilder builder, string? text);

    /// <summary>
    /// Курсивное начертание.
    /// </summary>
    void Italic (StringBuilder builder, string? text);

    /// <summary>
    /// Подчеркнутый текст.
    /// </summary>
    void Underline (StringBuilder builder, string? text);

    /// <summary>
    /// Вывод ссылки.
    /// </summary>
    void Link (StringBuilder builder, string? link, string? text);
}
