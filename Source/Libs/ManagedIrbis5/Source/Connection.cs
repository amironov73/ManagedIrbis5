// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Connection.cs -- подключение к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    public sealed class Connection
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Fired when <see cref="Busy"/> changed.
        /// </summary>
        public event EventHandler? BusyChanged;

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string Host { get; set; } = "127.0.0.1";

        /// <summary>
        ///
        /// </summary>
        public int Port { get; set; } = 6666;

        /// <summary>
        ///
        /// </summary>
        public string Username { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string Database { get; set; } = "IBIS";

        /// <summary>
        ///
        /// </summary>
        public string Workstation { get; set; } = "C";

        /// <summary>
        ///
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int QueryId { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string? ServerVersion { get; private set; }

        // /// <summary>
        // ///
        // /// </summary>
        // public IniFile? IniFile { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Socket.
        /// </summary>
        public ClientSocket Socket { get; private set; }

        /// <summary>
        /// Busy?
        /// </summary>
        public bool Busy { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public CancellationToken Cancellation { get; }

        /// <summary>
        /// Last error code.
        /// </summary>
        public int LastError { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Connection
            (
                ClientSocket socket
            )
        {
            Socket = socket;
            socket.Connection = this;
            _cancellation = new CancellationTokenSource();
            Cancellation = _cancellation.Token;
        }

        #endregion

        #region Private members

        private static readonly int[] _goodCodesForReadRecord = { -201, -600, -602, -603 };
        private static readonly int[] _goodCodesForReadTerms = { -202, -203, -204 };

        private CancellationTokenSource _cancellation;

        private bool _debug = false;

        private void SetBusy
            (
                bool busy
            )
        {
            Busy = busy;
            BusyChanged?.Invoke(this, EventArgs.Empty);
        } // method SetBusy


        private bool CheckConnection()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;
        } // method CheckConnection

        #endregion

        #region Public methods

        /// <summary>
        /// Cancel the current operation.
        /// </summary>
        public void CancelOperation()
        {
            _cancellation.Cancel();
        } // method CancelOperation

        /// <summary>
        ///
        /// </summary>
        public async Task<bool> ConnectAsync()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN:
            ClientId = new Random().Next(100000, 999999);
            QueryId = 1;
            var query = new Query(this, "A");
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return false;
            }

            if (response.GetReturnCode() == -3337)
            {
                goto AGAIN;
            }

            if (response.ReturnCode < 0)
            {
                return false;
            }

            Connected = true;
            ServerVersion = response.ServerVersion;
            Interval = response.ReadInteger();
            // TODO Read INI-file

            return true;
        } // method ConnectAsync

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> DisconnectAsync()
        {
            if (Connected)
            {
                var query = new Query(this, "B");
                query.AddAnsi(Username);
                try
                {
                    await ExecuteAsync(query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                Connected = false;
            }

            return true;
        } // method DisconnectAsync

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                Query query
            )
        {
            SetBusy(true);
            try
            {
                if (_cancellation.IsCancellationRequested)
                {
                    _cancellation = new CancellationTokenSource();
                }

                Response? result;
                try
                {
                    if (_debug)
                    {
                        query.Debug(Console.Out);
                    }

                    result = await Socket.TransactAsync(query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return null;
                }

                if (ReferenceEquals(result, null))
                {
                    return null;
                }

                if (_debug)
                {
                    result.Debug(Console.Out);
                }

                result.Parse();
                QueryId++;

                return result;
            }
            finally
            {
                SetBusy(false);
            }
        } // method ExecuteAsync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                string command,
                params object[] args
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new Query(this, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg?.ToString());
            }

            var result = await ExecuteAsync(query);

            return result;
        } // method ExecuteAsync

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        /// <param name="database">Опциональное имя базы данных
        /// (<c>null</c> означает текущую базу данных).</param>
        /// <returns>Макисмальный MFN.</returns>
        public async Task<int> GetMaxMfnAsync
            (
                string? database = default
            )
        {
            if (!CheckConnection())
            {
                return 0;
            }

            database ??= Database;
            var query = new Query(this, "O");
            query.AddAnsi(database);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return 0;
            }

            if (!response.CheckReturnCode())
            {
                return 0;
            }

            return response.ReturnCode;
        } // method GetMaxMfnAsync

        /// <summary>
        /// Пустая операция.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> NopAsync()
        {
            if (!CheckConnection())
            {
                return false;
            }

            var response = await ExecuteAsync("N");
            if (ReferenceEquals(response, null))
            {
                return false;
            }

            return response.CheckReturnCode();
        } // method NopAsync

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            if (Connected)
            {
                DisconnectAsync().GetAwaiter().GetResult();
            }
        }

        #endregion

    } // class Connection
}
