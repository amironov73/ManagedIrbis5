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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    public sealed class Connection
        : ConnectionBase
        /* IDisposable,
        IAsyncDisposable,
        IHandmadeSerializable */
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
        /// Socket.
        /// </summary>
        public ClientSocket Socket { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public CancellationToken Cancellation { get; }

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
            _logger = Magna.Factory.CreateLogger<Connection>();
        }

        #endregion

        #region Private members

        internal ILogger _logger;

        private static readonly int[] _goodCodesForReadRecord = { -201, -600, -602, -603 };
        private static readonly int[] _goodCodesForReadTerms = { -202, -203, -204 };

        private CancellationTokenSource _cancellation;

        /* private bool _debug; */

        private void SetBusy
            (
                bool busy
            )
        {
            if (Busy != busy)
            {
                _logger.LogTrace($"SetBusy{busy}");
                Busy = busy;
                BusyChanged?.Invoke(this, EventArgs.Empty);
            }
        } // method SetBusy


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
        /// Подключение к серверу ИРБИС64.
        /// </summary>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new ValueQuery(this, CommandCode.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = ExecuteSync(ref query);
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
        } // method Connect

        /// <summary>
        /// Подключение к серверу ИРБИС64.
        /// </summary>
        public async Task<bool> ConnectAsync()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new Query(this, CommandCode.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = await ExecuteAsync(query);
            if (response is null)
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
        /// <returns>Признак успешности завершения операции.</returns>
        public bool Disconnect()
        {
            if (Connected)
            {
                var query = new ValueQuery(this, CommandCode.UnregisterClient);
                query.AddAnsi(Username);
                try
                {
                    ExecuteSync(ref query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                Connected = false;
            }

            return true;
        } // method Disconnect

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        /// <returns>Признак успешности завершения операции.</returns>
        public async Task<bool> DisconnectAsync()
        {
            if (Connected)
            {
                var query = new Query(this, CommandCode.UnregisterClient);
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
        public override async Task<Response?> ExecuteAsync
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
                    /*
                    if (_debug)
                    {
                        query.Debug(Console.Out);
                    }
                    */

                    result = await Socket.TransactAsync(query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return null;
                }

                if (result is not null)
                {
                    /*
                    if (_debug)
                    {
                        result.Debug(Console.Out);
                    }
                    */

                    result.Parse();
                }

                QueryId++;

                return result;
            }
            finally
            {
                SetBusy(false);
            }
        } // method ExecuteAsync

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public override Response? ExecuteSync
            (
                ref ValueQuery query
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
                    /*
                    if (_debug)
                    {
                        query.Debug(Console.Out);
                    }
                    */

                    result = Socket.TransactSync(ref query);
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

                /*
                if (_debug)
                {
                    result.Debug(Console.Out);
                }
                */

                result.Parse();
                QueryId++;

                return result;
            }
            finally
            {
                SetBusy(false);
            }
        } // method ExecuteSync

        /*
        /// <summary>
        /// Получение списка баз данных с сервера.
        /// </summary>
        /// <param name="fileName">Имя файла со списком баз данных.</param>
        /// <returns>Массив описаний баз данных.</returns>
        public async Task<DatabaseInfo[]> ListDatabasesAsync
            (
                string fileName
            )
        {
            if (!CheckConnection())
            {
                return Array.Empty<DatabaseInfo>();
            }

            return Array.Empty<DatabaseInfo>();
        } // method ListDatabasesAsync
        */

        /// <summary>
        /// Восстановление ранее прикрытого с помощью <see cref="Suspend"/>
        /// соединения.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Connection Restore
            (
                string state
            )
        {
            throw new NotImplementedException();

            /*
            ConnectionSettings settings = ConnectionSettings.Decrypt(state);

            if (ReferenceEquals(settings, null))
            {
                throw new IrbisException
                (
                    "Decrypted state is null"
                );
            }

            var result = new IrbisConnection();
            settings.ApplyToConnection(result);

            return result;
            */
        } // method Restore

        /// <summary>
        /// Восстановление флага подключения, ранее погашенного
        /// при помощи <see cref="Suspend"/>.
        /// </summary>
        public void Rise()
        {
            throw new NotImplementedException();
        } // method Rise


        /// <summary>
        /// Временно "закрывает" соединение с сервером
        /// (на самом деле соединение не разрывается)
        /// и сериализует его состояние в строку
        /// с возможностью последующего восстановления.
        /// </summary>
        /// <returns></returns>
        public string Suspend()
        {
            throw new NotImplementedException();
        } // method Suspend


        #endregion

        #region IHandmadeSerializable members

        /*

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Host = reader.ReadNullableString()
                .ThrowIfNull("Host");
            Port = (ushort) reader.ReadPackedInt32();

            var username = reader.ReadNullableString();
            if (!string.IsNullOrEmpty(username))
            {
                Username = username;
            }

            var password = reader.ReadNullableString();
            if (!ReferenceEquals(password, null))
            {
                Password = password;
            }

            Database = reader.ReadNullableString()
                .ThrowIfNull("Database");

            var workstation = reader.ReadNullableString();
            if (!string.IsNullOrEmpty(workstation))
            {
                Workstation = workstation;
            }
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Host)
                .WritePackedInt32(Port)
                .WriteNullable(Username)
                .WriteNullable(Password)
                .WriteNullable(Database)
                .WriteNullable(Workstation);
        } // method SaveTiStream

        */

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable"/>

        public override void Dispose()
        {
            if (Connected)
            {
                Disconnect();
            }
        } // method Dispose

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
        } // method DisposeAsync

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            var status = Connected ? "[*]" : "[]";

            return $"{Host} {Database} {Username} {status}";
        } // method ToString

        #endregion

    } // class Connection
}
