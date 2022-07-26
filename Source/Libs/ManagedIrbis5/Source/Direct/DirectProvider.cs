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
using AM.Logging;
using AM.Parameters;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

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
    public DirectAccessMode Mode { get; }

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
    public string? FallForwardPath { get; set; }

    #endregion

    #region ISupportLogging members

    /// <inheritdoc cref="ISupportLogging.Logger"/>

    // TODO implement
    public ILogger? Logger => _logger;

    /// <inheritdoc cref="ISupportLogging.SetLogger"/>
    public void SetLogger
        (
            ILogger? logger
        )
    {
        _logger = logger;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="rootPath">Корневой путь.</param>
    /// <param name="mode">Режим доступа.</param>
    /// <param name="strategy">Стратегия создания акцессора.</param>
    /// <param name="caching">Стратегия кеширования.</param>
    /// <param name="locking">Стратегия блокировки базы данных.</param>
    /// <param name="serviceProvider">Провайдер сервисов.</param>
    public DirectProvider
        (
            string rootPath,
            DirectAccessMode mode = DirectAccessMode.ReadOnly,
            IDirectAccess64Strategy? strategy = default,
            IContextCachingStrategy? caching = default,
            IDirectLockingStrategy? locking = default,
            IServiceProvider? serviceProvider = default
        )
    {
        Sure.NotNullNorEmpty (rootPath);

        _access = strategy ?? new TransientDirectAccess64();
        _caching = caching ?? new TransientCaching();
        _locking = locking ?? new NullLocking();
        _serviceProvider = serviceProvider;
        _logger = (ILogger?)GetService (typeof (ILogger<MstFile64>));
        _logger?.LogTrace ($"{nameof (DirectProvider)}::Constructor ({rootPath}, {mode})");

        var fullPath = Path.GetFullPath (rootPath);
        if (!Directory.Exists (fullPath))
        {
            throw new FileNotFoundException (fullPath);
        }

        Mode = mode;
        RootPath = fullPath;
        DataPath = Path.Combine (RootPath, "DataI");
        Busy = new BusyState();
        PlatformAbstraction = PlatformAbstractionLayer.Current;
    }

    #endregion

    #region Private members

    private readonly IDirectAccess64Strategy _access;
    private readonly IContextCachingStrategy _caching;
    private readonly IDirectLockingStrategy _locking;
    private readonly IServiceProvider? _serviceProvider;
    private ILogger? _logger;

    #endregion

    #region Public methods

    /// <summary>
    /// Блокировка базы данных для выполнения операции (например, чтение записи).
    /// </summary>
    public LockMark LockUp
        (
            string databaseName
        )
    {
        Sure.NotNullNorEmpty (databaseName);

        _logger?.LogTrace ($"{nameof (DirectProvider)}::{nameof (LockUp)} ({databaseName})");

        var success = _locking.LockDatabase (this, databaseName);
        var result = new LockMark (this, _locking, databaseName, success);

        return result;
    }

    /// <summary>
    /// Ищет файл в папке Deposit
    /// </summary>
    public string? Deposit
        (
            string fileName
        )
    {
        // TODO: искать в Deposit_User

        var result = Unix.FindFile (DirectUtility.CombinePath (DataPath, "Deposit", fileName));
        if (!File.Exists (result))
        {
            result = null;
        }

        return result;
    }

    /// <summary>
    /// Ищем файл сначала в указанной базе данных, а затем,
    /// если он не найден, то в Deposit.
    /// </summary>
    public string? DatabaseOrDeposit
        (
            string fileName,
            string database
        )
    {
        string? result;

        var databasePath = MapDatabase (database);
        if (string.IsNullOrEmpty (databasePath))
        {
            result = Deposit (fileName);
        }
        else
        {
            result = Unix.FindFile
                         (
                             DirectUtility.CombinePath
                                 (
                                     databasePath,
                                     fileName
                                 )
                         )
                     ?? Deposit (fileName);
        }

        return result;
    }

    /// <summary>
    /// Получение акцессора для доступа к файлам базы.
    /// </summary>
    public DirectAccessProxy64 GetAccessor (string? databaseName = null)
    {
        return _access.CreateAccessor (this, databaseName, _serviceProvider);
    }

    /// <summary>
    /// Форматирование записи.
    /// </summary>
    public string FormatRecord
        (
            PftProgram program,
            Record? record
        )
    {
        var context = new PftContext (null)
        {
            Record = record
        };
        context.SetProvider (this);
        program.Execute (context);

        return context.GetProcessedOutput();
    }

    /// <summary>
    /// Получение таблицы символов.
    /// </summary>
    public AlphabetTable GetAlphabetTable()
    {
        var path = MapFile (IrbisPath.System, AlphabetTable.DefaultFileName);
        if (path is null)
        {
            return new AlphabetTable();
        }

        return Unix.FileExists (path)
            ? AlphabetTable.ParseLocalFile (path)
            : new AlphabetTable();
    }

    /// <summary>
    /// Получение кешированного файла (если есть).
    /// </summary>
    public string? GetCachedFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        return _caching.GetCachedFile (this, fileName);
    }

    /// <summary>
    /// Получение пути к базе данных.
    /// </summary>
    public string? MapDatabase
        (
            string? databaseName = default,
            bool mustExist = true
        )
    {
        databaseName ??= Database.ThrowIfNull();
        var specification = new FileSpecification
        {
            Path = IrbisPath.Data,
            FileName = databaseName + ".par"
        };

        var parFile = this.ReadParFile (specification);
        if (parFile is null)
        {
            return default;
        }

        var mstPath = parFile.MstPath.SafeTrim();
        if (string.IsNullOrEmpty (mstPath))
        {
            return default;
        }

        var result = Path.GetFullPath (DirectUtility.CombinePath (RootPath, mstPath));
        if (result.EndsWith (Path.DirectorySeparatorChar))
        {
            result = result.Substring (0, result.Length - 1);
        }

        var parent = Unix.FindDirectory (Path.GetDirectoryName (result) ?? ".");
        if (parent is null)
        {
            return null;
        }

        var child = Path.GetFileName (result);
        result = Path.Combine (parent, child);
        if (mustExist)
        {
            return Unix.FindDirectory (result);
        }

        return result;
    }

    /// <summary>
    /// Поиск файла по его спецификации.
    /// </summary>
    public string? MapFile
        (
            IrbisPath path,
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        return MapFile (new FileSpecification { Path = path, FileName = fileName });
    }

    /// <summary>
    /// Поиск файла по его спецификации.
    /// </summary>
    public string? MapFile (IrbisPath path, string database, string fileName) =>
        MapFile (new FileSpecification { Path = path, Database = database, FileName = fileName });

    /// <summary>
    /// Поиск файла по его спецификации.
    /// </summary>
    /// <param name="specification">Спецификация.</param>
    /// <param name="forReading">Файл должен существовать?
    /// Если <paramref name="forReading"/> равен <c>false</c>,
    /// то путь мапится чисто формально.</param>
    /// <returns>Метод должен выдавать либо полный путь к файлу
    /// (с учетом регистрозависимости Unix) либо <c>null</c></returns>
    /// <summary>При <paramref name="forReading"/>
    /// рассматриваются также папки <see cref="FallBackPath"/>
    /// и <see cref="FallForwardPath"/>.
    /// </summary>
    public string? MapFile
        (
            FileSpecification specification,
            bool forReading = true
        )
    {
        Sure.NotNull (specification);

        string? result;

        var fileName = specification.FileName;
        if (string.IsNullOrEmpty (fileName))
        {
            throw new IrbisException (nameof (fileName));
        }

        if (forReading
            && !string.IsNullOrEmpty (FallForwardPath))
        {
            result = Unix.FindFile (DirectUtility.CombinePath (FallForwardPath, fileName));
            if (result is not null)
            {
                return Path.GetFullPath (result);
            }
        }

        var database = specification.Database
                       ?? Database
                       ?? throw new IrbisException (nameof (Database));

        result = specification.Path switch
        {
            IrbisPath.System => DirectUtility.CombinePath (RootPath, fileName),

            IrbisPath.Data => DirectUtility.CombinePath (DataPath, fileName),

            IrbisPath.MasterFile or IrbisPath.InternalResource =>
                DatabaseOrDeposit (fileName, database),

            // TODO: мапить согласно PAR-файлу
            (IrbisPath)11 => fileName,

            _ => throw new IrbisException()
        };

        if (result is not null)
        {
            result = Unix.FindFile (result);
        }

        if (forReading
            && string.IsNullOrEmpty (result)
            && !string.IsNullOrEmpty (FallBackPath))
        {
            result = Unix.FindFile (DirectUtility.CombinePath (FallBackPath, fileName));
        }

        if (string.IsNullOrEmpty (result))
        {
            result = null;
            Magna.Logger.LogWarning
                (
                    "File not found: {Specification}",
                    specification
                );
        }
        else
        {
            result = Path.GetFullPath (result);
        }

        return result;
    }

    /// <summary>
    /// Чтение текстового файла по указанному пути.
    /// </summary>
    public string? ReadTextFile
        (
            IrbisPath path,
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        return ReadTextFile
            (
                new FileSpecification
                {
                    Path = path,
                    FileName = fileName
                }
            );
    }

    /// <summary>
    /// Чтение текстового файла по указанному пути.
    /// </summary>
    public string? ReadTextFile
        (
            IrbisPath path,
            string? databaseName,
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        return ReadTextFile
            (
                new FileSpecification
                {
                    Path = path,
                    Database = databaseName ?? Database,
                    FileName = fileName
                }
            );
    }

    /// <summary>
    /// Сохранение файла в кеше.
    /// </summary>
    public void StoreFileInCache
        (
            string fileName,
            string content
        )
    {
        Sure.NotNullNorEmpty (fileName);

        _caching.StoreFile (fileName, content);
    }

    #endregion

    #region ISyncProvider members

    /// <inheritdoc cref="ISyncProvider.FileExist"/>
    public bool FileExist
        (
            FileSpecification specification
        )
    {
        Sure.VerifyNotNull (specification);

        var fullPath = MapFile (specification);

        return fullPath is not null;
    }

    /// <inheritdoc cref="IIrbisProvider.GetGeneration"/>
    public string GetGeneration() => "64";

    /// <inheritdoc cref="IIrbisProvider.PlatformAbstraction"/>
    public PlatformAbstractionLayer PlatformAbstraction { get; set; }

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
            var name = parameter.Name.ThrowIfNull().ToLower();
            var value = parameter.Value.ThrowIfNull();

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
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _logger?.LogTrace ($"{nameof (DirectProvider)}::{nameof (Dispose)}");

        IsConnected = false;
        Disposing.Raise (this);
        _access.Dispose();
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
        return _serviceProvider?.GetService (serviceType);
    }

    private void SetBusy
        (
            bool busy
        )
    {
        Busy.SetState (busy);
    }

    /// <summary>
    /// Установка состояния подключения.
    /// </summary>
    private void SetConnected
        (
            bool state
        )
    {
        IsConnected = state;
    }

    /// <summary>
    /// Установка кода ошибки.
    /// </summary>
    private void SetLastError (int code)
    {
        LastError = code;
    }

    /// <inheritdoc cref="IIrbisProvider.Database"/>
    public string? Database { get; set; } = "IBIS";

    /// <inheritdoc cref="IIrbisProvider.IsConnected"/>
    public bool IsConnected { get; private set; }

    /// <inheritdoc cref="ICancellable.Busy"/>
    public BusyState Busy { get; private set; }

    /// <inheritdoc cref="IGetLastError.LastError"/>
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

    /// <inheritdoc cref="IIrbisProvider.CheckProviderState"/>
    public bool CheckProviderState()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IIrbisProvider.GetWaitHandle"/>
    public WaitHandle GetWaitHandle()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ActualizeRecord"/>
    public bool ActualizeRecord
        (
            ActualizeRecordParameters parameters
        )
    {
        Sure.NotNull (parameters);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.Connect"/>
    public bool Connect()
    {
        IsConnected = true;

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.CreateDatabase"/>
    public bool CreateDatabase
        (
            CreateDatabaseParameters parameters
        )
    {
        Sure.NotNull (parameters);

        var databaseName = parameters.Database;
        if (string.IsNullOrEmpty (databaseName))
        {
            return false;
        }

        var databasePath = MapDatabase (databaseName, false);
        if (Directory.Exists (databasePath))
        {
            // база уже существует
            return false;
        }

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.CreateDictionary"/>
    public bool CreateDictionary
        (
            string? databaseName = default
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.DeleteDatabase"/>
    public bool DeleteDatabase
        (
            string? databaseName = default
        )
    {
        var databasePath = MapDatabase (databaseName);
        if (string.IsNullOrEmpty (databasePath))
        {
            return false;
        }

        Directory.Delete (databasePath);

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.Disconnect"/>
    public bool Disconnect()
    {
        // TODO: что делать?

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.FormatRecords"/>
    public bool FormatRecords
        (
            FormatRecordParameters parameters
        )
    {
        Sure.VerifyNotNull (parameters);

        var program = PftUtility.CompileProgram
            (
                parameters.Format ?? string.Empty
            );

        if (parameters.Records is { } records)
        {
            var result = new List<string> (records.Length);
            foreach (var record in records)
            {
                result.Add (FormatRecord (program, record));
            }

            parameters.Result = result.ToArray();
        }
        else if (parameters.Record is { } record)
        {
            parameters.Result = FormatRecord (program, record);
        }

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.FullTextSearch"/>
    public FullTextResult? FullTextSearch
        (
            SearchParameters searchParameters,
            TextParameters textParameters
        )
    {
        Sure.NotNull (searchParameters);
        Sure.NotNull (textParameters);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.GetDatabaseInfo"/>
    public DatabaseInfo? GetDatabaseInfo
        (
            string? databaseName = default
        )
    {
        using var proxy = GetAccessor (databaseName);

        return proxy.Accessor.GetDatabaseInfo();
    }

    /// <inheritdoc cref="ISyncProvider.GetMaxMfn"/>
    public int GetMaxMfn
        (
            string? databaseName = default
        )
    {
        using var proxy = GetAccessor (databaseName);

        return proxy.Accessor.GetMaxMfn();
    }

    /// <inheritdoc cref="ISyncProvider.GetServerStat"/>
    public ServerStat? GetServerStat()
    {
        var result = new ServerStat();

        return result;
    }

    /// <inheritdoc cref="ISyncProvider.GetServerVersion"/>
    public ServerVersion? GetServerVersion()
    {
        var result = new ServerVersion
        {
            ConnectedClients = 1,
            MaxClients = int.MaxValue,
            Organization = "Сообщество пользователей АБИС ИРБИС64",
            Version = "64.2014.1"
        };

        return result;
    }

    /// <inheritdoc cref="ISyncProvider.GlobalCorrection"/>
    public GblResult? GlobalCorrection
        (
            GblSettings settings
        )
    {
        Sure.NotNull (settings);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ListFiles"/>
    public string[]? ListFiles
        (
            params FileSpecification[] specifications
        )
    {
        Sure.NotNull (specifications);

        var result = new List<string>();
        foreach (var specification in specifications)
        {
            var filePath = MapFile (specification, false);
            var directory = Path.GetDirectoryName (filePath);
            if (!string.IsNullOrEmpty (directory))
            {
                if (Directory.Exists (directory))
                {
                    var pattern = Path.GetFileName (filePath);
                    if (!string.IsNullOrEmpty (pattern))
                    {
                        var found = Directory.GetFiles (directory, pattern);
                        foreach (var one in found)
                        {
                            var fileName = Path.GetFileName (one);
                            if (!string.IsNullOrEmpty (fileName))
                            {
                                result.Add (one);
                            }
                        }
                    }
                }
            }
        }

        return result.ToArray();
    }

    /// <inheritdoc cref="ISyncProvider.ListProcesses"/>
    public ProcessInfo[]? ListProcesses() => Array.Empty<ProcessInfo>();

    /// <inheritdoc cref="ISyncProvider.ListUsers"/>
    public UserInfo[]? ListUsers()
    {
        var text = ReadTextFile (IrbisPath.Data, "client_m.mnu");

        return string.IsNullOrEmpty (text) ? default : UserInfo.Parse (text);
    }

    /// <inheritdoc cref="ISyncProvider.NoOperation"/>
    public bool NoOperation() => true;

    /// <inheritdoc cref="ISyncProvider.PrintTable"/>
    public string? PrintTable
        (
            TableDefinition definition
        )
    {
        Sure.NotNull (definition);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ReadBinaryFile"/>
    public byte[]? ReadBinaryFile
        (
            FileSpecification specification
        )
    {
        Sure.VerifyNotNull (specification);

        var fullPath = MapFile (specification);
        if (!string.IsNullOrEmpty (fullPath))
        {
            return File.ReadAllBytes (fullPath);
        }

        return default;
    }

    /// <inheritdoc cref="ISyncProvider.ReadPostings"/>
    public TermPosting[]? ReadPostings
        (
            PostingParameters parameters
        )
    {
        Sure.NotNull (parameters);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ReadRecord{T}"/>
    public T? ReadRecord<T>
        (
            ReadRecordParameters parameters
        )
        where T : class, IRecord, new()
    {
        Sure.NotNull (parameters);

        var databaseName = parameters.Database ?? Database.ThrowIfNullOrEmpty();

        // TODO: поддержка версий записи

        if (parameters.Mfn <= 0)
        {
            return default;
        }

        // TODO: выставлять код ошибки
        using var accessProxy = GetAccessor (databaseName);
        using var mark = LockUp (databaseName);
        var result = mark.Success
            ? accessProxy.Accessor.ReadRecord<T> (parameters.Mfn)
            : default;

        return result;
    }

    /// <inheritdoc cref="ISyncProvider.ReadRecordPostings"/>
    public TermPosting[]? ReadRecordPostings
        (
            ReadRecordParameters parameters,
            string prefix
        )
    {
        Sure.NotNull (parameters);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ReadTerms"/>
    public Term[]? ReadTerms
        (
            TermParameters parameters
        )
    {
        Sure.NotNull (parameters);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ReadTextFile"/>
    public string? ReadTextFile
        (
            FileSpecification specification
        )
    {
        Sure.VerifyNotNull (specification);

        var fileName = specification.ToString();
        var result = GetCachedFile (fileName);
        if (result is not null)
        {
            return result;
        }

        var fullPath = MapFile (specification);
        if (fullPath is null)
        {
            return null;
        }

        result = File.ReadAllText (fullPath, IrbisEncoding.Ansi);
        StoreFileInCache (fileName, result);

        return result;
    }

    /// <inheritdoc cref="ISyncProvider.ReloadDictionary"/>
    public bool ReloadDictionary
        (
            string? databaseName = default
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.ReloadMasterFile"/>
    public bool ReloadMasterFile
        (
            string? databaseName = default
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.RestartServer"/>
    public bool RestartServer()
    {
        Magna.Logger.LogTrace (nameof (DirectProvider) + "::" + nameof (RestartServer));

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.Search"/>
    public FoundItem[]? Search
        (
            SearchParameters parameters
        )
    {
        Sure.NotNull (parameters);

        var expression = parameters.Expression;
        if (string.IsNullOrEmpty (expression))
        {
            return Array.Empty<FoundItem>();
        }

        var manager = new SearchManager (this);
        var context = new SearchContext (manager, this);
        var tokenList = SearchQueryLexer.Tokenize (expression);
        var parser = new SearchQueryParser (tokenList);
        var program = parser.Parse();
        var found = program.Find (context);
        var result = new List<FoundItem> (found.Length);
        foreach (var termLink in found)
        {
            var item = new FoundItem()
            {
                Mfn = termLink.Mfn,
                Text = null // TODO: возвращать текст
            };

            result.Add (item);
        }

        return result.ToArray();
    }

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
        using var accessProxy = GetAccessor (databaseName);
        accessProxy.Accessor.Mst.LockDatabase (false);

        return true;
    }

    /// <summary>
    /// Разблокировка записей.
    /// </summary>
    public bool UnlockRecords
        (
            IEnumerable<int> mfnList,
            string? databaseName = default
        )
    {
        Sure.NotNull ((object?)mfnList);

        using var accessProxy = GetAccessor (databaseName);
        foreach (var mfn in mfnList)
        {
            accessProxy.Accessor.Xrf.LockRecord (mfn, false);
        }

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.UpdateIniFile"/>
    public bool UpdateIniFile
        (
            IEnumerable<string> lines
        )
    {
        Sure.NotNull ((object?)lines);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.UpdateUserList"/>
    public bool UpdateUserList
        (
            IEnumerable<UserInfo> users
        )
    {
        Sure.NotNull ((object?)users);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncProvider.WriteTextFile"/>
    public bool WriteTextFile
        (
            FileSpecification specification
        )
    {
        Sure.VerifyNotNull (specification);

        var fullPath = MapFile (specification);

        if (fullPath is null)
        {
            return false;
        }

        try
        {
            var content = specification.Content ?? string.Empty;
            File.WriteAllText (fullPath, content, IrbisEncoding.Ansi);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (DirectProvider) + "::" + nameof (WriteTextFile)
                );
            return false;
        }

        return true;
    }

    /// <inheritdoc cref="ISyncProvider.WriteRecord"/>
    public bool WriteRecord
        (
            WriteRecordParameters parameters
        )
    {
        Sure.NotNull (parameters);

        var record = parameters.Record;
        if (record is null)
        {
            return false;
        }

        using var accessProxy = GetAccessor();
        accessProxy.Accessor.WriteRecord ((Record)record);

        return true;
    }

    #endregion

    #region ISetLastError members

    /// <inheritdoc cref="ISetLastError.SetLastError"/>
    int ISetLastError.SetLastError (int code)
    {
        return LastError = code;
    }

    #endregion
}
