// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TimeMeasure.cs -- измерение продолжительности операции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Измерение продолжительности операции.
/// </summary>
public sealed class TimeMeasure
    : IDisposable
{
    #region Properties

    /// <inheritdoc cref="Stopwatch.ElapsedTicks"/>
    public long ElapsedTicks => _stopwatch.ElapsedTicks;

    /// <inheritdoc cref="Stopwatch.ElapsedMilliseconds"/>
    public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

    /// <inheritdoc cref="Stopwatch.Elapsed"/>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TimeMeasure()
    {
        _stopwatch.Start();
    }

    #endregion

    #region Private members

    private readonly Stopwatch _stopwatch = new();

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (_stopwatch.IsRunning)
        {
            _stopwatch.Stop();
        }
    }

    #endregion
}
