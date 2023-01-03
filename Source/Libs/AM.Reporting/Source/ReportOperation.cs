// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ReportOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// Specifies the report operation.
/// </summary>
public enum ReportOperation
{
    /// <summary>
    /// Specifies no operation.
    /// </summary>
    None,

    /// <summary>
    /// The report is running.
    /// </summary>
    Running,

    /// <summary>
    /// The report is printing.
    /// </summary>
    Printing,

    /// <summary>
    /// The report is exporting.
    /// </summary>
    Exporting
}
