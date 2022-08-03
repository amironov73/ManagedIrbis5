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

public enum PageProgressionDirection
{
    DEFAULT = 1,
    LEFT_TO_RIGHT,
    RIGHT_TO_LEFT,
    UNKNOWN
}

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name",
    Justification = "Enum and parser need to be close to each other to avoid issues when the enum was changed without changing the parser. The file needs to be named after enum.")]
internal static class PageProgressionDirectionParser
{
    public static PageProgressionDirection Parse(string stringValue)
    {
        switch (stringValue.ToLowerInvariant())
        {
            case "default":
                return PageProgressionDirection.DEFAULT;
            case "ltr":
                return PageProgressionDirection.LEFT_TO_RIGHT;
            case "rtl":
                return PageProgressionDirection.RIGHT_TO_LEFT;
            default:
                return PageProgressionDirection.UNKNOWN;
        }
    }
}