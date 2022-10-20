// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BbkEntry.cs -- запись в эталоне ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

namespace RestfulIrbis.RslServices;

/// <summary>
/// Запись в эталоне ББК.
/// </summary>
public sealed class BbkEntry
{
    #region Properties

    /// <summary>
    /// Индекс ББК.
    /// </summary>
    public string? Index { get; init; }

    /// <summary>
    /// Формулировка.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Ссылка на узел дерева.
    /// </summary>
    public string? Link { get; init; }

    #endregion

    #region ToString

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Index.ToVisibleString();

    #endregion
}
