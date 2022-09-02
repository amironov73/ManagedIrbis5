// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlTag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

internal sealed class HtmlTag
{
    #region Properties

    /// <summary>
    /// Gets the name of this tag
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets collection of attributes and their value the html tag has
    /// </summary>
    public Dictionary<string, string>? Attributes { get; }

    /// <summary>
    /// Gets if the tag is single placed; in other words it doesn't have a separate closing tag; <br/>
    /// e.g. &lt;br&gt;
    /// </summary>
    public bool IsSingle { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="name">the name of the html tag</param>
    /// <param name="isSingle">if the tag is single placed; in other words it doesn't have a separate closing tag;</param>
    /// <param name="attributes">collection of attributes and their value the html tag has</param>
    public HtmlTag
        (
            string name,
            bool isSingle,
            Dictionary<string, string>? attributes = null
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;
        IsSingle = isSingle;
        Attributes = attributes;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// is the html tag has attributes.
    /// </summary>
    /// <returns>true - has attributes, false - otherwise</returns>
    public bool HasAttributes()
    {
        return Attributes is { Count: > 0 };
    }

    /// <summary>
    /// Gets a boolean indicating if the attribute list has the specified attribute
    /// </summary>
    /// <param name="attribute">attribute name to check if exists</param>
    /// <returns>true - attribute exists, false - otherwise</returns>
    public bool HasAttribute
        (
            string attribute
        )
    {
        Sure.NotNullNorEmpty (attribute);

        return Attributes?.ContainsKey (attribute) ?? false;
    }

    /// <summary>
    /// Get attribute value for given attribute name or null if not exists.
    /// </summary>
    /// <param name="attribute">attribute name to get by</param>
    /// <param name="defaultValue">optional: value to return if attribute is not specified</param>
    /// <returns>attribute value or null if not found</returns>
    public string? TryGetAttribute
        (
            string attribute,
            string? defaultValue = null
        )
    {
        Sure.NotNullNorEmpty (attribute);

        return Attributes?.ContainsKey (attribute) ?? false
            ? Attributes[attribute]
            : defaultValue;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return string.Create
            (
                CultureInfo.InvariantCulture,
                $"<{Name}>"
            );
    }

    #endregion
}
