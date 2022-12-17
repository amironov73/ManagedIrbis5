// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DefaultElementNamingConvention.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
public class DefaultElementNamingConvention
    : IElementNamingConvention
{
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

        return accessor.Name;
    }

    #endregion
}
