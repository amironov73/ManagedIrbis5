// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

/* DirectProvider.cs -- провайдер, работающий напрямую с файлами баз данных ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Parameters;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Провайдер, работающий напрямую с файлами баз данных ИРБИС64.
    /// </summary>
    public class DirectProvider
        : ISyncProvider,
          ISetLastError
    {
        #region Events

        /// <inheritdoc cref="IIrbisProvider.Disposing"/>
        public event EventHandler? Disposing;

        #endregion

        #region Properties

        /// <summary>
        /// Режим доступа.
        /// </summary>
        public DirectAccessMode Mode { get; private set; }

        /// <summary>
        /// Корневой путь для текущейго экземпляра провайдера.
        /// </summary>
        public string RootPath { get; private set; }

        /// <summary>
        /// Data path.
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// Fallback path.
        /// </summary>
        public string? FallBackPath { get; set; }

        /// <summary>
        /// Fall-forward path.
        /// </summary>
        public string? FallForwardPath { get; set;}

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="rootPath">Корневой путь.</param>
        /// <param name="mode">Режим доступа.</param>
        /// <param name="strategy">Стратегия создания акцессора.</param>
        public DirectProvider
            (
                string rootPath,
                DirectAccessMode mode = DirectAccessMode.ReadOnly,
                IDirectAccess64Strategy? strategy = default
            )
        {
            _strategy = strategy ?? new TransientDirectAccess64();

            var fullPath = Path.GetFullPath(rootPath);
            if (!Directory.Exists(fullPath))
            {
                throw new FileNotFoundException(fullPath);
            }

            Mode = mode;
            RootPath = fullPath;
            DataPath = Path.Combine(RootPath, "DataI");
            Busy = new BusyState();
            PlatformAbstraction = PlatformAbstractionLayer.Current;
        }

        #endregion

        #region Private members

        private readonly IDirectAccess64Strategy _strategy;

        #endregion

        #region Public methods

        /// <summary>
        /// Ищем файл сначала в указанной базе данных, а затем,
        /// если он не найден, то в Deposit.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public string? DatabaseOrDeposit
            (
                string fileName,
                string database
            )
        {
            var result = Path.Combine(DataPath, database, fileName);
            if (!File.Exists(result))
            {
                result = Path.Combine ( DataPath, "Deposit" , fileName );
                if (!File.Exists(result))
                {
                    result = null;
                }
            }

            return result;

        } // method DatabaseOrDeposit

        /// <summary>
        /// Получение акцессора для доступа к файлам базы.
        /// </summary>
        public DirectAccessProxy64 GetAccessor (string? databaseName = null)
            => _strategy.CreateAccessor(this, databaseName);

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        public string FormatRecord
            (
                PftProgram program,
                Record? record
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            context.SetProvider(this);
            program.Execute(context);

            return context.GetProcessedOutput();

        } // method FormatRecord

        /// <summary>
        /// Получение таблицы символов.
        /// </summary>
        public AlphabetTable GetAlphabetTable()
        {
            var specification = new FileSpecification
                {
                    Path = IrbisPath.System,
                    FileName = AlphabetTable.DefaultFileName
                };
            var path = MapPath(specification);
            if (path is null)
            {
                return new AlphabetTable();
            }

            return File.Exists(path)
                ? AlphabetTable.ParseLocalFile(path)
                : new AlphabetTable();

        } // method GetAlphabetTable

        /// <summary>
        /// Поиск файла по его спецификации.
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="forReading">Файл должен существовать?
        /// Если <paramref name="forReading"/> равен <c>false</c>,
        /// то путь мапится чисто формально.</param>
        /// <returns></returns>
        /// <summary>При <paramref name="forReading"/>
        /// рассматриваются также папки <see cref="FallBackPath"/>
        /// и <see cref="FallForwardPath"/>.
        /// </summary>
        public string? MapPath
            (
                FileSpecification specification,
                bool forReading = true
            )
        {
            var fileName = specification.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new IrbisException(nameof(fileName));
            }

            if (forReading
                && !string.IsNullOrEmpty(FallForwardPath))
            {
                var probe = Path.Combine(FallForwardPath, fileName);
                if (File.Exists(fileName))
                {
                    return probe;
                }
            }

            var database = specification.Database
                ?? Database
                ?? throw new IrbisException(nameof(Database));

            var result = specification.Path switch
            {
                IrbisPath.System => Path.Combine (RootPath, fileName),

                IrbisPath.Data => Path.Combine (DataPath, fileName),

                IrbisPath.MasterFile or IrbisPath.InternalResource =>
                    DatabaseOrDeposit(fileName, database),

                (IrbisPath) 11 => fileName,

                _ => throw new IrbisException()
            };

            if (forReading
                && string.IsNullOrEmpty(result)
                && !string.IsNullOrEmpty(FallBackPath))
            {
                var probe = Path.Combine(FallBackPath, fileName);
                if (File.Exists(fileName))
                {
                    result = probe;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                Magna.Warning($"File not found: {specification}");
                // throw new IrbisException(nameof(fileName));
            }
            else
            {
                result = Path.GetFullPath(result);
                // var fileInfo = new FileInfo(result);
            }

            return result;

        } // method MapPath

        #endregion

        #region ISyncProvider members

        /// <inheritdoc cref="ISyncProvider.FileExist"/>
        public bool FileExist
            (
                FileSpecification specification
            )
        {
            var fullPath = MapPath(specification);

            return fullPath is not null;
        }

        /// <inheritdoc cref="IIrbisProvider.GetGeneration"/>
        public string GetGeneration() => "64";

        /// <inheritdoc cref="IIrbisProvider.PlatformAbstraction"/>
        public PlatformAbstractionLayer PlatformAbstraction
        {
            get;
            set;
        }

        /// <inheritdoc cref="IIrbisProvider.Configure"/>
        public void Configure
            (
                string configurationString
            )
        {
            var parameters = ParameterUtility.ParseString
            (
                configurationString
            );

            foreach (var parameter in parameters)
            {
                var name = parameter.Name
                    .ThrowIfNull("parameter.Name")
                    .ToLower();
                var value = parameter.Value
                    .ThrowIfNull("parameter.Value");

                switch (name)
                {
                    case "path":
                    case "root":
                        RootPath = value;
                        DataPath = value + "/DataI";
                        break;

                    case "db":
                    case "database":
                        Database = value;
                        break;

                    case "provider": // pass through
                        break;

                    default:
                        throw new IrbisException();
                }
            }
        } // method Configure

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Disposing.Raise(this);
        }

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService
            (
                Type serviceType
            )
        {
            throw new NotImplementedException();
        }

        private void SetBusy(bool busy)
        {
            Busy.SetState(busy);
        }

        private void SetConnected(bool state) => Connected = state;

        private void SetLastError(int code) => LastError = code;

        public string? Database { get; set; } = "IBIS";
        public bool Connected { get; private set; }
        public BusyState Busy { get; private set; }

        /// <inheritdoc cref="IIrbisProvider.LastError"/>
        public int LastError { get; set; }

        /// <inheritdoc cref="ICancellable.CancelOperation"/>
        public void CancelOperation()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICancellable.ThrowIfCancelled"/>
        public void ThrowIfCancelled()
        {
            throw new NotImplementedException();
        }

        public bool CheckProviderState()
        {
            throw new NotImplementedException();
        }

        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        }

        public bool ActualizeRecord(ActualizeRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool CreateDatabase(CreateDatabaseParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool CreateDictionary(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.FormatRecords"/>
        public bool FormatRecords
            (
                FormatRecordParameters parameters
            )
        {
            var program = PftUtility.CompileProgram
                (
                    parameters.Format ?? string.Empty
                );

            if (parameters.Records is { } records)
            {
                var result = new List<string>(records.Length);
                foreach (var record in records)
                {
                    result.Add(FormatRecord(program, record));
                }

                parameters.Result = result.ToArray();
            }
            else if (parameters.Record is { } record)
            {
                parameters.Result = FormatRecord(program, record);
            }

            return true;

        } // method FormatRecords

        /// <inheritdoc cref="ISyncProvider.FullTextSearch"/>
        public FullTextResult? FullTextSearch
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            throw new NotImplementedException();
        } // method FullTextSearch

        /// <inheritdoc cref="ISyncProvider.GetDatabaseInfo"/>
        public DatabaseInfo? GetDatabaseInfo
            (
                string? databaseName = default
            )
        {
            using var accessProxy = GetAccessor(databaseName);

            return accessProxy.Accessor.GetDatabaseInfo();

        } // method GetDatabaseInfo

        /// <inheritdoc cref="ISyncProvider.GetMaxMfn"/>
        public int GetMaxMfn
            (
                string? databaseName = default
            )
        {
            using var accessProxy = GetAccessor(databaseName);

            return accessProxy.Accessor.GetMaxMfn();

        } // method GetMaxMfn

        public ServerStat? GetServerStat()
        {
            throw new NotImplementedException();
        }

        public ServerVersion? GetServerVersion()
        {
            throw new NotImplementedException();
        }

        public GblResult? GlobalCorrection(GblSettings settings)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ListFiles"/>
        public string[]? ListFiles
            (
                params FileSpecification[] specifications
            )
        {
            var result = new List<string>();

            foreach (var specification in specifications)
            {
                var filePath = MapPath(specification, false);
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    if (Directory.Exists(directory))
                    {
                        var pattern = Path.GetFileName(filePath);
                        if (!string.IsNullOrEmpty(pattern))
                        {
                            var found = Directory.GetFiles(directory, pattern);
                            foreach (var one in found)
                            {
                                var fileName = Path.GetFileName(one);
                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    result.Add(one);
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();

        } // method ListFiles

        /// <inheritdoc cref="ISyncProvider.ListProcesses"/>
        public ProcessInfo[]? ListProcesses()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ListUsers"/>
        public UserInfo[]? ListUsers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.NoOperation"/>
        public bool NoOperation() => true;

        /// <inheritdoc cref="ISyncProvider.PrintTable"/>
        public string? PrintTable
            (
                TableDefinition definition
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReadBinaryFile"/>
        public byte[]? ReadBinaryFile
            (
                FileSpecification specification
            )
        {
            var fullPath = MapPath(specification);
            if (!string.IsNullOrEmpty(fullPath))
            {
                return File.ReadAllBytes(fullPath);
            }

            return default;
        }

        /// <inheritdoc cref="ISyncProvider.ReadPostings"/>
        public TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            )
        {
            throw new NotImplementedException();
        } // method ReadPostings

        /// <inheritdoc cref="ISyncProvider.ReadRecord"/>
        public Record? ReadRecord
            (
                ReadRecordParameters parameters
            )
        {
            // TODO: поддержка версий записи

            if (parameters.Mfn <= 0)
            {
                return default;
            }

            using var accessProxy = GetAccessor(parameters.Database);
            var result = accessProxy.Accessor.ReadRecord(parameters.Mfn);

            return result;
        } // method ReadRecord

        /// <inheritdoc cref="ISyncProvider.ReadRecordPostings"/>
        public TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            throw new NotImplementedException();
        } // method ReadRecordPostings

        /// <inheritdoc cref="ISyncProvider.ReadTerms"/>
        public Term[]? ReadTerms
            (
                TermParameters parameters
            )
        {
            throw new NotImplementedException();
        } // method ReadTerms

        /// <inheritdoc cref="ISyncProvider.ReadTextFile"/>
        public string? ReadTextFile
            (
                FileSpecification specification
            )
        {
            var fullPath = MapPath(specification);

            return fullPath is null
                ? null
                : File.ReadAllText(fullPath, IrbisEncoding.Ansi);

        } // method ReadTextFile

        public bool ReloadDictionary(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool ReloadMasterFile(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.RestartServer"/>
        public bool RestartServer()
        {
            Magna.Trace(nameof(DirectProvider) + "::" + nameof(RestartServer));

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.Search"/>
        public FoundItem[]? Search
            (
                SearchParameters parameters
            )
        {
            var expression = parameters.Expression;
            if (string.IsNullOrEmpty(expression))
            {
                return Array.Empty<FoundItem>();
            }

            var manager = new SearchManager(this);
            var context = new SearchContext(manager, this);
            var tokenList = SearchQueryLexer.Tokenize(expression);
            var parser = new SearchQueryParser(tokenList);
            var program = parser.Parse();
            var found = program.Find(context);
            var result = new List<FoundItem>(found.Length);
            foreach (var termLink in found)
            {
                var item = new FoundItem()
                {
                    Mfn = termLink.Mfn,
                    Text = null // TODO: возвращать текст
                };
            }

            return result.ToArray();

        } // method Search

        /// <inheritdoc cref="ISyncProvider.TruncateDatabase"/>
        public bool TruncateDatabase
            (
                string? databaseName = default
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.UnlockDatabase"/>
        public bool UnlockDatabase
            (
                string? databaseName = default
            )
        {
            using var accessProxy = GetAccessor(databaseName);
            accessProxy.Accessor.Mst.LockDatabase(false);

            return true;

        } // method UnlockDatabase

        public bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            using var accessProxy = GetAccessor(databaseName);
            foreach (var mfn in mfnList)
            {
                accessProxy.Accessor.Xrf.LockRecord(mfn, false);
            }

            return true;

        } // method UnlockRecords

        /// <inheritdoc cref="ISyncProvider.UpdateIniFile"/>
        public bool UpdateIniFile
            (
                IEnumerable<string> lines
            )
        {
            throw new NotImplementedException();
        } // method UpdateIniFile

        /// <inheritdoc cref="ISyncProvider.UpdateUserList"/>
        public bool UpdateUserList
            (
                IEnumerable<UserInfo> users
            )
        {
            throw new NotImplementedException();
        } // method UpdateUserList

        /// <inheritdoc cref="ISyncProvider.WriteTextFile"/>
        public bool WriteTextFile
            (
                FileSpecification specification
            )
        {
            var fullPath = MapPath(specification);

            if (fullPath is null)
            {
                return false;
            }

            try
            {
                var content = specification.Content ?? string.Empty;
                File.WriteAllText(fullPath, content, IrbisEncoding.Ansi);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        nameof(DirectProvider) + "::" + nameof(WriteTextFile),
                        exception
                    );
                return false;
            }

            return true;

        } // method WriteTextFile

        /// <inheritdoc cref="ISyncProvider.WriteRecord"/>
        public bool WriteRecord
            (
                WriteRecordParameters parameters
            )
        {
            var record = parameters.Record;
            if (record is null)
            {
                return false;
            }

            using var accessProxy = GetAccessor();
            accessProxy.Accessor.WriteRecord((Record) record);

            return true;

        } // method WriteRecord

        #endregion

        #region ISetLastError members

        /// <inheritdoc cref="ISetLastError.SetLastError"/>
        int ISetLastError.SetLastError(int code) => LastError = code;

        #endregion

    } // class DirectProvider

} // namespace ManagedIrbis.Direct
