// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BusyGuard.cs -- обертка для захвата и освобождения BusyState
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Threading;

/// <summary>
/// Обертка для захвата и освобождения <see cref="BusyState"/>.
/// </summary>
public readonly struct BusyGuard
    : IDisposable
{
    #region Properties

    /// <summary>
    /// State.
    /// </summary>
    public BusyState State { get; }

    /// <summary>
    /// Timeout.
    /// </summary>
    public TimeSpan Timeout { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public BusyGuard
        (
            BusyState state
        )
    {
        State = state;
        Timeout = TimeSpan.Zero;

        _Grab();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public BusyGuard
        (
            BusyState state,
            TimeSpan timeout
        )
    {
        State = state;
        Timeout = timeout;

        _Grab();
    }

    #endregion

    #region Private members

    private void _Grab()
    {
        if (Timeout.IsZeroOrLess())
        {
            State.WaitAndGrab();
        }
        else
        {
            if (!State.WaitAndGrab (Timeout))
            {
                Magna.Logger.LogError
                    (
                        nameof (BusyGuard)
                        + "::"
                        + nameof (_Grab)
                        + ": "
                        + "timeout"
                    );

                throw new TimeoutException();
            }
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        State.SetState (false);
    }

    #endregion
}
