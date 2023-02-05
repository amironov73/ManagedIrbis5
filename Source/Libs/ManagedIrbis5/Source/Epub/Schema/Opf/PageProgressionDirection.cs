// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
/// 
/// </summary>
public enum PageProgressionDirection
{
    /// <summary>
    /// 
    /// </summary>
    DEFAULT = 1,

    /// <summary>
    /// 
    /// </summary>
    LEFT_TO_RIGHT,

    /// <summary>
    /// 
    /// </summary>
    RIGHT_TO_LEFT,

    /// <summary>
    /// 
    /// </summary>
    UNKNOWN
}

[SuppressMessage ("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name",
    Justification =
        "Enum and parser need to be close to each other to avoid issues when the enum was changed without changing the parser. The file needs to be named after enum.")]
internal static class PageProgressionDirectionParser
{
    public static PageProgressionDirection Parse 
        (
            string stringValue
        )
    {
        return stringValue.ToLowerInvariant() switch
        {
            "default" => PageProgressionDirection.DEFAULT,
            "ltr" => PageProgressionDirection.LEFT_TO_RIGHT,
            "rtl" => PageProgressionDirection.RIGHT_TO_LEFT,
            _ => PageProgressionDirection.UNKNOWN
        };
    }
}
