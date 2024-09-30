// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Loan.cs -- информация о читательской задолженности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о читательской задолженности.
/// </summary>
[PublicAPI]
public sealed class Loan
{
    #region Properties

    /// <summary>
    /// Инвентарный номер книги.
    /// </summary>
    [JsonPropertyName ("number")]
    public string? Number { get; set; }

    /// <summary>
    /// Библиографическое описание книги.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Дата выдачи книги читателю.
    /// </summary>
    [JsonPropertyName ("date")]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Крайний срок возврата книги.
    /// </summary>
    [JsonPropertyName ("deadline")]
    public DateTimeOffset Deadline { get; set; }

    /// <summary>
    /// Счетчик продлений.
    /// </summary>
    [JsonPropertyName ("prolongation")]
    public int Prolongation { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Number}: {Description}";

    #endregion
}
