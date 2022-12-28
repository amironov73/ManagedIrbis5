// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DisplayTagHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags;

using Conventions.Elements;

using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
///
/// </summary>
[HtmlTargetElement ("display-tag",
    Attributes = ForAttributeName,
    TagStructure = TagStructure.WithoutEndTag)]
public class DisplayTagHelper
    : HtmlTagTagHelper
{
    #region Protected members

    /// <summary>
    ///
    /// </summary>
    protected override string Category => ElementConstants.Display;

    #endregion
}
