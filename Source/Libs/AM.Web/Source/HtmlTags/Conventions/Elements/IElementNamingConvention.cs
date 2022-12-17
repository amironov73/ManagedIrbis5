// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IElementNamingConvention.cs --
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
public interface IElementNamingConvention
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="modelType"></param>
    /// <param name="accessor"></param>
    /// <returns></returns>
    string GetName
        (
            Type modelType,
            Accessor accessor
        );
}
