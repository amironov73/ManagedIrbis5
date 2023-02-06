// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BusyState.cs -- флаг, сигнализирующий о занятости ресурса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Threading;

/// <summary>
/// Флаг, сигнализирующий о занятости некоторого ресурса.
/// </summary>
public sealed class BusyState
    : IHandmadeSerializable,
    IDisposable
{
    #region Events

    /// <summary>
    /// Событие, возникающее при изменении состояния занятости ресурса.
    /// </summary>
    public event EventHandler? StateChanged;

    /// <summary>
    /// Событие, возникающее при очистке.
    /// </summary>
    public event EventHandler? Disposing;

    #endregion

    #region Properties

    /// <summary>
    /// Собственно состояние.
    /// </summary>
    public bool State => !_waitHandle.IsSet;

    /// <summary>
    /// Использовать асинхронный обработчик события?
    /// </summary>
    public bool UseAsync { get; set; }

    /// <summary>
    /// Хэндл для ожидания.
    /// </summary>
    public WaitHandle WaitHandle => _waitHandle.WaitHandle;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// Начальное состояние "ресурс не занят".
    /// </summary>
    public BusyState()
    {
        _waitHandle = new ManualResetEventSlim (true);
    }

    #endregion

    #region Private members

    private readonly ManualResetEventSlim _waitHandle;

    #endregion

    #region Public methods

    /// <summary>
    /// Run some code.
    /// </summary>
    public void Run
        (
            Action action
        )
    {
        WaitAndGrab();

        try
        {
            action();
        }
        finally
        {
            SetState (false);
        }
    }

    /// <summary>
    /// Run some code in asychronous manner.
    /// </summary>
    public Task RunAsync
        (
            Action action
        )
    {
        var result = Task.Factory.StartNew
            (
                () => Run (action)
            );

        return result;
    }

    /// <summary>
    /// Change the state.
    /// </summary>
    public void SetState
        (
            bool newState
        )
    {
        Magna.Logger.LogTrace
            (
                nameof (BusyState) + "::" + nameof (SetState) + ": newState={NewState}",
                newState
            );

        if (newState != State)
        {
            if (newState)
            {
                // считаемся занятыми
                _waitHandle.Reset();
            }
            else
            {
                // считаемся свободными
                _waitHandle.Set();
            }


            if (UseAsync)
            {
                StateChanged.RaiseAsync (this);
            }
            else
            {
                StateChanged.Raise (this);
            }
        }
    }

    /// <summary>
    /// Ожидаем, пока не освободится, и захватываем.
    /// </summary>
    public void WaitAndGrab()
    {
        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (WaitAndGrab) + ": enter");

        WaitHandle.WaitOne();
        SetState (true);

        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (WaitAndGrab) + ": return");
    }

    /// <summary>
    /// Ожидаем, пока не освободится, затем захватываем.
    /// </summary>
    public bool WaitAndGrab
        (
            TimeSpan timeout
        )
    {
        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (WaitAndGrab) + ": enter");

        var result = WaitHandle.WaitOne (timeout);
        SetState (true);

        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (WaitAndGrab) + ": return");

        return result;
    }

    /// <summary>
    /// Ожидаем, пока не освободится.
    /// </summary>
    public void Wait()
    {
        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (Wait) + ": enter");

        WaitHandle.WaitOne();

        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (Wait) + ": return");
    }

    /// <summary>
    /// Ожидаем, пока не освободится.
    /// </summary>
    public bool Wait
        (
            TimeSpan timeout
        )
    {
        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (Wait) + ": enter");

        var result = WaitHandle.WaitOne (timeout);

        Magna.Logger.LogTrace (nameof (BusyState) + "::" + nameof (Wait) + ": return");

        return result;
    }

    /// <summary>
    /// Оператор неявного преобразования типа.
    /// </summary>
    public static implicit operator bool
        (
            BusyState state
        )
    {
        return state.State;
    }

    /// <summary>
    /// Оператор неявного преобразования типа.
    /// </summary>
    public static implicit operator BusyState
        (
            bool value
        )
    {
        var result = new BusyState();
        result.SetState (value);

        return result;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        SetState (reader.ReadBoolean());
        UseAsync = reader.ReadBoolean();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        writer.Write (State);
        writer.Write (UseAsync);
    }

    #endregion

    #region IDisposable methods

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Disposing.Raise (this);
        _waitHandle.Dispose();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => State.ToString();

    #endregion
}
