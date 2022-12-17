// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DotNotationElementNamingConvention.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
public class DotNotationElementNamingConvention
    : IElementNamingConvention
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static Func<string, bool> IsCollectionIndexer = x => x.StartsWith ("[") && x.EndsWith ("]");

    #endregion

    #region IElementNamingConvention members

    /// <inheritdoc cref="IElementNamingConvention.GetName"/>
    public string GetName
        (
            Type modelType,
            Accessor accessor
        )
    {
        Sure.NotNull (accessor);
        modelType.NotUsed();

        return accessor.PropertyNames
            .Aggregate ((x, y) =>
            {
                var formatString = IsCollectionIndexer (y)
                    ? "{0}{1}"
                    : "{0}.{1}";
                return string.Format (formatString, x, y);
            });
    }

    #endregion
}
