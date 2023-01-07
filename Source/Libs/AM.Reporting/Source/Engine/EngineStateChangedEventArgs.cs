// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EngineStateChangedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Engine;

internal class EngineStateChangedEventArgs
{
    #region Properties

    public ReportEngine Engine { get; }

    public EngineState State { get; }

    #endregion

    #region Constructors

    internal EngineStateChangedEventArgs (ReportEngine engine, EngineState state)
    {
        Engine = engine;
        State = state;
    }

    #endregion
}
