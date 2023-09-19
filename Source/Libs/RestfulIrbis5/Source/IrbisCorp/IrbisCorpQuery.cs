// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IrbisCorpRequest.cs -- поисковый запрос для ИРБИС-корпорации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace RestfulIrbis.IrbisCorp;

/// <summary>
/// Поисковый клиент для ИРБИС-корпорации.
/// </summary>
[PublicAPI]
public sealed class IrbisCorpQuery
{
    #region Properties

    /// <summary>
    /// ISBN.
    /// </summary>
    public string? Isbn { get; set; }

    /// <summary>
    /// Автор, редактор, составитель.
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Заглавие.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Год издания.
    /// </summary>
    public string? Year { get; set; }

    #endregion
}
