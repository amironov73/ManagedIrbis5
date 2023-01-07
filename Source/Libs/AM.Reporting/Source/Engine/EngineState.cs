// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EngineState.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Engine;

internal enum EngineState
{
    ReportStarted,
    ReportFinished,
    ReportPageStarted,
    ReportPageFinished,
    PageStarted,
    PageFinished,
    ColumnStarted,
    ColumnFinished,
    BlockStarted,
    BlockFinished,
    GroupStarted,
    GroupFinished
}
