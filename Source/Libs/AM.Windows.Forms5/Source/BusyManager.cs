// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

/* BusyManager.cs -- управляет отображением диалога "ИРБИС занят"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using AM;
using AM.Threading;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms;

/// <summary>
/// Управляет отображением диалога "ИРБИС занят, подождите"
/// или чего-нибудь аналогичного.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public sealed class BusyManager
    : Component
{
    #region Constants

    /// <summary>
    /// Задержка по умолчанию, миллисекунды.
    /// </summary>
    public const int DelayConstant = 100;

    #endregion

    #region Properties

    /// <summary>
    /// Значение задержки по умолчанию.
    /// </summary>
    public static int DefaultDelay = DelayConstant;

    /// <summary>
    /// Занят ли сейчас клиент обращением к ИРБИС-серверу.
    /// </summary>
    public BusyState Busy => Connection.Busy;

    /// <summary>
    /// Ссылка на потенциально занятой объект.
    /// </summary>
    public ICancellable Connection { get; }

    /// <summary>
    /// Задержка между началом запроса к серверу
    /// и появлением окна "Ждите", миллисекунды.
    /// Нулевое или отрицательное значение - нет задержки.
    /// </summary>
    [DefaultValue (DelayConstant)]
    public int Delay { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BusyManager
        (
            ICancellable connection
        )
    {
        Sure.NotNull (connection);

        Delay = DefaultDelay;
        Connection = connection;
        Busy.StateChanged += OnBusyChanged;
        Busy.Disposing += OnConnectionDisposing;
    }

    #endregion

    #region Private members

    private Thread? _uiThread;
    private BusyForm? _waitForm;
    private bool _workDone;
    private ManualResetEventSlim? _waitEvent;
    private static int _counter;
    private static string? _formTitle;
    private static string? _formMessage;

    private void _DebugThreadId() => Magna.Logger.LogDebug
        (
            nameof (BusyManager) + ": Thread name={ThreadName}, thread ID={ThreadId}",
            Thread.CurrentThread.Name,
            Environment.CurrentManagedThreadId
        );

    private void OnConnectionDisposing
        (
            object? sender,
            EventArgs e
        )
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (OnConnectionDisposing) + ": enter");
        _DebugThreadId();

        Dispose (true);

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (OnConnectionDisposing) + ": leave");
    }

    private void OnBreakPressed
        (
            object? sender,
            EventArgs e
        )
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (OnBreakPressed) + ": enter");
        _DebugThreadId();

        Connection.CancelOperation();

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (OnBreakPressed) + ": leave");
    }

    private void OnBusyChanged
        (
            object? sender,
            EventArgs e
        )
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (OnBusyChanged) + ": enter, busy={Busy}", Busy);
        _DebugThreadId();

        if (Busy)
        {
            // Началось обращение к серверу
            _counter++;
            _workDone = false;
            _uiThread = new Thread (_UiThreadMethod)
            {
                IsBackground = true,
                Name = "IrbisBusyManager" + _counter
            };
            _uiThread.SetApartmentState (ApartmentState.STA);
            _uiThread.Start();
        }
        else
        {
            // Обращение к серверу закончилось
            _workDone = true;
            _waitEvent?.Set();
            _FormCleanup();
        }

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (OnBusyChanged) + ": leave");
    }

    /// <summary>
    /// Выполняется в отдельном потоке.
    /// Обратите внимание: созданная форма имеет
    /// отдельную очередь сообщений, поэтому не блокируется.
    /// Основной пользовательский интерфейс при этом
    /// может блокироваться клиентом.
    /// </summary>
    private void _UiThreadMethod()
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (_UiThreadMethod) + ": enter");
        _DebugThreadId();

        if (Delay > 0)
        {
            _waitEvent = new ManualResetEventSlim (false);
            _waitEvent.Wait (Delay);
        }

        if (!_workDone)
        {
            try
            {
                _waitForm = new BusyForm();
                if (!string.IsNullOrEmpty (_formTitle))
                {
                    _waitForm.SetTitle (_formTitle);
                }

                if (!string.IsNullOrEmpty (_formMessage))
                {
                    _waitForm.SetMessage (_formMessage);
                }

                _waitForm.BreakPressed += OnBreakPressed;
                _waitForm.ShowDialog();
            }
            finally
            {
                _FormCleanup();
            }
        }

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (_UiThreadMethod) + ": leave");
    }

    private void _FormCleanup()
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (_FormCleanup) + ": enter");
        _DebugThreadId();

        if (!ReferenceEquals (_waitForm, null))
        {
            if (_waitForm.InvokeRequired)
            {
                _waitForm.Invoke ((MethodInvoker)_FormCleanup);
            }
            else
            {
                _waitForm.BreakPressed -= OnBreakPressed;
                _waitForm.Close();

                //_waitForm = null; // не сметь обнулять!!!
            }
        }

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (_FormCleanup) + ": leave");
    }

    private void _ClientCleanup()
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (_ClientCleanup) + ": enter");
        _DebugThreadId();

        Busy.StateChanged -= OnBusyChanged;
        Busy.Disposing -= OnConnectionDisposing;

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (_ClientCleanup) + ": leave");
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Установка заголовка окна.
    /// </summary>
    public void SetTitle
        (
            string title
        )
    {
        Sure.NotNull (title);
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (SetTitle) + ": enter");
        _DebugThreadId();

        _formTitle = title;
        _waitForm?.Invoke ((MethodInvoker)(() => _waitForm.SetTitle (title)));

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (SetTitle) + ": leave");
    }

    /// <summary>
    /// Установка надписи на форме.
    /// </summary>
    public void SetMessage
        (
            string message
        )
    {
        Sure.NotNull (message);
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (SetMessage) + ": enter");

        _DebugThreadId();
        _formMessage = message;
        _waitForm?.Invoke ((MethodInvoker)(() => _waitForm.SetMessage (message)));

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (SetMessage) + ": leave");
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="Component.Dispose(bool)" />
    protected override void Dispose
        (
            bool disposing
        )
    {
        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (Dispose) + ": enter: disposing={Disposing}", disposing);
        _DebugThreadId();

        _FormCleanup();
        _ClientCleanup();

        base.Dispose (disposing);

        Magna.Logger.LogDebug (nameof (BusyManager) + "::" + nameof (Dispose) + ": leave");
    }

    #endregion
}
