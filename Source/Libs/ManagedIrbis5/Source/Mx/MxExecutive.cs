﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MxExecutive.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.ConsoleIO;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Mx.Commands;
using ManagedIrbis.Mx.Infrastructrure;
using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx
{
    /// <summary>
    ///
    /// </summary>
    public sealed class MxExecutive
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public PftContext Context { get; internal set; }

        /// <summary>
        /// Console.
        /// </summary>
        [NotNull]
        public IMxConsole MxConsole { get; set; }

        /// <summary>
        /// Palette.
        /// </summary>
        [NotNull]
        public MxPalette Palette { get; set; }

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; internal set; }

        /// <summary>
        /// Commands.
        /// </summary>
        [NotNull]
        public NonNullCollection<MxCommand> Commands { get; private set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        public string DescriptionFormat { get; set; }

        /// <summary>
        /// Order expression.
        /// </summary>
        [CanBeNull]
        public string OrderFormat { get; set; }

        /// <summary>
        /// Search limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public NonNullCollection<MxRecord> Records { get; private set; }

        /// <summary>
        /// Stop repl.
        /// </summary>
        public bool StopFlag { get; set; }

        /// <summary>
        /// Verbosity level.
        /// </summary>
        public int VerbosityLevel { get; set; }

        /// <summary>
        /// Search history.
        /// </summary>
        [NotNull]
        public Stack<string> History { get; private set; }

        /// <summary>
        /// Stack of databases.
        /// </summary>
        [NotNull]
        public Stack<string> Databases { get; private set; }

        /// <summary>
        /// List of modules.
        /// </summary>
        [NotNull]
        public List<MxModule> Modules { get; private set; }

        /// <summary>
        /// List of handlers.
        /// </summary>
        [NotNull]
        public List<MxHandler> Handlers { get; private set; }

        /// <summary>
        /// Get version of the executive.
        /// </summary>
        public static Version Version
        {
            get
            {
#if CLASSIC || NETCORE

                Assembly assembly = typeof(MxExecutive).Assembly;
                Version result = assembly.GetName().Version;

                return result;

#else

                return new Version (1, 0);

#endif
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxExecutive()
        {
            _output = new StringBuilder();

            VerbosityLevel = 3;
            DescriptionFormat = "@brief";

            MxConsole = new MxConsole();
            Palette = MxPalette.GetDefaultPalette();
            Provider = new NullProvider();
            Context = new PftContext(null);
            Context.SetProvider(Provider);
            Commands = new NonNullCollection<MxCommand>();
            Records = new NonNullCollection<MxRecord>();
            History = new Stack<string>();
            Databases = new Stack<string>();
            Modules = new List<MxModule>();
            Handlers = new List<MxHandler>();

            _CreateStandardCommands();
            _InitializeCommands();
        }

        #endregion

        #region Private members

        private StringBuilder _output;

#if !WINMOBILE && !PocketPC

        private void _CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            StopFlag = true;
        }

#endif

        private void _CreateStandardCommands()
        {
            Commands.AddRange
                (
                    new MxCommand[]
                    {
                        new AliasCommand(),
                        new BangCommand(),
                        new ClsCommand(),
                        new ConnectCommand(),
                        new CsCommand(),
                        new CsFileCommand(),
                        new DbCommand(),
                        new DirCommand(),
                        new DisconnectCommand(),
                        new ExitCommand(),
                        new ExportCommand(),
                        new FormatCommand(),
                        new HelpCommand(),
                        new HistoryCommand(),
                        new InfoCommand(),
                        new LimitCommand(),
                        new ListCommand(),
                        new ListDbCommand(),
                        new ListUsersCommand(),
                        new ModuleCommand(),
                        new NopCommand(),
                        new PftCommand(),
                        new PingCommand(),
                        new PrintCommand(),
                        new RefineCommand(),
                        new RestartCommand(),
                        new SearchCommand(),
                        new SortCommand(),
                        new StatCommand(),
                        new StoreCommand(),
                        new TypeCommand(),
                        new VerCommand()
                    }
                );
        }

        private void _DisposeCommands()
        {
            foreach (MxCommand command in Commands)
            {
                command.Dispose();
            }
        }

        private void _DisposeHandlers()
        {
            foreach (MxHandler handler in Handlers)
            {
                handler.Dispose();
            }
        }

        private void _DisposeModules()
        {
            foreach (MxModule module in Modules)
            {
                module.Dispose();
            }
        }

        private bool _ExecuteLine
            (
                [NotNull] TextNavigator navigator
            )
        {
            navigator.SkipWhitespace();

            string line = navigator.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                return true;
            }

            if (line.StartsWith("#"))
            {
                // Comment, ignore it
                return true;
            }

            List<MxHandler> detectedHandlers = new List<MxHandler>();

            string[] prefixes = Handlers.Select(h => h.Prefix).ToArray();
            if (prefixes.Length != 0)
            {
                while (true)
                {
                    int index = StringUtility.LastIndexOfAny(line, prefixes);
                    if (index < 0)
                    {
                        break;
                    }

                    string handlerCommand = line.Substring(index);
                    foreach (MxHandler handler in Handlers)
                    {
                        if (handlerCommand.StartsWith(handler.Prefix))
                        {
                            handlerCommand = handlerCommand
                                .Substring(handler.Prefix.Length).Trim();
                            handler.Parse(this, handlerCommand);
                            detectedHandlers.Add(handler);
                            break;
                        }
                    }
                    line = line.Substring(0, index);
                }
            }

            line = line.Trim();
            detectedHandlers.Reverse();

            string[] parts = StringUtility.SplitString
                (
                    line,
                    CommonSeparators.SpaceOrTab,
                    2
                );
            string commandName = parts[0];
            string commandArgument = null;
            if (parts.Length != 1)
            {
                commandArgument = parts[1];
            }

            MxCommand command = null;

            if (!string.IsNullOrEmpty(commandName))
            {
                command = _FindCommand(commandName);
                if (ReferenceEquals(command, null))
                {
                    WriteError(string.Format
                        (
                            "Unknown command: '{0}'", commandName
                        ));
                    return false;
                }
            }

            MxArgument[] arguments =
            {
                new MxArgument
                {
                    Text = commandArgument
                }
            };

            bool result = false;

            try
            {
                foreach (MxHandler handler in detectedHandlers)
                {
                    handler.BeginOutput(this);
                }

                if (!ReferenceEquals(command, null))
                {
                    result = command.Execute
                    (
                        this,
                        arguments
                    );
                }

                foreach (MxHandler handler in detectedHandlers)
                {
                    handler.EndOutput(this);
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "MxExecutive::_ExecuteLine",
                        exception
                    );

                WriteError(string.Format
                    (
                        "Exception: {0}",
                        exception
                    ));

                return result;
            }

            return result;
        }

        [CanBeNull]
        private MxCommand _FindCommand
            (
                string name
            )
        {
            return Commands.FirstOrDefault
                (
                    cmd => cmd.Name.SameString(name)
                );
        }

        private void _InitializeCommands()
        {
            foreach (MxCommand command in Commands)
            {
                command.Initialize(this);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Print banner.
        /// </summary>
        public void Banner()
        {
            WriteMessage(string.Format("mx64 version {0}", Version));
            WriteLine(string.Empty);
        }

        /// <summary>
        /// Clear the output.
        /// </summary>
        public void ClearOutput()
        {
            _output.Length = 0;
        }

        /// <summary>
        /// Execute initialization script.
        /// </summary>
        public bool ExecuteInitScript()
        {
            // TODO implement

            return true;
        }

        /// <summary>
        /// Execute script file.
        /// </summary>
        public bool ExecuteFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = FileUtility.ReadAllText(fileName, IrbisEncoding.Utf8);
            bool result = ExecuteLine(text);

            return result;
        }

        /// <summary>
        /// Execute line of the script.
        /// </summary>
        public bool ExecuteLine
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                if (!_ExecuteLine(navigator))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get collected output.
        /// </summary>
        [CanBeNull]
        public string GetOutput()
        {
            if (ReferenceEquals(_output, null))
            {
                return null;
            }

            return _output.ToString();
        }

        /// <summary>
        /// Форматирование на сервере.
        /// </summary>
        [NotNull]
        public string FormatRemote
            (
                [NotNull] string source
            )
        {
            MarcRecord record = new MarcRecord();
            string result = Provider.FormatRecord(record, source)
                          ?? string.Empty;

            return result;
        }

        /// <summary>
        /// Get specified command.
        /// </summary>
        [NotNull]
        public T GetCommand<T>()
            where T : MxCommand
        {
            T result = Commands.OfType<T>().FirstOrDefault();
            if (ReferenceEquals(result, null))
            {
                throw new IrbisException
                    (
                        string.Format
                            (
                                "Command {0} not found",
                                typeof(T).Name
                            )
                    );
            }

            return result;
        }

        /// <summary>
        /// Get specified command.
        /// </summary>
        [CanBeNull]
        public MxCommand GetCommand
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            return Commands.FirstOrDefault(c => c.Name.SameString(name));
        }

        /// <summary>
        /// Load the module.
        /// </summary>
        public void LoadModule
            (
                [NotNull] string modulePath
            )
        {
            Code.NotNullNorEmpty(modulePath, "modulePath");

#if UAP || WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            string extension = Path.GetExtension(modulePath);
            if (string.IsNullOrEmpty(extension))
            {
                modulePath = modulePath + ".mxmodule";
            }

            if (!File.Exists(modulePath))
            {
                modulePath = Path.Combine("modules", modulePath);
            }

            ModuleDefinition definition = JsonUtility.ReadObjectFromFile<ModuleDefinition>(modulePath);

            // Must not load twice
            MxModule found = Modules.FirstOrDefault
                (
                    m => m.GetType().FullName == definition.ClassName
                );
            if (!ReferenceEquals(found, null))
            {
                return;
            }

            Assembly assembly = Assembly.LoadFile(definition.AssemblyPath);
            Type type = assembly.GetType(definition.ClassName);
            MxModule module = (MxModule)Activator.CreateInstance(type);
            module.Initialize(this);
            Modules.Add(module);

#endif
        }

        /// <summary>
        /// Read one line.
        /// </summary>
        [NotNull]
        public string ReadLine()
        {
            ConsoleColor saveColor = MxConsole.ForegroundColor;
            try
            {
                MxConsole.ForegroundColor = Palette.Command;
                return MxConsole.ReadLine();
            }
            finally
            {
                MxConsole.ForegroundColor = saveColor;
            }
        }

        /// <summary>
        /// REPL
        /// </summary>
        public void Repl()
        {
#if !WINMOBILE && !PocketPC && !UAP

            Console.CancelKeyPress += _CancelKeyPress;
            Console.Title = string.Format
                (
                    "mx64 v{0}", Version
                );

#endif

            while (!StopFlag)
            {
                string line = ReadLine();
                ExecuteLine(line);
                WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Write error message.
        /// </summary>
        public void WriteError
            (
                [NotNull] string text
            )
        {
            WriteLine(Palette.Error, text);
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string text
            )
        {
            MxConsole.Write(text);
            MxConsole.Write(Environment.NewLine);
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                ConsoleColor color,
                [NotNull] string text
            )
        {
            ConsoleColor saveColor = MxConsole.ForegroundColor;
            try
            {
                ConsoleInput.ForegroundColor = color;
                MxConsole.Write(text);
                MxConsole.Write(Environment.NewLine);
            }
            finally
            {
                MxConsole.ForegroundColor = saveColor;
            }
        }

        /// <summary>
        /// Write message
        /// </summary>
        public void WriteMessage
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            ConsoleColor saveColor = MxConsole.ForegroundColor;
            try
            {
                MxConsole.ForegroundColor = Palette.Message;
                WriteLine(text);
            }
            finally
            {
                MxConsole.ForegroundColor = saveColor;
            }
        }

        /// <summary>
        /// Write the text to the output.
        /// </summary>
        public void WriteOutput
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                _output.AppendLine(text);
                WriteLine(Palette.Foreground, text);
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _DisposeModules();
            _DisposeHandlers();
            _DisposeCommands();

            Provider.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
