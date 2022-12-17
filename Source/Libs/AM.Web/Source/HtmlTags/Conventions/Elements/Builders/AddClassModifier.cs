// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AddClassModifier.cs --
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
public class AddClassModifier
    : IElementModifier
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    public AddClassModifier
        (
            string className
        )
    {
        Sure.NotNullNorEmpty (className);

        _className = className;
    }

    #endregion

    #region Private members

    private readonly string _className;

    #endregion

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

        request.CurrentTag.AddClass (_className);
    }

    #endregion
}
