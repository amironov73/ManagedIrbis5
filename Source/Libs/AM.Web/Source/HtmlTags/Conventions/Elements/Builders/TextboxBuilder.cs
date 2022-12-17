// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TextboxBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
public class TextboxBuilder
    : IElementBuilder
{
    #region IElementBuilder

    /// <inheritdoc cref="ITagBuilder.Build"/>
    public HtmlTag Build
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        return new TextboxTag().Attr
            (
                "value",
                (request.RawValue ?? string.Empty).ToString()!
            );
    }

    #endregion
}
