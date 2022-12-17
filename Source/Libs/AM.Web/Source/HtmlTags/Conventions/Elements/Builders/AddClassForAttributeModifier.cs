// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AddClassForAttributeModifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

namespace AM.HtmlTags.Conventions.Elements.Builders;

#region Using directives

using Reflection;

#endregion

#nullable enable

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
[PublicAPI]
public class AddClassForAttributeModifier<T>
    : IElementModifier where T : Attribute
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    public AddClassForAttributeModifier
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
    public bool Matches
        (
            ElementRequest token
        )
    {
        Sure.NotNull (token);

        return token.Accessor.HasAttribute<T>();
    }

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

#endregion
