// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PeekableEnumerator.cs -- перечислитель с возможностью подглядывания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linq;

/// <summary>
/// Перечислитель с возможностью "подглядывания" на один элемент
/// последовательности вперед.
/// </summary>
/// <remarks>
/// Заимствовано из https://gist.github.com/Denis535/1c5a6eb76a2eb79e3061721855d3e03f
/// </remarks>
public class PeekableEnumerator<T> : IEnumerator<T>
{
    private enum State_
    {
        Uninitialized,
        Started,
        Finished
    }

    private IEnumerator<T> Source { get; }
    private State_ State { get; set; }
    T IEnumerator<T>.Current => Current.Value;
    object? IEnumerator.Current => Current.Value;

    /// <summary>
    ///
    /// </summary>
    public bool IsStarted => State == State_.Started;

    /// <summary>
    ///
    /// </summary>
    public bool IsFinished => State == State_.Finished;

    /// <summary>
    ///
    /// </summary>
    public Option<T> Current { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public Option<T> Next { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public PeekableEnumerator (IEnumerator<T> source)
    {
        Source = source ?? throw new ArgumentNullException (nameof (source));
        State = State_.Uninitialized;
        Current = default;
        Next = default;
    }

    /// <summary>
    ///
    /// </summary>
    public void Dispose()
    {
        Source.Dispose();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        Current = MoveNext (Next, Source);
        Next = default;
        State = Current.HasValue ? State_.Started : State_.Finished;
        return Current.HasValue;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool MoveNext (out T? value)
    {
        if (MoveNext())
        {
            value = Current.Value;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool PeekNext()
    {
        Next = MoveNext (Next, Source);
        return Next.HasValue;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool PeekNext (out T? value)
    {
        if (PeekNext())
        {
            value = Next.Value;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void Reset()
    {
        Source.Reset();
        State = State_.Uninitialized;
        Current = default;
        Next = default;
    }

    // Helpers
    private static Option<T> MoveNext (Option<T> next, IEnumerator<T> enumerator)
    {
        return next.HasValue ? next.Value : MoveNext (enumerator);
    }

    private static Option<T> MoveNext (IEnumerator<T> enumerator)
    {
        var hasValue = enumerator.MoveNext();
        return hasValue ? (Option<T>)enumerator.Current : default;
    }
}
