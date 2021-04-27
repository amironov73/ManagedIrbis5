// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

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

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Управляет отображением диалога "ИРБИС занят, подождите"
    /// или чего-нибудь аналогичного.
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public sealed class BusyManager
        : Component
    {
        #region Constants

        /// <summary>
        /// Default delay, milliseconds.
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
        [DefaultValue(DelayConstant)]
        public int Delay { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструкторs.
        /// </summary>
        public BusyManager
            (
                ICancellable connection
            )
        {
            Delay = DefaultDelay;
            Connection = connection;
            Busy.StateChanged += _BusyChanged;
            Busy.Disposing += _ConnectionDisposing;
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

        private void _DebugThreadId() =>
            Magna.Debug(nameof(BusyManager)
            + ": Thread name=" + Thread.CurrentThread.Name
            + ", thread ID=" + Environment.CurrentManagedThreadId);

        private void _ConnectionDisposing
            (
                object? sender,
                EventArgs e
            )
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(_ConnectionDisposing) + ": enter");

            _DebugThreadId();
            Dispose(true);

            Magna.Debug(nameof(BusyManager) + "::" + nameof(_ConnectionDisposing) + ": leave");
        }

        private void _BreakPressed
            (
                object? sender,
                EventArgs e
            )
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(_BreakPressed) + ": enter");

            _DebugThreadId();
            Connection.CancelOperation();

            Magna.Debug(nameof(BusyManager) + "::" + nameof(_BreakPressed) + ": leave");
        }

        private void _BusyChanged
            (
                object? sender,
                EventArgs e
            )
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(_BusyChanged) + ": enter");
            Magna.Debug(nameof(BusyManager) + ": Busy=" + Busy);

            _DebugThreadId();

            if (Busy)
            {
                // Началось обращение к серверу
                _counter++;
                _workDone = false;
                _uiThread = new Thread(_UiThreadMethod)
                {
                    IsBackground = true,
                    Name = "IrbisBusyManager" + _counter
                };
                _uiThread.SetApartmentState(ApartmentState.STA);
                _uiThread.Start();
            }
            else
            {
                // Обращение к серверу закончилось
                _workDone = true;
                if (!ReferenceEquals(_waitEvent, null))
                {
                    _waitEvent.Set();
                }
                _FormCleanup();
            }

            Magna.Debug(nameof(BusyManager) + "::" + nameof(_BusyChanged) + ": leave");
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
            Magna.Debug(nameof(BusyManager) + "::" + nameof(_UiThreadMethod) + ": enter");

            _DebugThreadId();
            if (Delay > 0)
            {
                _waitEvent = new ManualResetEventSlim(false);
                _waitEvent.Wait(Delay);
            }

            if (!_workDone)
            {
                try
                {
                    _waitForm = new BusyForm();
                    if (!ReferenceEquals(_formTitle, null))
                    {
                        _waitForm.SetTitle(_formTitle);
                    }

                    if (!ReferenceEquals(_formMessage, null))
                    {
                        _waitForm.SetMessage(_formMessage);
                    }

                    _waitForm.BreakPressed += _BreakPressed;
                    _waitForm.ShowDialog();
                }
                finally
                {
                    _FormCleanup();
                }
            }

            Magna.Debug(nameof(BusyManager) + "::" + nameof(_UiThreadMethod) + ": leave");
        }

        private void _FormCleanup()
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(_FormCleanup) + ": enter");

            _DebugThreadId();

            if (!ReferenceEquals(_waitForm, null))
            {
                if (_waitForm.InvokeRequired)
                {
                    _waitForm.Invoke((MethodInvoker) _FormCleanup);
                }
                else
                {
                    _waitForm.BreakPressed -= _BreakPressed;
                    _waitForm.Close();
                    //_waitForm = null; // не сметь обнулять!!!
                }
            }

            Magna.Debug(nameof(BusyManager) + "::" + nameof(_FormCleanup) + ": leave");
        }

        private void _ClientCleanup()
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(_ClientCleanup) + ": enter");

            _DebugThreadId();
            Busy.StateChanged -= _BusyChanged;
            Busy.Disposing -= _ConnectionDisposing;

            Magna.Debug(nameof(BusyManager) + "::" + nameof(_ClientCleanup) + ": leave");
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
            Magna.Debug(nameof(BusyManager) + "::" + nameof(SetTitle) + ": enter");

            _DebugThreadId();
            _formTitle = title;

            if (!ReferenceEquals(_waitForm, null))
            {
                _waitForm.Invoke((MethodInvoker) (() => _waitForm.SetTitle(title)));
            }

            Magna.Debug(nameof(BusyManager) + "::" + nameof(SetTitle) + ": leave");
        }

        /// <summary>
        /// Установка надписи на форме.
        /// </summary>
        public void SetMessage
            (
                string message
            )
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(SetMessage) + ": enter");

            _DebugThreadId();
            _formMessage = message;

            if (!ReferenceEquals(_waitForm, null))
            {
                _waitForm.Invoke((MethodInvoker) (() => _waitForm.SetMessage(message)));
            }

            Magna.Debug(nameof(BusyManager) + "::" + nameof(SetMessage) + ": leave");
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        protected override void Dispose
            (
                bool disposing
            )
        {
            Magna.Debug(nameof(BusyManager) + "::" + nameof(Dispose) + ": enter: disposing=" + disposing);

            _DebugThreadId();
            _FormCleanup();
            _ClientCleanup();

            base.Dispose(disposing);

            Magna.Debug(nameof(BusyManager) + "::" + nameof(Dispose) + ": leave");
        }

        #endregion

    } // class BusyManager

} //
