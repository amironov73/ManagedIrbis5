// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SyncConnection.cs -- подключение к серверу
 * Ars Magna project, http://arsmagna.ru
 */

#region Usinh directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Синхронное подключение к серверу ИРБИС64.
    /// </summary>
    public class SyncConnection
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Адрес или имя хоста сервера ИРБИС64.
        /// </summary>
        /// <remarks>Значение по умолчанию "127.0.0.1".</remarks>
        public string? Host { get; set; } = "127.0.0.1";

        /// <summary>
        /// Номер порта, на котором сервер ИРБИС64 принимает клиентские подключения.
        /// </summary>
        /// <remarks>Значение по умолчанию 6666.</remarks>
        public ushort Port { get; set; } = 6666;

        /// <summary>
        /// Имя (логин) пользователя системы ИРБИС64.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>null</c>,
        /// с таким значением подключение не может быть установлено.</remarks>
        public string? Username { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя системы ИРБИС64.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>null</c>,
        /// с таким значением подключение не может быть установлено.</remarks>
        public string? Password { get; set; } = string.Empty;

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>"IBIS"</c>.
        /// </remarks>
        public string? Database { get; set; } = "IBIS";

        /// <summary>
        /// Код типа приложения.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>null</c>.
        /// </remarks>
        public string? Workstation { get; set; } = "C";

        /// <summary>
        /// Уникальный идентификатор клиента.
        /// </summary>
        public int ClientId { get; protected internal set; }

        /// <summary>
        /// Порядковый номер команды.
        /// </summary>
        public int QueryId
        {
            get => _queryId; // переменная нужна для Interlocked.Increment
            protected internal set => _queryId = value;
        }

        /// <summary>
        /// Признак активного подключения к серверу.
        /// </summary>
        public bool Connected { get; protected internal set; }

        /// <summary>
        /// Код ошибки, установленный последней командой.
        /// </summary>
        public int LastError { get; protected internal set; }

        /// <summary>
        /// Токен для отмены длительных операций.
        /// </summary>
        public CancellationToken Cancellation { get; protected set; }

        /// <summary>
        /// Версия сервера. Берется из ответа на регистрацию клиента.
        /// Сервер может прислать и пустую строку, надо быть
        /// к этому готовым.
        /// </summary>
        public string? ServerVersion { get; protected internal set; }

        /// <summary>
        /// INI-файл, присылвемый сервером в ответ на регистрацию клиента.
        /// </summary>
        public IniFile? IniFile { get; protected set; }

        /// <summary>
        /// Интервал подтверждения на сервере, минуты.
        /// Берется из ответа сервера при регистрации клиента.
        /// Сервер может прислать и пустую строку, к этому надо
        /// быть готовым.
        /// </summary>
        public int Interval { get; protected set; }

        #endregion

        #region Private members

        /// <summary>
        /// Номер запроса.
        /// </summary>
        protected int _queryId;

        #endregion

        #region Public methods

        /// <summary>
        /// Подстановка имени текущей базы данных, если она не задана явно.
        /// </summary>
        public string EnsureDatabase (string? database = null) =>
            ReferenceEquals (database, null) || database.Length == 0
                ? ReferenceEquals (Database, null) || Database.Length == 0
                    ? throw new ArgumentException (nameof (Database))
                    : Database
                : database;

        /// <summary>
        /// Проверка состояния подключения.
        /// </summary>
        public bool CheckProviderState()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;
        }

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command,
                params object[] args
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, command);
            foreach (var arg in args)
            {
                query.AddAnsi (arg.ToString());
            }

            var result = ExecuteSync (query);

            return result;
        }

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, command);
            var result = ExecuteSync (query);

            return result;
        }

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="arg1">Первый и единственный параметр команды.</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command,
                object arg1
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, command);
            query.AddAnsi (arg1.ToString());

            var result = ExecuteSync (query);

            return result;
        }

        /// <summary>
        /// Выполнение обмена с сервером.
        /// </summary>
        public Response? TransactSync
            (
                SyncQuery query
            )
        {
            using var client = new TcpClient (AddressFamily.InterNetwork);
            try
            {
                var host = Host.ThrowIfNull (nameof (Host));
                client.Connect (host, Port);
            }
            catch (Exception)
            {
                LastError = -100_002;

                return default;
            }

            var socket = client.Client;
            var length = query.GetLength();
            var prefix = new byte[12];
            length = Private.Int32ToBytes (length, prefix);
            prefix[length] = 10; // перевод строки
            var body = query.GetBody();

            try
            {
                socket.Send (prefix, 0, length + 1, SocketFlags.None);
                socket.Send (body, SocketFlags.None);
                socket.Shutdown (SocketShutdown.Send);
            }
            catch (Exception)
            {
                LastError = -100_002;

                return default;
            }

            var result = new Response();
            try
            {
                while (true)
                {
                    var buffer = new byte[2048];
                    var read = socket.Receive (buffer, SocketFlags.None);
                    if (read <= 0)
                    {
                        break;
                    }

                    var chunk = new ArraySegment<byte> (buffer, 0, read);
                    result.Add (chunk);
                }
            }
            catch (Exception)
            {
                LastError = -100_002;

                return default;
            }

            return result;
        }

        #endregion

        #region ISyncConnection members

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public Response? ExecuteSync
            (
                SyncQuery query
            )
        {
            Response? result;
            try
            {
                result = TransactSync (query);
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
                return null;
            }

            if (result is null)
            {
                return null;
            }

            result.Parse();
            Interlocked.Increment (ref _queryId);

            return result;
        }

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public string? GetDatabaseStat
            (
                StatDefinition definition
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, Constants.DatabaseStat);
            definition.Encode (this, query);

            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = "{\\rtf1 "
                         + response!.ReadRemainingUtfText()
                         + "}";

            return result;
        }

        /// <summary>
        /// Переподключение к серверу.
        /// </summary>
        public bool Reconnect()
        {
            if (Connected)
            {
                Disconnect();
            }

            IniFile?.Dispose();
            IniFile = null;

            return Connect();
        }

        /// <summary>
        /// Разблокирование указанной записи (альтернативный вариант).
        /// </summary>
        public bool UnlockRecordAlt (int mfn) =>
            ExecuteSync ("E", EnsureDatabase(), mfn).IsGood();

        #endregion

        #region ISyncProvider members

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        public bool ActualizeDatabase (string? database = default) =>
            ActualizeRecord (new () { Database = database, Mfn = 0 });

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        public bool ActualizeRecord (ActualizeRecordParameters parameters) => ExecuteSync
            (
                Constants.ActualizeRecord,
                EnsureDatabase (parameters.Database),
                parameters.Mfn
            ).IsGood();

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN:
            LastError = 0;
            QueryId = 1;
            ClientId = new Random().Next (100000, 999999);

            // нельзя использовать using response из-за goto
            var query = new SyncQuery (this, Constants.RegisterClient);
            query.AddAnsi (Username);
            query.AddAnsi (Password);

            var response = ExecuteSync (query);
            if (response is null)
            {
                LastError = -100_500;
                return false;
            }

            if (response.GetReturnCode() == -3337)
            {
                goto AGAIN;
            }

            if (response.ReturnCode < 0)
            {
                LastError = response.ReturnCode;
                return false;
            }

            ServerVersion = response.ServerVersion;
            Interval = response.ReadInteger();

            IniFile = new IniFile();
            var remainingText = response.RemainingText (Utility.Ansi);
            var reader = new StringReader (remainingText);
            IniFile.Read (reader);
            Connected = true;

            return true;
        }

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        public bool CreateDatabase
            (
                CreateDatabaseParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery (this, Constants.CreateDatabase);
            query.AddAnsi (EnsureDatabase (parameters.Database));
            query.AddAnsi (parameters.Description);
            query.Add (parameters.ReaderAccess);
            var response = ExecuteSync (query);

            return response.IsGood();
        }

        /// <summary>
        /// Создание словаря в указанной базе данных.
        /// </summary>
        public bool CreateDictionary (string? databaseName = default) =>
            ExecuteSync (Constants.CreateDictionary,
                EnsureDatabase (databaseName)).IsGood();

        /// <summary>
        /// Удаление базы данных на сервере.
        /// </summary>
        public bool DeleteDatabase (string? databaseName = default) =>
            ExecuteSync (Constants.DeleteDatabase,
                EnsureDatabase (databaseName)).IsGood();

        /// <summary>
        /// Удаление записи с указанным MFN.
        /// </summary>
        public bool DeleteRecord
            (
                int mfn
            )
        {
            var record = ReadRecord (mfn);
            if (record is null)
            {
                return false;
            }

            if (record.Deleted)
            {
                return true;
            }

            record.Status |= RecordStatus.LogicallyDeleted;

            return WriteRecord (record, dontParse: true);
        }

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        public bool Disconnect()
        {
            if (Connected)
            {
                try
                {
                    var _ = ExecuteSync (Constants.UnregisterClient);
                }
                catch (Exception)
                {
                    Debug.WriteLine (nameof (SyncConnection) + "::" + nameof (Disconnect) + ": error");
                }

                Connected = false;
            }

            return true;
        }

        /// <summary>
        /// Файл существует?
        /// </summary>
        public bool FileExist (FileSpecification specification) =>
            !string.IsNullOrEmpty (ReadTextFile (specification));

        /// <summary>
        /// Форматирование указанной записи по ее MFN.
        /// </summary>
        public string? FormatRecord
            (
                string format,
                int mfn
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, Constants.FormatRecord);
            query.AddAnsi (Database);
            query.AddFormat (format);
            query.Add (1);
            query.Add (mfn);
            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = response!.ReadRemainingUtfText().TrimEnd();

            return result;
        }

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        public bool FormatRecords
            (
                FormatRecordParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            // TODO: обнаруживать Records

            if (parameters.Mfns.IsNullOrEmpty())
            {
                parameters.Result = FormatRecord (parameters.Format!, parameters.Mfn)!;
                return true;
            }

            var query = new SyncQuery (this, Constants.FormatRecord);
            query.AddAnsi (EnsureDatabase (parameters.Database));
            query.AddFormat (parameters.Format);
            query.Add (parameters.Mfns!.Length);
            foreach (var mfn in parameters.Mfns)
            {
                query.Add (mfn);
            }

            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return false;
            }

            var lines = response!.ReadRemainingUtfLines();
            var result = new List<string> (lines.Length);
            if (parameters.Mfns.Length == 1)
            {
                result.Add (lines[0]);
            }
            else
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty (line))
                    {
                        continue;
                    }

                    var parts = line.Split (Constants.NumberSign, 2);
                    if (parts.Length > 1)
                    {
                        result.Add (parts[1]);
                    }
                }
            }

            parameters.Result = result.ToArray();

            return true;
        }

        /// <summary>
        /// Форматирование записи в клиентском представлении.
        /// </summary>
        public string? FormatRecord
            (
                string format,
                Record record
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (string.IsNullOrEmpty (format))
            {
                return string.Empty;
            }

            var query = new SyncQuery (this, Constants.FormatRecord);
            query.AddAnsi (EnsureDatabase (string.Empty));
            query.AddFormat (format);
            query.Add (-2);
            query.AddUtf (record.Encode());
            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = response!.ReadRemainingUtfText().TrimEnd();

            return result;
        }

        /// <summary>
        /// Форматирование записей по их MFN.
        /// </summary>
        public string[]? FormatRecords
            (
                int[] mfns,
                string format
            )
        {
            var parameters = new FormatRecordParameters
            {
                Database = EnsureDatabase(),
                Mfns = mfns,
                Format = format
            };

            return FormatRecords (parameters)
                ? parameters.Result.AsArray()
                : null;
        }

        /// <summary>
        /// Полнотекстовый поиск.
        /// </summary>
        public FullTextResult? FullTextSearch
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, Constants.NewFulltextSearch);
            searchParameters.Encode (this, query);
            textParameters.Encode (this, query);
            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode (response!);

            return result;
        }

        /// <summary>
        /// Получение информации о базе данных.
        /// </summary>
        public DatabaseInfo? GetDatabaseInfo (string? databaseName = default) =>
            ExecuteSync (Constants.RecordList, EnsureDatabase (databaseName))
                .Transform (resp => DatabaseInfo.Parse (EnsureDatabase (databaseName), resp));

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        public int GetMaxMfn
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync (Constants.GetMaxMfn, EnsureDatabase (databaseName));

            return response.IsGood() ? response!.ReturnCode : 0;
        }

        /// <summary>
        /// Получение статистики работы сервера ИРБИС64.
        /// </summary>
        public ServerStat? GetServerStat() =>
            ExecuteSync (Constants.GetServerStat).Transform (ServerStat.Parse);

        /// <summary>
        /// Получение информации о версии сервера.
        /// </summary>
        public ServerVersion? GetServerVersion() =>
            ExecuteSync (Constants.ServerInfo).Transform (ManagedIrbis.ServerVersion.Parse);

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        public GblResult? GlobalCorrection
            (
                GblSettings settings
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var database = EnsureDatabase (settings.Database);
            var query = new SyncQuery (this, Constants.GlobalCorrection);
            query.AddAnsi (database);
            settings.Encode (query);

            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = new GblResult();
            result.Parse (response!);

            return result;
        }

        /// <summary>
        /// Получение списка баз данных.
        /// </summary>
        public DatabaseInfo[] ListDatabases
            (
                string listFile = "dbnam3.mnu"
            )
        {
            var specification = new FileSpecification
            {
                Path = IrbisPath.Data,
                FileName = listFile
            };
            var menu = RequireMenuFile (specification);

            return DatabaseInfo.ParseMenu (menu);
        }

        /// <summary>
        /// Получение списка файлов из ответа сервера.
        /// </summary>
        public static string[]? ListFiles
            (
                Response? response
            )
        {
            if (response is null)
            {
                return null;
            }

            var lines = response.ReadRemainingAnsiLines();
            var result = new List<string>();
            var delimiters = new[] { Constants.WindowsDelimiter };
            foreach (var line in lines)
            {
                var files = Private.SplitIrbisToLines (line);
                foreach (var file1 in files)
                {
                    if (!string.IsNullOrEmpty (file1))
                    {
                        foreach (var file2 in file1.Split (delimiters, StringSplitOptions.None))
                        {
                            if (!string.IsNullOrEmpty (file2))
                            {
                                result.Add (file2);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение списка файлов.
        /// </summary>
        public string[]? ListFiles
            (
                params FileSpecification[] specifications
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (specifications.Length == 0)
            {
                return Array.Empty<string>();
            }

            var query = new SyncQuery (this, Constants.ListFiles);
            foreach (var specification in specifications)
            {
                query.AddAnsi (specification.ToString());
            }

            var response = ExecuteSync (query);

            return ListFiles (response);
        }

        /// <summary>
        /// Получение списка файлов, соответствующих спецификации.
        /// </summary>
        public string[]? ListFiles
            (
                string specification
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (string.IsNullOrEmpty (specification))
            {
                return Array.Empty<string>();
            }

            var response = ExecuteSync (Constants.ListFiles, specification);

            return ListFiles (response);
        }

        /// <summary>
        /// Список серверных процессов.
        /// </summary>
        public ProcessInfo[]? ListProcesses() =>
            ExecuteSync (Constants.GetProcessList).Transform (ProcessInfo.Parse);

        /// <summary>
        /// Список пользователей, зарегистрированных в системе.
        /// </summary>
        public UserInfo[]? ListUsers() =>
            ExecuteSync (Constants.GetUserList).Transform (UserInfo.Parse);

        /// <summary>
        /// Блокирование указанной записи.
        /// </summary>
        public bool LockRecord
            (
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Mfn = mfn,
                Lock = true
            };

            return ReadRecord (parameters) is not null;
        }

        /// <summary>
        /// Пустая операция - подтверждение регистрации.
        /// </summary>
        public bool NoOperation() => ExecuteSync (Constants.Nop).IsGood();

        /// <summary>
        /// Построение таблицы.
        /// </summary>
        public string? PrintTable (TableDefinition definition)
        {
            var query = new SyncQuery (this, Constants.Print);
            query.AddAnsi (EnsureDatabase (definition.DatabaseName));
            definition.Encode (query);

            var response = ExecuteSync (query);

            return response?.ReadRemainingUtfText();
        }

        /// <summary>
        /// Чтение двоичного файла с сервера.
        /// </summary>
        public byte[]? ReadBinaryFile
            (
                FileSpecification specification
            )
        {
            specification.BinaryFile = true;
            var response = ExecuteSync (Constants.ReadDocument, specification.ToString());
            if (response is null || !response.FindPreamble())
            {
                return null;
            }

            return response.RemainingBytes();
        }

        /// <summary>
        /// Чтение INI-файла как текстового.
        /// </summary>
        public IniFile? ReadIniFile (FileSpecification specification)
        {
            var content = ReadTextFile (specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader (content);
            var result = new IniFile { FileName = specification.FileName };
            result.Read (reader);

            return result;
        }

        /// <summary>
        /// Чтение меню как текстового файла.
        /// </summary>
        public MenuFile? ReadMenuFile
            (
                FileSpecification specification
            )
        {
            var content = ReadTextFile (specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader (content);
            var result = MenuFile.ParseStream (reader);
            result.FileName = specification.FileName;

            return result;
        }

        /// <summary>
        /// Чтение постингов термина.
        /// </summary>
        public TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            )
        {
            var query = new SyncQuery (this, Constants.ReadPostings);
            parameters.Encode (this, query);

            var response = ExecuteSync (query);
            if (!response.IsGood (Constants.GoodCodesForReadTerms))
            {
                return null;
            }

            return TermPosting.Parse (response!);
        }

        /// <summary>
        /// Чтение библиографической записи.
        /// </summary>
        public Record? ReadRecord
            (
                ReadRecordParameters parameters
            )
        {
            Record? result;

            try
            {
                var database = EnsureDatabase (parameters.Database);
                var query = new SyncQuery (this, Constants.ReadRecord);
                query.AddAnsi (database);
                query.Add (parameters.Mfn);
                if (parameters.Version != 0)
                {
                    query.Add (parameters.Version);
                }
                else
                {
                    query.Add (parameters.Lock);
                }

                query.AddFormat (parameters.Format);

                var response = ExecuteSync (query);
                if (!response.IsGood (Constants.GoodCodesForReadRecord))
                {
                    return null;
                }

                result = new Record
                {
                    Database = Database
                };

                switch ((ReturnCode)response!.ReturnCode)
                {
                    case ReturnCode.PreviousVersionNotExist:
                        result.Status |= RecordStatus.Absent;
                        break;

                    case ReturnCode.PhysicallyDeleted:
                    case ReturnCode.PhysicallyDeleted1:
                        result.Status |= RecordStatus.PhysicallyDeleted;
                        break;

                    default:
                        result.Decode (response);
                        break;
                }

                if (parameters.Version != 0)
                {
                    UnlockRecords (new[] { parameters.Mfn });
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                    (
                        nameof (ReadRecord) + " " + parameters,
                        exception
                    );
            }

            return result;
        }

        /// <summary>
        /// Чтение библиографической записи.
        /// </summary>
        public Record? ReadRecord
            (
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = Database,
                Mfn = mfn
            };

            return ReadRecord (parameters);
        }

        /// <summary>
        /// Чтение постингов, относящихся к указанной записи.
        /// </summary>
        public TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            if (!CheckProviderState() || string.IsNullOrEmpty (prefix))
            {
                return null;
            }

            var query = new SyncQuery (this, Constants.GetRecordPostings);
            query.AddAnsi (EnsureDatabase (parameters.Database));
            query.Add (parameters.Mfn);
            query.AddUtf (prefix);

            return ExecuteSync (query).Transform (TermPosting.Parse);
        }

        /// <summary>
        /// Чтение терминов поискового словаря.
        /// </summary>
        public Term[]? ReadTerms
            (
                TermParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var command = parameters.ReverseOrder
                ? Constants.ReadTermsReverse
                : Constants.ReadTerms;
            var query = new SyncQuery (this, command);
            parameters.Encode (this, query);
            var response = ExecuteSync (query);

            return !response.IsGood (Constants.GoodCodesForReadTerms) ? null : Term.Parse (response!);
        }

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="startTerm">Параметры терминов.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public Term[]? ReadTerms
            (
                string startTerm,
                int numberOfTerms
            )
        {
            var parameters = new TermParameters
            {
                Database = EnsureDatabase(),
                StartTerm = startTerm,
                NumberOfTerms = numberOfTerms
            };

            return ReadTerms (parameters);
        }

        /// <summary>
        /// Чтение текстового файла с сервера.
        /// </summary>
        public string? ReadTextFile (FileSpecification specification) =>
            ExecuteSync (Constants.ReadDocument, specification.ToString())
                .TransformNoCheck (resp => Private.IrbisToWindows (resp.ReadAnsi()));

        /// <summary>
        /// Чтение несколькних текстовых файлов с сервера.
        /// </summary>
        public string[]? ReadTextFiles (FileSpecification[] specifications)
        {
            var query = new SyncQuery (this, Constants.ReadDocument);
            foreach (var specification in specifications)
            {
                query.AddAnsi (specification.ToString());
            }

            var response = ExecuteSync (query);

            return response.IsGood() ? response!.ReadRemainingAnsiLines() : null;
        }

        /// <summary>
        /// Перезагрузка словаря для указанной базы данных.
        /// </summary>
        public bool ReloadDictionary (string? databaseName = default) =>
            ExecuteSync (Constants.ReloadDictionary, EnsureDatabase (databaseName)).IsGood();

        /// <summary>
        /// Перезагрузка файла документов в указанной базе данных.
        /// </summary>
        public bool ReloadMasterFile (string? databaseName = default) =>
            ExecuteSync (Constants.ReloadMasterFile,
                databaseName ?? Database.ThrowIfNull (nameof (Database))).IsGood();

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public Record RequireRecord (int mfn) => ReadRecord (mfn)
                                                 ?? throw new IrbisException ($"Record not found: MFN={mfn}");

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public Record RequireRecord (string expression) => SearchReadOneRecord (expression)
                                                           ?? throw new IrbisException ($"Record not found: expression={expression}");

        /// <summary>
        /// Чтение с сервера текстового файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="FileNotFoundException">Файл отсутствует или другая ошибка при чтении.</exception>
        public string RequireTextFile (FileSpecification specification) => ReadTextFile (specification)
                                                                           ?? throw new IrbisException ($"File not found: {specification}");

        /// <summary>
        /// Чтение с сервера INI-файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public IniFile RequireIniFile (FileSpecification specification) => ReadIniFile (specification)
                                                                           ?? throw new IrbisException ($"INI not found: {specification}");

        /// <summary>
        /// Чтение с сервера файла меню, которое обязательно должно быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public MenuFile RequireMenuFile (FileSpecification specification) => ReadMenuFile (specification)
                                                                             ?? throw new IrbisException ($"Menu not found: {specification}");

        /// <summary>
        /// Перезапуск сервера.
        /// </summary>
        public bool RestartServer() => ExecuteSync (Constants.RestartServer).IsGood();

        /// <summary>
        /// Поиск записей.
        /// </summary>
        public FoundItem[]? Search (SearchParameters parameters)
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery (this, Constants.Search);
            parameters.Encode (this, query);

            return ExecuteSync (query).Transform (FoundItem.Parse);
        }

        /// <summary>
        /// Упрощенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public int[] Search (string expression)
        {
            if (!CheckProviderState())
            {
                return Array.Empty<int>();
            }

            var query = new SyncQuery (this, Constants.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression
            };
            parameters.Encode (this, query);
            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn (response!);
        }

        /// <summary>
        /// Определение количества записей, удовлетворяющих
        /// заданному запросу.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public int SearchCount (string expression)
        {
            if (!CheckProviderState())
            {
                return -1;
            }

            var query = new SyncQuery (this, Constants.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode (this, query);
            var response = ExecuteSync (query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return -1;
            }

            return response.ReadInteger();
        }

        /// <summary>
        /// Поиск с последующим чтением записей.
        /// </summary>
        public Record[]? SearchRead (string expression)
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var found = Search (expression);
            var result = new List<Record> (found.Length);
            foreach (var mfn in found)
            {
                var record = ReadRecord (mfn);
                if (record is not null)
                {
                    result.Add (record);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Поиск с последующим чтением одной записи.
        /// </summary>
        public Record? SearchReadOneRecord
            (
                string expression
            )
        {
            var parameters = new SearchParameters
            {
                Expression = expression,
                NumberOfRecords = 1
            };
            var found = Search (parameters);

            return found is { Length: 1 }
                ? ReadRecord (found[0].Mfn)
                : default;
        }

        /// <summary>
        /// Очистка базы данных (до нулевой длины).
        /// </summary>
        public bool TruncateDatabase (string? databaseName = default) =>
            ExecuteSync (Constants.EmptyDatabase, EnsureDatabase (databaseName)).IsGood();

        /// <summary>
        /// Разблокирование базы данных.
        /// </summary>
        public bool UnlockDatabase (string? databaseName = default) =>
            ExecuteSync (Constants.UnlockDatabase, EnsureDatabase (databaseName)).IsGood();

        /// <summary>
        /// Разблокирование записей.
        /// </summary>
        public bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            var query = new SyncQuery (this, Constants.UnlockRecords);
            query.AddAnsi (EnsureDatabase (databaseName));
            var counter = 0;
            foreach (var mfn in mfnList)
            {
                query.Add (mfn);
                ++counter;
            }

            // Если список MFN пуст, считаем операцию успешной
            if (counter == 0)
            {
                return true;
            }

            return ExecuteSync (query).IsGood();
        }

        /// <summary>
        /// Обновление сервеного INI-файла.
        /// </summary>
        public bool UpdateIniFile (IEnumerable<string> lines)
        {
            var query = new SyncQuery (this, Constants.UpdateIniFile);
            var counter = 0;
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    query.AddAnsi (line);
                    ++counter;
                }
            }

            // Если список обновляемых строк пуст, считаем операцию успешной
            if (counter == 0)
            {
                return true;
            }

            return ExecuteSync (query).IsGood();
        }

        /// <summary>
        /// Обновление списка зарегистрированных в системе пользователей.
        /// </summary>
        public bool UpdateUserList (IEnumerable<UserInfo> users)
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery (this, Constants.SetUserList);
            var counter = 0;
            foreach (var user in users)
            {
                query.AddAnsi (user.Encode());
                ++counter;
            }

            // Если список обновляемых пользователей пуст, считаем операцию неуспешной
            if (counter == 0)
            {
                return false;
            }

            return ExecuteSync (query).IsGood();
        }

        /// <summary>
        /// Сохранение записи на сервере.
        /// </summary>
        public bool WriteRecord
            (
                WriteRecordParameters parameters
            )
        {
            var record = parameters.Record;
            if (record is not null)
            {
                var database = EnsureDatabase (record.Database);
                var query = new SyncQuery (this, Constants.UpdateRecord);
                query.AddAnsi (database);
                query.Add (parameters.Lock);
                query.Add (parameters.Actualize);
                query.AddUtf (record.Encode());

                var response = ExecuteSync (query);
                if (!response.IsGood())
                {
                    return false;
                }

                var result = response!.ReturnCode;
                if (!parameters.DontParse)
                {
                    record.Database ??= database;
                    record.Decode (response);
                }

                parameters.MaxMfn = result;

                return true;
            }

            var records = parameters.Records.ThrowIfNull (nameof (parameters.Records));
            if (records.Length == 0)
            {
                return true;
            }

            if (records.Length == 1)
            {
                parameters.Record = records[0];
                parameters.Records = null;
                var result2 = WriteRecord (parameters);
                parameters.Record = null;
                parameters.Records = records;

                return result2;
            }

            return WriteRecords
                (
                    records,
                    parameters.Lock,
                    parameters.Actualize,
                    parameters.DontParse
                );
        }

        /// <summary>
        /// Сохранение/обновление записи в базе данных.
        /// </summary>
        public bool WriteRecord
            (
                Record record,
                bool actualize = true,
                bool lockRecord = false,
                bool dontParse = false
            )
        {
            var parameters = new WriteRecordParameters
            {
                Record = record,
                Actualize = actualize,
                Lock = lockRecord,
                DontParse = dontParse
            };

            return WriteRecord (parameters);
        }

        /// <summary>
        /// Сохранение записей на сервере.
        /// </summary>
        public bool WriteRecords
            (
                IEnumerable<Record> records,
                bool lockFlag = false,
                bool actualize = true,
                bool dontParse = true
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery (this, Constants.SaveRecordGroup);
            query.Add (lockFlag);
            query.Add (actualize);
            var recordList = new List<Record>();
            foreach (var record in records)
            {
                var line = EnsureDatabase (record.Database)
                           + Constants.IrbisDelimiter
                           + Private.EncodeRecord (record);
                query.AddUtf (line);
                recordList.Add (record);
            }

            if (recordList.Count == 0)
            {
                return true;
            }

            var response = ExecuteSync (query);
            if (!response.IsGood())
            {
                return false;
            }

            if (!dontParse)
            {
                foreach (var record in recordList)
                {
                    Private.ParseResponseForWriteRecords (response!, record);
                }
            }

            return true;
        }

        /// <summary>
        /// Сохранение на сервере текстового файла.
        /// </summary>
        public bool WriteTextFile (FileSpecification specification) =>
            ExecuteSync (Constants.ReadDocument, specification.ToString()).IsGood();

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}
