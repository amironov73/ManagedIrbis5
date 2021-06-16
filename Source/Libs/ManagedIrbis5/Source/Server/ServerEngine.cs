// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ServerEngine.cs -- серверный движок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Server.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Серверный движок.
    /// </summary>
    public class ServerEngine
        : IAsyncDisposable
    {
        #region Properties

        /// <summary>
        /// Object for synchronization.
        /// </summary>
        public object SyncRoot { get; private set; }

        /// <summary>
        /// Момент старта движка.
        /// </summary>
        public DateTime StartedAt { get; private set; }

        /// <summary>
        /// Whether the engine paused client request processing.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Contexts.
        /// </summary>
        public List<ServerContext> Contexts { get; private set; }

        /// <summary>
        /// Cache.
        /// </summary>
        public ServerCache Cache { get; private set; }

        /// <summary>
        /// MNU file with standard _server_ INI file names.
        /// </summary>
        public MenuFile ClientIni { get; private set; }

        /// <summary>
        /// System data directory path.
        /// </summary>
        public string DataPath { get; private set; }

        /// <summary>
        /// Path for Deposit directory.
        /// </summary>
        public string DepositPath { get; private set; }

        /// <summary>
        /// Path for Deposit_USER directory.
        /// </summary>
        public string DepositUserPath { get; private set; }

        /// <summary>
        /// Ini file.
        /// </summary>
        public ServerIniFile IniFile { get; private set; }

        /// <summary>
        /// TCP listener.
        /// </summary>
        public IAsyncServerListener[] Listeners { get; private set; }

        /// <summary>
        /// Command mapper.
        /// </summary>
        public CommandMapper Mapper { get; private set; }

        /// <summary>
        /// System root directory path.
        /// </summary>
        public string SystemPath { get; private set; }

        /// <summary>
        /// Known users.
        /// </summary>
        public UserInfo[] Users { get; private set; }

        /// <summary>
        /// Workers.
        /// </summary>
        public List<ServerWorker> Workers { get; private set; }

        /// <summary>
        /// System work directory path.
        /// </summary>
        public string WorkDir { get; private set; }

        /// <summary>
        /// IP port number.
        /// </summary>
        public int PortNumber { get; private set; }

        /// <summary>
        /// Ban list.
        /// </summary>
        public BanMaster BanList { get; private set; }

        /// <summary>
        /// Delayed update task.
        /// </summary>
        public Task? DelayedUpdater { get; private set; }

        /// <summary>
        /// Watchdog.
        /// </summary>
        public ServerWatchdog? Watchdog { get; private set; }

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();

        } // method DisposeAsync

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ServerEngine
            (
                ServerSetup setup
            )
        {
            Magna.Info
                (
                    nameof(ServerEngine) + "::Constructor"
                    + ": enter"
                );

            if (setup.Break)
            {
                Debugger.Launch();
            }

            _cancellation = new CancellationTokenSource();

            SyncRoot = new object();
            Cache = new ServerCache();
            IniFile = setup.IniFile;
            var rootPathOverride = setup.RootPathOverride;
            SystemPath = rootPathOverride
                         ?? IniFile.SystemPath.ThrowIfNull("SystemPath");
            Magna.Info("SysPath=" + SystemPath);
            _VerifyDirectoryReadable(SystemPath);
            DataPath = ReferenceEquals(rootPathOverride, null)
                ? IniFile.DataPath.ThrowIfNull("DataPath")
                : Path.Combine(rootPathOverride, "Datai");
            Magna.Info("DataPath=" + DataPath);
            _VerifyDirectoryReadable(DataPath);
            DepositPath = Path.Combine(DataPath, "Deposit");
            DepositUserPath = Path.Combine(DataPath, "Deposit_USER");
            var workdirOverride = setup.WorkdirOverride;
            WorkDir = workdirOverride
                ?? IniFile.WorkDir.ThrowIfNull("WorkDir");
            Magna.Info("WorkDir=" + WorkDir);
            if (!Directory.Exists(WorkDir))
            {
                Directory.CreateDirectory(WorkDir);
            }
            _VerifyDirectoryReadable(WorkDir);
            _VerifyDirectoryWriteable(WorkDir);

            var fileName = Path.Combine(SystemPath, "client_ini.mnu");
            ClientIni = MenuFile.ParseLocalFile(fileName, IrbisEncoding.Ansi);
            var clientList = IniFile.ClientList ?? "client_m.mnu";
            clientList = Path.Combine(DataPath, clientList);
            Users = ServerUtility.LoadClientList(clientList, ClientIni);
            Contexts = new List<ServerContext>();
            Workers = new List<ServerWorker>();
            Mapper = new CommandMapper(this);
            BanList = new BanMaster();

            DelayedUpdater = Task.Factory.StartNew(_DelayedUpdater);
            Watchdog = new ServerWatchdog(this);
            Watchdog.Task.Start();

            _BuildListeners(setup);

            Magna.Info
                (
                    nameof(ServerEngine) + "::Constructor"
                    + ": leave"
                );

        } // constructor

        #endregion

        #region Private members

        private CancellationTokenSource _cancellation;

        private void _BuildListeners
            (
                ServerSetup setup
            )
        {
            var usePortOverride = true;
            var portNumber = setup.PortNumberOverride;
            if (portNumber <= 0)
            {
                usePortOverride = false;
                portNumber = IniFile.IPPort;
            }

            var listeners = new List<IAsyncServerListener>();
            if (setup.UseTcpIpV4)
            {
                listeners.Add
                    (
                        Tcp4ServerListener.ForPort(portNumber, _cancellation.Token)
                    );

                if (!usePortOverride)
                {
                    for (var i = 1; i < 10; i++)
                    {
                        var parameterName = "IP_PORT" + i;
                        portNumber = IniFile.GetValue(parameterName, 0);
                        if (portNumber > 0)
                        {
                            listeners.Add
                                (
                                    Tcp4ServerListener.ForPort(portNumber, _cancellation.Token)
                                );
                        }
                    }
                }
            }

            // if (setup.UseTcpIpV6)
            // {
            //     listeners.Add
            //         (
            //             Tcp6Listener.ForPort(portNumber, _cancellation.Token)
            //         );
            // }
            //
            // if (setup.HttpPort > 0)
            // {
            //     listeners.Add
            //         (
            //             HttpServerListener.ForPort(setup.HttpPort, _cancellation.Token)
            //         );
            // }

            // if (!string.IsNullOrEmpty(setup.PipeName))
            // {
            //     var instanceCount = setup.PipeInstanceCount;
            //     if (instanceCount <= 0)
            //     {
            //         instanceCount = 3;
            //     }
            //     listeners.Add
            //         (
            //             new PipeListener
            //                 (
            //                     setup.PipeName,
            //                     instanceCount,
            //                     _cancellation.Token
            //                 )
            //         );
            // }

            Listeners = listeners.ToArray();
            PortNumber = portNumber;
        }

        private string _GetDepositFile
            (
                string fileName
            )
        {
            var result = Path.GetFullPath(Path.Combine(DepositPath, fileName));
            if (!File.Exists(result))
            {
                result = null;
            }

            return result;

        } // method _GetDepositFile

        private async Task _DelayedUpdater()
        {
            while (true)
            {
                if (_cancellation.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    // TODO: implement
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            nameof(ServerEngine) + "::" + nameof(_DelayedUpdater),
                            exception
                        );
                }
            }

        } // method _DelayedUpdater

        private void _VerifyDirectoryReadable
            (
                string path
            )
        {
            // TODO Implement
        }

        private void _VerifyDirectoryWriteable
            (
                string path
            )
        {
            // TODO implement
        }

        private async Task _HandleClient
            (
                IAsyncServerSocket socket
            )
        {
            Magna.Trace(nameof(ServerEngine) + "::" + nameof(_HandleClient) + ": enter");

            if (_cancellation.IsCancellationRequested)
            {
                await socket.DisposeAsync();
                Magna.Trace (nameof(ServerEngine) + "::" + nameof(_HandleClient) + ": error leave");
                return;
            }

            if (BanList.IsAddressBanned(socket))
            {
                await socket.DisposeAsync();
                Magna.Trace (nameof(ServerEngine) + "::" + nameof(_HandleClient) + ": error leave");
                return;
            }

            var data = new WorkData
            {
                Engine = this,
                Socket = socket
            };

            var worker = new ServerWorker(data);
            data.Worker = worker;

            lock (SyncRoot)
            {
                Workers.Add(worker);
            }

            data.Task!.Start();

            Magna.Trace (nameof(ServerEngine) + "::" + nameof(_HandleClient) + ": leave");

        } // method _HandleClient

        #endregion

        #region Public methods

        /// <summary>
        /// Before execute the command.
        /// </summary>
        public void OnBeforeExecute
            (
                WorkData data
            )
        {
            // TODO implement
        }

        /// <summary>
        /// After command execution.
        /// </summary>
        public void OnAfterExecute
            (
                WorkData data
            )
        {
            // TODO implement
        }

        /// <summary>
        /// Create the context.
        /// </summary>
        public ServerContext CreateContext
            (
                string clientId
            )
        {
            var result = new ServerContext
            {
                Id = clientId,
                Connected = DateTime.Now
            };
            lock (SyncRoot)
            {
                Contexts.Add(result);
            }

            return result;
        }

        /// <summary>
        /// Destroy the context.
        /// </summary>
        public void DestroyContext
            (
                ServerContext context
            )
        {
            lock (SyncRoot)
            {
                Contexts.Remove(context);
            }
        }

        /// <summary>
        /// Find context for the client.
        /// </summary>
        public ServerContext? FindContext
            (
                string clientId
            )
        {
            lock (SyncRoot)
            {
                foreach (var context in Contexts)
                {
                    if (context.Id == clientId)
                    {
                        return context;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Find context for the client.
        /// </summary>
        public ServerContext RequireContext
            (
                WorkData data
            )
        {
            var request = data.Request.ThrowIfNull();
            var clientId = request.ClientId.ThrowIfNull();
            var result = FindContext(clientId);
            if (ReferenceEquals(result, null))
            {
                // Клиент не выполнил вход на сервер
                throw new IrbisException(-3334);
            }

            if (result.Username != request.Login
                || result.Password != request.Password
                || result.Workstation != request.Workstation)
            {
                // Неправильный уникальный идентификатор клиента
                throw new IrbisException(-3335);
            }

            return result;
        }

        /// <summary>
        /// Find administrator context for the user.
        /// </summary>
        public ServerContext RequireAdministratorContext
            (
                WorkData data
            )
        {
            var result = RequireContext(data);
            if (string.IsNullOrEmpty(result.Workstation))
            {
                result.Workstation = data.Request.Workstation;
            }
            if (result.Workstation != "A")
            {
                // Требуется вход администратора
                throw new IrbisException(-3338);
            }

            return result;
        }

        /// <summary>
        /// Find the specified user.
        /// </summary>
        public UserInfo? FindUser
            (
                string username
            )
        {
            lock (SyncRoot)
            {
                foreach (var user in Users)
                {
                    if (user.Name.SameString(username))
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get name of the default INI file for specified client type.
        /// </summary>
        public string? GetDefaultIniName
            (
                string? proposed,
                int index
            )
        {
            if (!string.IsNullOrEmpty(proposed))
            {
                return proposed;
            }

            var entry = ClientIni.Entries!.SafeAt(index);

            return entry?.Code;
        }

        /// <summary>
        /// Раскрываем ссылки на INI-файлы.
        /// </summary>
        public void FixIniFile
            (
                IniFile iniFile
            )
        {
            string? sectionName;

            while (true)
            {
                IniFile.Section? found = null;

                foreach (var section in iniFile.GetSections())
                {
                    sectionName = section.Name;

                    if (!string.IsNullOrEmpty(sectionName) && sectionName.StartsWith("@"))
                    {
                        found = section;
                        break;
                    }
                }

                if (ReferenceEquals(found, null))
                {
                    break;
                }

                sectionName = found.Name;
                if (!string.IsNullOrEmpty(sectionName))
                {
                    var filename = sectionName.Substring(1);
                    var ext = Path.GetExtension(filename);
                    if (string.IsNullOrEmpty(ext))
                    {
                        filename = filename + ".ini";
                    }
                    filename = Path.Combine(SystemPath, filename);
                    if (File.Exists(filename))
                    {
                        using var substitute = new IniFile(filename, IrbisEncoding.Ansi, false);
                        foreach (IniFile.Section section in substitute.GetSections())
                        {
                            iniFile.MergeSection(section);
                        }
                    }

                    iniFile.RemoveSection(sectionName);
                }
            }
        }

        /// <summary>
        /// Get user INI-file.
        /// </summary>
        public string GetUserIniFile
            (
                UserInfo user,
                string workstation
            )
        {
            string filename;
            switch (workstation)
            {
                case "a":
                case "A":
                    filename = GetDefaultIniName(user.Administrator, 5);
                    break;

                case "b":
                case "B":
                    filename = GetDefaultIniName(user.Circulation, 2);
                    break;

                case "c":
                case "C":
                    filename = GetDefaultIniName(user.Cataloger, 0);
                    break;

                case "k":
                case "K":
                    filename = GetDefaultIniName(user.Provision, 4);
                    break;

                case "m":
                case "M":
                    filename = GetDefaultIniName(user.Cataloger, 0);
                    break;

                case "r":
                case "R":
                    filename = GetDefaultIniName(user.Reader, 1);
                    break;

                case "p":
                case "P":
                    filename = GetDefaultIniName(user.Acquisitions, 3);
                    break;

                default:
                    // Недопустимый клиент
                    throw new IrbisException(-3338);
            }

            if (string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }

            string result;
            try
            {
                var ext = Path.GetExtension(filename);
                if (string.IsNullOrEmpty(ext))
                {
                    filename = filename + ".ini";
                }
                filename = Path.Combine(SystemPath, filename);
                var iniFile = new IniFile(filename, IrbisEncoding.Ansi, false);
                FixIniFile(iniFile);
                var writer = new StringWriter();
                iniFile.Save(writer);
                result = writer.ToString();
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(ServerEngine) + "::" + nameof(GetUserIniFile), exception);
                return string.Empty;
            }

            return result;

        } // method GetUserIniFile

        /// <summary>
        /// Get MST file path for the database.
        /// </summary>
        public string GetMstFile
            (
                string database
            )
        {
            // TODO cache

            var parPath = Path.Combine(DataPath, database + ".par");
            if (!File.Exists(parPath))
            {
                throw new IrbisException(-5555);
            }

            string result;
            try
            {
                var parFile = ParFile.ParseFile(parPath);
                var mstFile = parFile.MstPath.ThrowIfNull();
                mstFile = Path.Combine(SystemPath, mstFile);
                mstFile = Path.Combine(mstFile, database + ".mst");
                mstFile = Path.GetFullPath(mstFile);
                if (!File.Exists(mstFile))
                {
                    throw new IrbisException(-5555);
                }
                result = mstFile;
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        nameof(ServerEngine) + "::" + nameof(GetMstFile),
                        exception
                    );
                throw;
            }

            return result;
        }

        /// <summary>
        /// Get database access.
        /// </summary>
        public DirectAccess64 GetDatabase
            (
                string database
            )
        {
            // TODO cache

            var mstFile = GetMstFile(database);
            DirectAccess64 result;
            try
            {
                result = new DirectAccess64(mstFile, DirectAccessMode.ReadOnly);
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(ServerEngine) + "::" + nameof(GetDatabase), exception);
                throw;
            }

            return result;

        } // method GetDatabase

        /// <summary>
        /// Get provider for specified database.
        /// </summary>
        public ISyncProvider GetProvider
            (
                string database
            )
        {
            // TODO cache

            GetMstFile(database);

            DirectProvider result;
            try
            {
                result = new DirectProvider(SystemPath, DirectAccessMode.ReadOnly)
                {
                    Database = database
                };
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(ServerEngine) + "::" + nameof(GetProvider), exception);
                throw;
            }

            return result;

        } // method GetProvider

        //=====================================================================

        /// <summary>
        /// Главный цикл обработки запросов от клиентов.
        /// </summary>
        public async Task MainLoop()
        {
            Magna.Info(nameof(ServerEngine) + "::" + nameof(MainLoop) + ": enter");

            StartedAt = DateTime.Now;

            foreach (var listener in Listeners)
            {
                await listener.StartAsync();
            }

            while (true)
            {
                if (_cancellation.IsCancellationRequested)
                {
                    Magna.Info(nameof(ServerEngine) + "::" + nameof(MainLoop) + ": break signal 1");
                    break;
                }

                if (Paused)
                {
                    SpinWait.SpinUntil(() => Paused, 100);
                    continue;
                }

                try
                {
                    var taskCount = Listeners.Length;
                    var tasks = new Task<IAsyncServerSocket?>[taskCount];
                    for (var i = 0; i < taskCount; i++)
                    {
                        tasks[i] = Listeners[i].AcceptClientAsync();
                    }

                    var ready = Task.WaitAny(tasks, _cancellation.Token);
                    if (_cancellation.IsCancellationRequested)
                    {
                        Magna.Info(nameof(ServerEngine) + "::" + nameof(MainLoop) + ": break signal 2");
                        break;
                    }

                    var socket = tasks[ready].Result;

                    // Do we really need this?
                    // https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.dispose?view=net-5.0
                    for (var i = 0; i < taskCount; i++)
                    {
                        if (i != ready)
                        {
                            tasks[i].Dispose();
                        }
                    }

                    _HandleClient(socket);
                }
                catch (AggregateException)
                {
                    Magna.Info(nameof(ServerEngine) + "::" + nameof(MainLoop) + ": break signal 3");
                    break;
                }
                catch (OperationCanceledException)
                {
                    Magna.Info(nameof(ServerEngine) + "::" + nameof(MainLoop) + ": break signal 3");
                    break;
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            nameof(ServerEngine) + "::" + nameof(MainLoop),
                            exception
                        );
                }
            }

            foreach (var listener in Listeners)
            {
                await listener.StopAsync();
            }

            Magna.Info(nameof(ServerEngine) + "::" + nameof(MainLoop) + ": leave");

        } // method MainLoop

        //=====================================================================

        /// <summary>
        /// Resolve the file path.
        /// </summary>
        public string? ResolveFile
            (
                FileSpecification specification
            )
        {
            var fileName = specification.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            string result;
            var database = specification.Database;
            var path = (int)specification.Path;
            if (path == 0)
            {
                result = Path.Combine(SystemPath, fileName);
            }
            else if (path == 1)
            {
                result = Path.Combine(DataPath, fileName);
            }
            else
            {
                result = Path.GetFullPath(Path.Combine(DepositUserPath, fileName));
                if (File.Exists(result))
                {
                    return result;
                }

                if (string.IsNullOrEmpty(database))
                {
                    return _GetDepositFile(fileName);
                }

                var parPath = Path.Combine(DataPath, database + ".par");
                if (!File.Exists(parPath))
                {
                    result = _GetDepositFile(fileName);
                }
                else
                {
                    Dictionary<int, string> dictionary;
                    using (var reader
                        = TextReaderUtility.OpenRead(parPath, IrbisEncoding.Ansi))
                    {
                        dictionary = ParFile.ReadDictionary(reader);
                    }

                    if (!dictionary.ContainsKey(path))
                    {
                        result = _GetDepositFile(fileName);
                    }
                    else
                    {
                        result = Path.GetFullPath(Path.Combine
                            (
                                Path.Combine(SystemPath, dictionary[path]),
                                fileName
                            ));
                        if (!File.Exists(result))
                        {
                            result = _GetDepositFile(fileName);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the cancellation token.
        /// </summary>
        public CancellationToken GetCancellationToken()
        {
            return _cancellation.Token;
        }

        /// <summary>
        /// Get the workers count.
        /// </summary>
        public int GetWorkerCount()
        {
            lock (SyncRoot)
            {
                return Workers.Count;
            }
        }

        /// <summary>
        /// Cancel <see cref="MainLoop"/> processing.
        /// </summary>
        public void CancelProcessing()
        {
            _cancellation.Cancel();
        }

        /// <summary>
        /// Wait for workers (if any).
        /// </summary>
        public void WaitForWorkers()
        {
            var tasks = Workers
                .Select(worker => worker.Data.Task)
                .ToArray();

            if (tasks.Length != 0)
            {
                Task.WaitAll(tasks);
            }
        }

        #endregion

    } // class ServerEngine

} // namespace ManagedIrbis.Server
