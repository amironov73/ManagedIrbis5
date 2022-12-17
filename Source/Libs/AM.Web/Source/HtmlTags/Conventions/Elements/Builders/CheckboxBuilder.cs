// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CheckboxBuilder.cs --
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
public class CheckboxBuilder
    : ElementTagBuilder
{
    #region TagBuilder members

    /// <inheritdoc cref="TagBuilder.Matches"/>
    public override bool Matches
        (
            ElementRequest subject
        )
    {
        return subject?.Accessor?.PropertyType == typeof (bool);
    }

    /// <inheritdoc cref="TagBuilder.Build"/>
    public override HtmlTag Build
        (
            ElementRequest request
        )
    {
        return new CheckboxTag (request?.RawValue?.As<bool>() ?? false);
    }

    #endregion
}
