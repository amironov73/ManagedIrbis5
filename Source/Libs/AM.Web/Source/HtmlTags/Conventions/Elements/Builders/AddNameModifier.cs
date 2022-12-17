// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AddNameModifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
[PublicAPI]
public class AddNameModifier
    : IElementModifier
{
    #region IElementModifier members

    /// <inheritdoc cref="ITagModifier.Matches"/>
    public bool Matches (ElementRequest token) => true;

    /// <inheritdoc cref="ITagModifier.Modify"/>
    public void Modify
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        var tag = request.CurrentTag;
        if (tag.IsInputElement() && !tag.HasAttr ("name"))
        {
            tag.Attr ("name", request.ElementId);
        }
    }

    #endregion
}
