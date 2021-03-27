// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncConnection.cs -- синхронное подключение к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.PlatformAbstraction;
using AM.Runtime;
using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Синхронное подключение к серверу ИРБИС64.
    /// </summary>
    public sealed class SyncConnection
        : ISyncConnection
    {
        #region Events

        /// <summary>
        /// Fired when <see cref="Busy"/> changed.
        /// </summary>
        public event EventHandler? BusyChanged;

        #endregion

        #region Properties

        /// <inheritdoc cref="IIrbisConnectionSettings.Host"/>
        public string Host { get; set; } = "127.0.0.1";

        /// <inheritdoc cref="IIrbisConnectionSettings.Port"/>
        public ushort Port { get; set; } = 6666;

        /// <inheritdoc cref="IIrbisConnectionSettings.Username"/>
        public string Username { get; set; } = string.Empty;

        /// <inheritdoc cref="IIrbisConnectionSettings.Password"/>
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc cref="IBasicIrbisProvider.Database"/>
        public string Database { get; set; } = "IBIS";

        /// <summary>
        ///
        /// </summary>
        public string Workstation { get; set; } = "C";

        /// <summary>
        ///
        /// </summary>
        public int ClientId { get; protected internal set; }

        /// <summary>
        ///
        /// </summary>
        public int QueryId { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public bool Connected { get; internal set; } = false;

        /// <summary>
        /// Busy?
        /// </summary>
        public bool Busy { get; internal set; } = false;

        /// <summary>
        /// Last error code.
        /// </summary>
        public int LastError { get; internal set; } = 0;

        /// <summary>
        ///
        /// </summary>
        public CancellationToken Cancellation { get; }

        // TODO Implement properly

        /// <summary>
        /// Версия клиента.
        /// </summary>
        public static readonly Version ClientVersion = Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version
            ?? throw new ApplicationException("ClientVersion not defined");

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
        public ISyncClientSocket Socket { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SyncConnection
            (
                ISyncClientSocket socket,
                IServiceProvider provider
            )
        {
            Socket = socket;
            socket.Connection = this;
            _cancellation = new CancellationTokenSource();
            Cancellation = _cancellation.Token;
            _logger = Magna.Factory.CreateLogger<IBasicIrbisProvider>();
            _provider = provider;
        }

        #endregion

        #region Private members

        internal ILogger _logger;

        internal IServiceProvider _provider;

        internal CancellationTokenSource _cancellation;

        private static readonly int[] _goodCodesForReadRecord = { -201, -600, -602, -603 };
        private static readonly int[] _goodCodesForReadTerms = { -202, -203, -204 };

        internal void SetBusy
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

        #region ISyncConnection members

        /// <summary>
        /// Cancel the current operation.
        /// </summary>
        public void CancelOperation()
        {
            _cancellation.Cancel();
        } // method CancelOperation

        /// <summary>
        /// Проверка, установлено ли соединение.
        /// </summary>
        /// <returns></returns>
        public bool CheckProviderState()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;
        } // method CheckConnection

        /// <inheritdoc cref="ISyncConnection.Connect"/>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new SyncQuery(this, CommandCode.RegisterClient);
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
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public Response? ExecuteSync
            (
                ref SyncQuery query
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

        #endregion

        #region IAsyncDisposable members

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType) =>
            _provider.GetService(serviceType);

        #endregion

        #region Object members

        #endregion

        #region Temp

        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        }

        public PlatformAbstractionLayer PlatformAbstraction { get; set; }
        public IPftFormatter AcquireFormatter()
        {
            throw new NotImplementedException();
        }

        public void Configure(string configurationString)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(FileSpecification file)
        {
            throw new NotImplementedException();
        }

        public TermLink[] ExactSearchLinks(string term)
        {
            throw new NotImplementedException();
        }

        public TermLink[] ExactSearchTrimLinks(string term, int i)
        {
            throw new NotImplementedException();
        }

        public string FormatRecord(string format, Record record)
        {
            throw new NotImplementedException();
        }

        public string FormatRecord(string format, int mfn)
        {
            throw new NotImplementedException();
        }

        public string[] FormatRecords(int[] mfns, string format)
        {
            throw new NotImplementedException();
        }

        public int GetMaxMfn(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public ServerVersion GetServerVersion()
        {
            throw new NotImplementedException();
        }

        public string[] ListFiles(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool NoOperation()
        {
            throw new NotImplementedException();
        }

        public MenuFile? ReadMenuFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Record ReadRecord(int mfn)
        {
            throw new NotImplementedException();
        }

        public void ReleaseFormatter(IPftFormatter formatter)
        {
            throw new NotImplementedException();
        }

        public int[] Search(string expression)
        {
            throw new NotImplementedException();
        }

        public Record? ReadRecordVersion(int mfn, int version)
        {
            throw new NotImplementedException();
        }

        public Term[] ReadTerms(TermParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool FileExist(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public string GetGeneration()
        {
            throw new NotImplementedException();
        }

        public IniFile GetUserIniFile()
        {
            throw new NotImplementedException();
        }

        public void WriteRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public void ParseConnectionString(string connectionString)
        {
            throw new NotImplementedException();
        }

        public string? ReadTextFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public GblResult GlobalCorrection(GblSettings settings)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
