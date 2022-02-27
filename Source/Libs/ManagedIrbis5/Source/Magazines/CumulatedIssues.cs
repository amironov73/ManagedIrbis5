// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* CumulatedMagazineIssues.cs -- предварительно кумулированные выпуски журналов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Предварительно кумулированные выпуски журналов.
/// </summary>
public sealed class CumulatedIssues
{
    #region Properties

    /// <summary>
    /// Значение, общее для всех выпусков (например, место хранения).
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Выпуски журналов.
    /// </summary>
    public CumulationData[]? Issues { get; set; }

    #endregion
}
