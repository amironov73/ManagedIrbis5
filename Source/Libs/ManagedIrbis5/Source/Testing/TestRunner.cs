// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* TestRunner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using AM;
using AM.ConsoleIO;
using AM.Json;
using AM.Text.Output;

using Microsoft.CSharp;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Testing;

/// <summary>
/// Test runner.
/// </summary>
public sealed class TestRunner
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider? Connection { get; set; }

    /// <summary>
    /// Connection string for IRBIS64-server.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Don't run or stop IRBIS64-server.
    /// </summary>
    public bool ForeignServer { get; set; }

    /// <summary>
    /// Path to irbis_server.exe (including filename).
    /// </summary>
    public string? IrbisServerPath { get; set; }

    /// <summary>
    /// Server process (for stopping).
    /// </summary>
    public Process? ServerProcess { get; set; }

    /// <summary>
    /// Path to the tests.
    /// </summary>
    public string? TestPath { get; set; }

    /// <summary>
    /// Path to the test data.
    /// </summary>
    public string? DataPath { get; set; }

    /// <summary>
    /// List of tests.
    /// </summary>
    public List<string> TestList { get; }

    /// <summary>
    /// Compiled assembly.
    /// </summary>
    public Assembly? CompiledAssembly { get; set; }

    /// <summary>
    /// Assembly references.
    /// </summary>
    public List<string> References { get; }

    /// <summary>
    /// Where to store results.
    /// </summary>
    public string? ResultPath { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public TestRunner()
    {
        TestList = new ();
        References = new ();
        _contextList = new List<TestContext>();
    }

    #endregion

    #region Private members

    private const int WM_CLOSE = 0x0010;
    private const int WM_QUIT = 0x0012;

    private const string DefaultProcessName = "irbis_server";
    private const string ServerWindowTitle = "TCP/IP-Сервер ИРБИС 64";
    private const string ServerWindowClass = "TLabelServer";

    [DllImport ("User32.dll", SetLastError = false)]
    private static extern int PostThreadMessage
        (
            int threadId,
            int msg,
            int wParam,
            IntPtr lParam
        );

    [DllImport ("User32.dll", SetLastError = false)]
    private static extern IntPtr FindWindow
        (
            string className,
            string windowTitle
        );

    [DllImport ("User32.dll", SetLastError = false)]
    private static extern IntPtr SendMessage
        (
            IntPtr hwnd,
            int msg,
            int lParam,
            int wParam
        );

    private readonly List<TestContext> _contextList;

    private Type[] _GetTestClasses
        (
            Assembly assembly
        )
    {
        var types = assembly.GetTypes();
        var result = new List<Type>();
        foreach (var type in types)
        {
            var classAttribute
                = type.GetCustomAttribute<TestClassAttribute>();
            if (classAttribute == null)
            {
                continue;
            }

            if (!type.IsSubclassOf (typeof (AbstractTest)))
            {
                continue;
            }

            if (_GetTestMethods (type).Length == 0)
            {
                continue;
            }

            result.Add (type);
        }

        return result.ToArray();
    }

    private MethodInfo[] _GetTestMethods
        (
            Type type
        )
    {
        var result = new List<MethodInfo>();

        var methods = type.GetMethods();
        foreach (var method in methods)
        {
            var methodAttribute
                = method.GetCustomAttribute<TestMethodAttribute>();
            if (methodAttribute == null)
            {
                continue;
            }

            if (method.GetParameters().Length != 0)
            {
                continue;
            }

            if (method.ReturnType != typeof (void))
            {
                continue;
            }

            result.Add (method);
        }

        return result.ToArray();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Compile tests.
    /// </summary>
    public void CompileTests()
    {
        Write (ConsoleColor.Yellow, "Compiling tests ");

        var sources = new List<string>();
        foreach (var item in TestList)
        {
            var fileName = Path.Combine
                (
                    TestPath.ThrowIfNull(),
                    item
                );
            sources.Add (fileName);
        }

        var provider = new CSharpCodeProvider();

        var parameters = new CompilerParameters
            (
                References.ToArray()
            )
            {
                CompilerOptions = "/d:DEBUG",
                IncludeDebugInformation = true
            };
        var results = provider.CompileAssemblyFromFile
            (
                parameters,
                sources.ToArray()
            );
        var haveError = false;
        foreach (var error in results.Errors)
        {
            ConsoleInput.WriteLine (error.ToString()!);
            haveError = true;
        }

        if (haveError)
        {
            Magna.Logger.LogError
                (
                    nameof (TestRunner) + "::" + nameof (CompileTests)
                    + ": can't compile"
                );

            throw new ApplicationException ("Can't compile");
        }

        CompiledAssembly = results.CompiledAssembly;
        WriteLine (ConsoleColor.Green, "OK");
    }

    /// <summary>
    /// Discover tests.
    /// </summary>
    public void DiscoverTests()
    {
        WriteLine (ConsoleColor.Yellow, "Discovering tests");

        var count = 0;
        foreach (var item in TestList)
        {
            var fullPath = Path.Combine
                (
                    TestPath.ThrowIfNull(),
                    item
                );

            ConsoleInput.Write ($"\t{item}: ");
            if (File.Exists (fullPath))
            {
                WriteLine (ConsoleColor.Green, "OK");
                count++;
            }
            else
            {
                WriteLine (ConsoleColor.Red, "not found!");

                Magna.Logger.LogError
                    (
                        nameof (TestRunner) + "::" + nameof (DiscoverTests)
                        + ": not found: {Path}",
                        fullPath
                    );

                throw new FileNotFoundException (fullPath);
            }
        }

        WriteLine (ConsoleColor.Yellow, $"Source files found: {count}");
    }

    /// <summary>
    /// Find running local server process.
    /// </summary>
    public bool FindLocalServer()
    {
        var processes = Process.GetProcessesByName ("irbis_server");

        return processes.Length != 0;
    }

    /// <summary>
    /// Hide server window.
    /// </summary>
    public void HideServerWindow()
    {
        if (ServerProcess != null)
        {
            HideWindow
                (
                    ServerWindowClass,
                    ServerWindowTitle
                );
        }
    }

    /// <summary>
    /// Hide window
    /// </summary>
    public static void HideWindow
        (
            string className,
            string windowTitle
        )
    {
        Sure.NotNullNorEmpty (className);
        Sure.NotNullNorEmpty (windowTitle);

        var hwnd = FindWindow (className, windowTitle);
        if (hwnd != IntPtr.Zero)
        {
            SendMessage (hwnd, WM_CLOSE, 0, 0);
        }
    }

    /// <summary>
    /// Load configuration from local JSON file.
    /// </summary>
    public void LoadConfig
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        // TODO разобраться с загрузкой JSON

        /*

        var root = JObject.Parse
            (
                File.ReadAllText (fileName, Encoding.UTF8)
            );

        IrbisServerPath = root["irbisServer"]
            .ThrowIfNull()
            .ToString();

        ConnectionString = root["connectionString"]
            .ThrowIfNull()
            .ToString();

        DataPath = root["dataPath"]
            .ThrowIfNull()
            .ToString();

        TestPath = root["testPath"]
            .ThrowIfNull()
            .ToString();

        ResultPath = root["resultPath"]
            .ThrowIfNull()
            .ToString();

        ForeignServer = bool.Parse
            (
                root["foreignServer"]
                    .ThrowIfNull()
                    .ToString()
            );

        JToken tests = root["tests"];
        foreach (JToken child in tests.Children())
        {
            string testName = child.ToString();
            TestList.Add (testName);
        }

        JToken references = root["references"];
        foreach (JToken child in references.Children())
        {
            string assemblyReferences = child.ToString();
            References.Add (assemblyReferences);
        }

        */
    }

    /// <summary>
    /// Ping the server.
    /// </summary>
    public void PingTheServer()
    {
        Write (ConsoleColor.Magenta, "Pinging the server... ");

        using (var connection = ConnectionFactory.Shared.CreateSyncConnection())
        {
            connection.ParseConnectionString (ConnectionString);
            connection.Connect();
        }

        WriteLine (ConsoleColor.Green, "OK");
    }

    /// <summary>
    /// Print test execution report.
    /// </summary>
    public void PrintReport()
    {
        const ConsoleColor tableColor = ConsoleColor.White;
        const string tableFormat = "| {0,-50} | {1,-5} | {2,10} |";
        const int tableWidth = 50 + 3 + 5 + 3 + 10 + 3;
        var horizontalLine = new string ('-', tableWidth);

        WriteLine (ConsoleColor.Blue, string.Empty);

        var tests = _contextList
            .OrderBy (test => test.Name)
            .ToArray();

        if (tests.Length == 0)
        {
            return;
        }

        WriteLine
            (
                tableColor,
                horizontalLine
            );
        WriteLine
            (
                tableColor,
                string.Format
                    (
                        tableFormat,
                        "Test",
                        "OK?",
                        "Duration"
                    )
            );
        WriteLine
            (
                tableColor,
                horizontalLine
            );
        foreach (var context in tests)
        {
            WriteLine
                (
                    tableColor,
                    string.Format
                        (
                            tableFormat,
                            context.Name,
                            context.Failed ? "FAIL" : "OK",
                            context.Duration.TotalMilliseconds
                        )
                );
        }

        WriteLine
            (
                tableColor,
                horizontalLine
            );
        WriteLine (tableColor, string.Empty);

        var total = _contextList.Count;
        var failed = _contextList.Count (test => test.Failed);
        var success = total - failed;
        WriteLine
            (
                tableColor,
                $"Tests: {total}, failed: {failed}, success: {success}"
            );
        WriteLine (tableColor, string.Empty);
    }

    /// <summary>
    /// Run the tests.
    /// </summary>
    public void RunTests
        (
            string? testToRun
        )
    {
        var testClasses = _GetTestClasses (CompiledAssembly.ThrowIfNull ());

        WriteLine (ConsoleColor.White, "Test execution started");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        using (Connection = ConnectionFactory.Shared.CreateSyncConnection())
        {
            Connection.ParseConnectionString (ConnectionString);
            Connection.Connect();

            foreach (var testClass in testClasses)
            {
                RunTests
                    (
                        testClass,
                        testToRun
                    );
            }
        }

        stopwatch.Stop();

        WriteLine (ConsoleColor.White, "Test execution finished");
        WriteLine (ConsoleColor.Gray, $"Time elapsed: {stopwatch.Elapsed}");

        var fileName = $"run-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.json";
        var filePath = Path.Combine
            (
                ResultPath.ThrowIfNull (),
                fileName
            );

        var resultText = JsonUtility.SerializeShort (_contextList);
        File.WriteAllText
            (
                filePath,
                resultText,
                Encoding.UTF8
            );
        WriteLine
            (
                ConsoleColor.Magenta,
                $"Results written to {filePath}"
            );
    }

    /// <summary>
    /// Run tests in the class.
    /// </summary>
    public void RunTests
        (
            Type testClass,
            string? testToRun
        )
    {
        var testMethods = _GetTestMethods (testClass);

        var testObject = (AbstractTest?) Activator.CreateInstance (testClass);
        if (testObject is null)
        {
            // TODO намекнуть пользователю о проблеме
            return;
        }

        testObject.Connection = Connection;
        testObject.DataPath = DataPath;

        if (!string.IsNullOrEmpty (testToRun))
        {
            testMethods = testMethods
                .Where (method => method.Name == testToRun)
                .ToArray();
        }

        foreach (var method in testMethods)
        {
            RunTests
                (
                    testObject,
                    method
                );

            Thread.Sleep (500);
        }
    }

    /// <summary>
    /// Run tests in the class.
    /// </summary>
    public bool RunTests
        (
            AbstractTest testObject,
            MethodInfo method
        )
    {
        var result = true;

        Write (ConsoleColor.Cyan, $"{method.Name} ");
        var context = new TestContext (new ConsoleOutput())
        {
            Name = method.Name,
            StartTime = DateTime.Now
        };
        _contextList.Add (context);
        testObject.Context = context;
        testObject.Output = context.Output;

        var action = (Action)method.CreateDelegate
            (
                typeof (Action),
                testObject
            );

        try
        {
            action();
        }
        catch (Exception ex)
        {
            context.Output.WriteError (ex.ToString());
            context.Output.WriteError (Environment.NewLine);
            WriteLine (ConsoleColor.Red, "FAIL");
            result = false;
        }
        finally
        {
            context.FinishTime = DateTime.Now;
            context.Duration = context.FinishTime - context.StartTime;
        }

        if (result)
        {
            WriteLine (ConsoleColor.Green, " OK");
        }

        context.Failed = !result;

        return result;
    }

    /// <summary>
    /// Start server.
    /// </summary>
    public void StartServer()
    {
        if (ForeignServer)
        {
            WriteLine
                (
                    ConsoleColor.Yellow,
                    "Foreign server -- need not to start"
                );

            return;
        }

        var workingDirectory = Path.GetDirectoryName (IrbisServerPath);
        if (string.IsNullOrEmpty (workingDirectory))
        {
            Magna.Logger.LogError
                (
                    nameof (TestRunner) + "::" + nameof (StartServer)
                    + ": can't determine working directory"
                );

            throw new IrbisException
                (
                    "can't determine working directory"
                );
        }

        var startInfo = new ProcessStartInfo (IrbisServerPath!)
            {
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Minimized,
                WorkingDirectory = workingDirectory
            };

        ServerProcess = Process.Start (startInfo);
        Write (ConsoleColor.Yellow, "Starting IRBIS_SERVER.EXE ");
        Write (ConsoleColor.Gray, "initialization... ");

        // даем серверу прочухаться
        Thread.Sleep (3000);

        WriteLine (ConsoleColor.Green, "OK");
    }

    /// <summary>
    /// Stop process.
    /// </summary>
    public static bool StopProcess
        (
            Process process
        )
    {
        Sure.NotNull (process);

        try
        {
            foreach (ProcessThread thread in process.Threads)
            {
                PostThreadMessage
                    (
                        thread.Id,
                        WM_QUIT,
                        0,
                        IntPtr.Zero
                    );
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Stop server.
    /// </summary>
    public void StopServer()
    {
        if (ServerProcess == null)
        {
            WriteLine (ConsoleColor.Yellow, "Server not started");
            return;
        }

        if (ForeignServer)
        {
            WriteLine (ConsoleColor.Yellow, "Foreign server -- need not to stop");
            return;
        }

        Write (ConsoleColor.Yellow, "Stopping the server... ");
        StopProcess (ServerProcess);
        Thread.Sleep (3000);
        WriteLine (ConsoleColor.Green, "OK");
    }

    /// <summary>
    /// Write to console.
    /// </summary>
    public void Write
        (
            ConsoleColor color,
            string text
        )
    {
        var savedColor = Console.ForegroundColor;
        ConsoleInput.ForegroundColor = color;
        ConsoleInput.Write (text);
        ConsoleInput.ForegroundColor = savedColor;
    }

    /// <summary>
    /// Write to console.
    /// </summary>
    public void WriteLine
        (
            ConsoleColor color,
            string text
        )
    {
        var savedColor = Console.ForegroundColor;
        ConsoleInput.ForegroundColor = color;
        ConsoleInput.WriteLine (text);
        ConsoleInput.ForegroundColor = savedColor;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var result = !string.IsNullOrEmpty (IrbisServerPath)
                     && !string.IsNullOrEmpty (ConnectionString)
                     && !string.IsNullOrEmpty (TestPath);

        if (!result)
        {
            Magna.Logger.LogError (nameof (TestRunner) + "::" + nameof (Verify));
            if (throwOnError)
            {
                throw new VerificationException();
            }
        }

        return result;
    }

    #endregion
}
