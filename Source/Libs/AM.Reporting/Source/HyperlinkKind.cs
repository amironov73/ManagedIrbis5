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

namespace AM.Reporting;

/// <summary>
/// Specifies the hyperlink type.
/// </summary>
public enum HyperlinkKind
{
    /// <summary>
    /// Specifies the hyperlink to external URL such as "http://www.fast-report.com", "mailto:"
    /// or any other system command.
    /// </summary>
    URL,

    /// <summary>
    /// Specifies hyperlink to a given page number.
    /// </summary>
    PageNumber,

    /// <summary>
    /// Specifies hyperlink to a bookmark.
    /// </summary>
    Bookmark,

    /// <summary>
    /// Specifies hyperlink to external report. This report will be run when you follow the hyperlink.
    /// </summary>
    DetailReport,

    /// <summary>
    /// Specifies hyperlink to this report's page. The page will be run when you follow the hyperlink.
    /// </summary>
    DetailPage,

    /// <summary>
    /// Specifies a custom hyperlink. No actions performed when you click it, you should handle it
    /// in the object's Click event handler.
    /// </summary>
    Custom
}
