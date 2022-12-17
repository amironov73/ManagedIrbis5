// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DefaultIdBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
[PublicAPI]
public class DefaultIdBuilder
{
    #region Private members

    private static readonly Regex IdRegex = new (@"[\.\[\]]");

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string Build
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        return IdRegex.Replace (request.ElementId, "_");
    }

    #endregion
}
