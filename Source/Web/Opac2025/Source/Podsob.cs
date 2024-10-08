// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Podsob.cs -- информация о выданном экземпляре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

// using LinqToDB.Mapping;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о выданном экземпляре.
/// </summary>
[PublicAPI]
// [Table ("podsob")]
public sealed class Podsob
{
    #region Properties

    /// <summary>
    /// Инвентарный номер.
    /// </summary>
    // [Column ("invnum")]
    public int Number { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    // [Nullable]
    // [Column ("chb")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Книга на руках.
    /// </summary>
    // [Nullable]
    // [Column ("onhand")]
    public string? OnHands { get; set; }

    #endregion
}
