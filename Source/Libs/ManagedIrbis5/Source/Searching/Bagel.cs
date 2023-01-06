// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Bagel.cs -- результат "самоварного поиска"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Результат "самоварного поиска"
/// </summary>
public sealed class Bagel
{
    #region Properties

    /// <summary>
    /// Расформатированное библиографическое описание.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Найденная библиографическая запись.
    /// </summary>
    public Record? Record { get; set; }

    /// <summary>
    /// Рейтинг найденной записи.
    /// </summary>
    public int Rating { get; set; }

    #endregion
}
