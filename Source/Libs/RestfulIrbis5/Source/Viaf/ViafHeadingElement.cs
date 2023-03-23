// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafHeadingElement.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace RestfulIrbis.Viaf;

/// <summary>
///
/// </summary>
public class ViafHeadingElement
{
    #region Public methods

    /// <summary>
    /// Parse the object.
    /// </summary>
    public static ViafHeadingElement Parse
        (
            JObject obj
        )
    {
        throw new NotImplementedException();

        // return new ViafHeadingElement
        // {
        // };
    }

    /// <summary>
    /// Parse the array.
    /// </summary>
    public static ViafHeadingElement[] Parse
        (
            JArray? array
        )
    {
        if (array is null)
        {
            return Array.Empty<ViafHeadingElement>();
        }

        var result = new ViafHeadingElement[array.Count];
        for (var i = 0; i < array.Count; i++)
        {
            result[i] = Parse ((JObject) array[i]);
        }

        return result;
    }

    #endregion
}
