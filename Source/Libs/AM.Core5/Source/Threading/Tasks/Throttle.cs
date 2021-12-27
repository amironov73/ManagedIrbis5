// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* Throttle .cs -- дефолтная реализация дросселя (ограничителя)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Threading.Tasks;

/// <summary>
/// Дефолтная реализация дросселя, т. е. ограничителя
/// пропускания задач.
/// </summary>
/// <remarks>Borrowed from Tom DuPont:
/// http://www.tomdupont.net/2015/02/await-interval-with-throttle-class-in.html
/// </remarks>
public class Throttle
    : IThrottle
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Throttle
        (
            TimeSpan interval
        )
    {
        _interval = interval;
        _nextTime = DateTime.Now.Subtract (interval);
    }

    #endregion

    #region Private members

    private readonly object _lock = new ();

    private readonly TimeSpan _interval;

    private DateTime _nextTime;

    #endregion

    #region Public methods

    /// <inheritdoc cref="IThrottle.GetNext()"/>
    public virtual Task GetNext()
    {
        return GetNext (out _);
    }

    /// <inheritdoc cref="IThrottle.GetNext(out System.TimeSpan)"/>
    public virtual Task GetNext
        (
            out TimeSpan delay
        )
    {
        lock (_lock)
        {
            var now = DateTime.Now;

            _nextTime = _nextTime.Add (_interval);

            if (_nextTime > now)
            {
                delay = _nextTime - now;
                return Task.Delay (delay);
            }

            _nextTime = now;

            delay = TimeSpan.Zero;
            return Task.FromResult (true);
        }
    }

    #endregion
}
