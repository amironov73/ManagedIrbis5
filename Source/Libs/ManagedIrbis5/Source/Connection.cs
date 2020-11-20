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
using AM;
using AM.Collections;

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
        public ushort Port { get; set; } = 6666;

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
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> ActualizeDatabaseAsync
            (
                string? database = default
            )
        {
            var result = await ActualizeRecordAsync(database, 0);

            return result;
        } // method ActualizeDatabase

        /// <summary>
        /// Актуализация записи с указанным MFN.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN, подлежащий актуализации.</param>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> ActualizeRecordAsync
            (
                string? database,
                int mfn
            )
        {
            database ??= Database;
            var response = await ExecuteAsync
                (
                    CommandCode.ActualizeRecord,
                    database,
                    mfn
                );

            return !ReferenceEquals(response, null);
        } // method ActualizeRecordAsync


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
        /// Создание базы данных.
        /// </summary>
        /// <param name="database">Имя создаваемой базы.</param>
        /// <param name="description">Описание в свободной форме.</param>
        /// <param name="readerAccess">Читатель будет иметь доступ?</param>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> CreateDatabaseAsync
            (
                string database,
                string description,
                bool readerAccess = true
            )
        {
            if (!CheckConnection())
            {
                return false;
            }

            var query = new Query(this, CommandCode.CreateDatabase);
            query.AddAnsi(database).NewLine();
            query.AddAnsi(description).NewLine();
            query.Add(readerAccess ? 1 : 0).NewLine();
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return false;
            }

            if (!response.CheckReturnCode())
            {
                return false;
            }

            return true;
        } // method CreateDatabaseAsync


        /// <summary>
        /// Создание словаря в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> CreateDictionaryAsync
            (
                string? database = default
            )
        {
            if (!CheckConnection())
            {
                return false;
            }

            database ??= Database;
            var query = new Query(this, CommandCode.CreateDictionary);
            query.AddAnsi(database).NewLine();
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return false;
            }

            if (!response.CheckReturnCode())
            {
                return false;
            }

            return true;
        } // method CreateDictionaryAsync

        /// <summary>
        /// Удаление указанной базы данных.
        /// </summary>
        /// <param name="database">Имя удаляемой базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> DeleteDatabaseAsync
            (
                string? database = default
            )
        {
            if (!CheckConnection())
            {
                return false;
            }

            database ??= Database;
            var query = new Query(this, CommandCode.DeleteDatabase);
            query.AddAnsi(database).NewLine();
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return false;
            }

            if (!response.CheckReturnCode())
            {
                return false;
            }

            return true;

        } // method DeleteDatabaseAsync

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
        /// Форматирование записи с указанием её MFN.
        /// </summary>
        /// <param name="format">Спецификация формата.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <returns>Результат расформатирования.</returns>
        public async Task<string?> FormatRecordAsync
            (
                string format,
                int mfn
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new Query(this, CommandCode.FormatRecord);
            query.AddAnsi(Database);
            var prepared = IrbisFormat.PrepareFormat(format);
            query.AddAnsi(prepared);
            query.Add(1);
            query.Add(mfn);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return null;
            }

            response.CheckReturnCode();
            string result = response.ReadRemainingUtfText();
            if (!string.IsNullOrEmpty(result))
            {
                result = result.TrimEnd();
            }

            return result;
        } // method FormatRecordAsync


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
            var query = new Query(this, CommandCode.GetMaxMfn);
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
        ///
        /// </summary>
        public async Task<ServerVersion?> GetServerVersionAsync()
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new Query(this, CommandCode.ServerInfo);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return null;
            }

            response.CheckReturnCode();
            var result = new ServerVersion();
            result.Parse(response);

            return result;
        } // method GetServerVersionAsync

        /// <summary>
        /// Получение списка файлов на сервере.
        /// </summary>
        public async Task<string[]> ListFilesAsync
            (
                string specification
            )
        {
            if (!CheckConnection() || string.IsNullOrEmpty(specification))
            {
                return Array.Empty<string>();
            }

            var query = new Query(this, CommandCode.ListFiles);
            query.AddAnsi(specification);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return Array.Empty<string>();
            }

            var lines = response.ReadRemainingAnsiLines();
            var result = new LocalList<string>();
            foreach (var line in lines)
            {
                var files = IrbisText.SplitIrbisToLines(line);
                foreach (var file1 in files)
                {
                    if (!string.IsNullOrEmpty(file1))
                    {
                        foreach (var file2 in file1.Split(IrbisText.WindowsDelimiter))
                        {
                            if (!string.IsNullOrEmpty(file2))
                            {
                                result.Add(file2);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        } // method ListFilesAsync

        /// <summary>
        /// Получение списка процессов на сервере.
        /// </summary>
        public async Task<ProcessInfo[]> ListProcessesAsync()
        {
            if (!CheckConnection())
            {
                return Array.Empty<ProcessInfo>();
            }

            var query = new Query(this, CommandCode.GetProcessList);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return Array.Empty<ProcessInfo>();
            }

            response.CheckReturnCode();
            var result = ProcessInfo.Parse(response);

            return result;
        } // method ListProcessesAsync

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

            var response = await ExecuteAsync(CommandCode.Nop);

            return !ReferenceEquals(response, null)
                   && response.CheckReturnCode();
        } // method NopAsync

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public void ParseConnectionString
            (
                string? connectionString
            )
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            var pairs = connectionString.Split
                (
                    ';',
                    StringSplitOptions.RemoveEmptyEntries
                );
            foreach (var pair in pairs)
            {
                if (!pair.Contains('='))
                {
                    continue;
                }

                var parts = pair.Split('=', 2);
                var name = parts[0].Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                var value = parts[1].Trim();

                switch (name)
                {
                    case "host":
                    case "server":
                    case "address":
                        Host = value;
                        break;

                    case "port":
                        Port = ushort.Parse(value);
                        break;

                    case "user":
                    case "username":
                    case "name":
                    case "login":
                    case "account":
                        Username = value;
                        break;

                    case "password":
                    case "pwd":
                    case "secret":
                        Password = value;
                        break;

                    case "db":
                    case "database":
                    case "base":
                    case "catalog":
                        Database = value;
                        break;

                    case "arm":
                    case "workstation":
                        Workstation = value;
                        break;

                    case "debug":
                        _debug = true;
                        break;

                    default:
                        throw new IrbisException($"Unknown key {name}");
                }
            }
        } // method ParseConnectionString

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        public async Task<Record?> ReadRecordAsync
            (
                int mfn
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new Query(this, CommandCode.ReadRecord);
            query.AddAnsi(Database);
            query.Add(mfn);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return null;
            }

            if (!response.CheckReturnCode(_goodCodesForReadRecord))
            {
                return null;
            }

            var result = new Record
            {
                Database = Database
            };
            result.Decode(response);

            return result;
        } // method ReadRecordAsync

        /// <summary>
        ///
        /// </summary>
        public async Task<string?> ReadTextFileAsync
            (
                string? specification
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            if (string.IsNullOrEmpty(specification))
            {
                return null;
            }

            var query = new Query(this, CommandCode.ReadDocument);
            query.AddAnsi(specification);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null))
            {
                return null;
            }

            var result = IrbisText.IrbisToWindows(response.ReadAnsi());

            return result;
        } // method ReadTextFileAsync

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        public async Task<FoundItem[]> SearchAsync
            (
                SearchParameters parameters
            )
        {
            if (!CheckConnection())
            {
                return Array.Empty<FoundItem>();
            }

            var query = new Query(this, CommandCode.Search);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null)
                || !response.CheckReturnCode())
            {
                return Array.Empty<FoundItem>();
            }

            return FoundItem.Parse(response);
        } // method SearchAsync

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public async Task<int[]> SearchAsync
            (
                string expression
            )
        {
            if (!CheckConnection())
            {
                return Array.Empty<int>();
            }

            var query = new Query(this, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression
            };
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (ReferenceEquals(response, null)
                || !response.CheckReturnCode())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn(response);
        } // method SearchAsync

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
