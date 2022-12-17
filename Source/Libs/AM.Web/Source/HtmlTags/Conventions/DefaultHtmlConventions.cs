// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DefaultHtmlConventions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class DefaultHtmlConventions
    : HtmlConventionRegistry
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public DefaultHtmlConventions()
    {
        this.Defaults();
    }

    #endregion
}
