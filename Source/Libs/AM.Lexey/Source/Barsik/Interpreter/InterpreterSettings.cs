// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* InterpreterSettings.cs -- настройки интерпретатора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

using AM.Json;
using AM.Lexey.Barsik.Diagnostics;
using AM.Lexey.Barsik.Directives;
using AM.Lexey.Tokenizing;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Barsik;

/// <summary>
/// Настройки интерпретатора.
/// </summary>
[PublicAPI]
public sealed class InterpreterSettings
{
    #region Properties

    /// <summary>
    /// Запускать скрипт под отладчиком.
    /// </summary>
    [JsonPropertyName ("start-debugger")]
    public bool StartDebugger { get; set; }

    /// <summary>
    /// Отладочная печать во время разбора скрипта.
    /// </summary>
    [JsonPropertyName ("debug-parser")]
    public bool DebugParser { get; set; }

    /// <summary>
    /// Вывод дампа синтаксического дерева перед исполнением
    /// каждого скрипта.
    /// </summary>
    [JsonPropertyName ("dump-ast")]
    public bool DumpAst { get; set; }

    /// <summary>
    /// Вывод дампа всех переменных после исполнения скрипта.
    /// </summary>
    [JsonPropertyName ("dump-variables")]
    [JsonConverter (typeof (AnyTypeConverter<IBarsikDumper>))]
    public IBarsikDumper? VariableDumper { get; set; }

    /// <summary>
    /// Вычисление выражения, заданного в командной строке
    /// (перед любыми скриптами).
    /// </summary>
    [JsonPropertyName ("evaluate")]
    public string? EvaluateExpression { get; set; }

    /// <summary>
    /// Известные директивы.
    /// </summary>
    [JsonPropertyName ("directives")]
    public List<DirectiveBase> KnownDirectives { get; set; }

    /// <summary>
    /// Загрузка сборок перед началом разбора и выполнения скриптов.
    /// </summary>
    [JsonPropertyName ("assemblies")]
    public List<string> LoadAssemblies { get; set; }

    /// <summary>
    /// Файлы скриптов, подлежащие выполнению.
    /// </summary>
    [JsonPropertyName ("scripts")]
    public List<string> ScriptFiles { get; set; }

    /// <summary>
    /// Аргументы для скриптов.
    /// </summary>
    [JsonPropertyName ("arguments")]
    public List<string> ScriptArguments { get; set; }

    /// <summary>
    /// Запуск REPL.
    /// </summary>
    [JsonPropertyName ("repl")]
    public bool ReplMode { get; set; }

    /// <summary>
    /// Настройки токенайзера.
    /// </summary>
    [UsedImplicitly]
    [JsonPropertyName ("tokenizer")]
    public TokenizerSettings? TokenizerSettings { get; set; }

    /// <summary>
    /// Добавление пространств перед исполнением скриптов.
    /// </summary>
    [JsonPropertyName ("use-namespaces")]
    public List<string> UseNamespaces { get; set; }

    /// <summary>
    /// Основное приглашение к вводу.
    /// </summary>
    [JsonPropertyName ("main-prompt")]
    public string? MainPrompt { get; set; }

    /// <summary>
    /// Дополнительное приглашение к вводу.
    /// </summary>
    [JsonPropertyName ("secondary-prompt")]
    public string? SecondaryPrompt { get; set; }

    // /// <summary>
    // /// Грамматика.
    // /// </summary>
    // [JsonIgnore]
    // public IGrammar Grammar { get; set; }

    // /// <summary>
    // /// Токенайзер.
    // /// </summary>
    // [JsonIgnore]
    // public Tokenizer Tokenizer { get; set; }

    /// <summary>
    /// Нужно раскрасить исходный код скрипта?
    /// </summary>
    [JsonIgnore]
    public string? Highlight { get; set; }

    /// <summary>
    /// Режим Barsor.
    /// </summary>
    [JsonIgnore]
    public bool BarsorMode { get; set; }

    /// <summary>
    /// Выводить дамп токенов перед началом разбора программы.
    /// </summary>
    [JsonPropertyName ("dump-tokens")]
    public bool DumpTokens { get; set; }

    /// <summary>
    /// Не загружать настройки из файла рядом с интерпретатором.
    /// </summary>
    [JsonIgnore]
    public bool DontLoadSettings { get; set; }

    /// <summary>
    /// Вывести версию.
    /// </summary>
    [JsonIgnore]
    public bool PrintVersion { get; set; }

    /// <summary>
    /// Пути для поиска скриптов, модулей  и т. д.
    /// Инициализируется значением переменной окружения "BARSIK_PATH".
    /// </summary>
    [JsonPropertyName ("path")]
    public List<string> Paths { get; }

    /// <summary>
    /// Разрешение использовать оператор <c>new</c>.
    /// </summary>
    [JsonPropertyName ("allow-new")]
    public bool AllowNewOperator { get; set; }

    /// <summary>
    /// Список модулей, которые необходимо.
    /// </summary>
    [JsonPropertyName ("modules")]
    public List<ModuleDefinition>? ModulesToLoad { get; set; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public InterpreterSettings()
    {
        // Tokenizer = new Tokenizer();
        // Grammar = new Grammar();
        KnownDirectives = new ();
        LoadAssemblies = new ();
        ScriptFiles = new ();
        ScriptArguments = new ();
        UseNamespaces = new ();
        MainPrompt = "> ";
        SecondaryPrompt = "... ";
        Paths = new ();
        AllowNewOperator = true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Слияние настроек.
    /// </summary>
    public void Coalesce
        (
            InterpreterSettings other
        )
    {
        Sure.NotNull (other);

        StartDebugger = StartDebugger || other.StartDebugger;
        DebugParser = DebugParser || other.DebugParser;
        DumpAst = DumpAst || other.DumpAst;
        ReplMode = ReplMode || other.ReplMode;
        BarsorMode = BarsorMode || other.BarsorMode;
        DumpTokens = DumpTokens || other.DumpTokens;
        AllowNewOperator = AllowNewOperator && other.AllowNewOperator; // это не описка!
        VariableDumper ??= other.VariableDumper;
        EvaluateExpression ??= other.EvaluateExpression;
        MainPrompt ??= other.MainPrompt;
        SecondaryPrompt ??= other.SecondaryPrompt;
        TokenizerSettings ??= other.TokenizerSettings;
        Highlight ??= other.Highlight;

        if (KnownDirectives.Count is 0)
        {
            KnownDirectives.AddRange (other.KnownDirectives);
        }

        if (LoadAssemblies.Count is 0)
        {
            LoadAssemblies.AddRange (other.LoadAssemblies);
        }

        if (ScriptFiles.Count is 0)
        {
            ScriptFiles.AddRange (other.ScriptFiles);
        }

        if (UseNamespaces.Count is 0)
        {
            UseNamespaces.AddRange (other.UseNamespaces);
        }

        if (Paths.Count is 0)
        {
            Paths.AddRange (other.Paths);
        }
    }

    /// <summary>
    /// Применение аргументов командной строки
    /// к настройкам интерпретатора.
    /// </summary>
    public void FromCommandLine
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        if (args.Length == 0)
        {
            ReplMode = true;
        }

        var scriptArgs = false;
        for (var index = 0; index < args.Length; index++)
        {
            var arg = args[index];

            if (arg is "--settings" or "-s")
            {
                var loaded = FromFile (args[++index], false);
                Coalesce (loaded);
            }
            else if (arg is "--tokenizer")
            {
                var fileName = args[++index];
                TokenizerSettings = TokenizerSettings.Load (fileName);
            }
            else if (arg is "--version" or "-v")
            {
                PrintVersion = true;
            }
            else if (arg is "--dump-variables")
            {
                VariableDumper = new StandardDumper();
            }
            else if (arg is "--dump-ast")
            {
                DumpAst = true;
            }
            else if (arg is "--module")
            {
                var fileName = args[++index];
                var moduleDefinition = ModuleDefinition.Load (fileName);
                ModulesToLoad ??= new ();
                ModulesToLoad.Add (moduleDefinition);
            }
            else if (arg is "--grammar")
            {
                // var typeName = args[++index];
                // var grammarType = Type.GetType (typeName, true).ThrowIfNull();
                // Grammar = (IGrammar) Activator.CreateInstance (grammarType).ThrowIfNull();
            }
            else if (arg is "--repl" or "-r")
            {
                ReplMode = true;
            }
            else if (arg is "--debug-parser" or "-p")
            {
                DebugParser = true;
            }
            else if (arg is "--external")
            {
                // TODO установка обработчика внешнего кода
                index++;
            }
            else if (arg is "--use-namespace" or "-u")
            {
                UseNamespaces.Add (args[++index]);
            }
            else if (arg is "--load-assembly" or "-a")
            {
                LoadAssemblies.Add (args[++index]);
            }
            else if (arg is "--debugger" or "-d")
            {
                StartDebugger = true;
            }
            else if (arg is "--eval" or "-e")
            {
                EvaluateExpression = string.Join (' ', args.Skip (index + 1));
                break;
            }
            else if (arg is "--highlight-html")
            {
                Highlight = "html";
            }
            else if (arg is "--higlight-console" or "--highlight")
            {
                Highlight = "console";
            }
            else if (arg is "--barsor")
            {
                BarsorMode = true;
            }
            else if (arg is "--dump-tokens" or "-t")
            {
                DumpTokens = true;
            }
            else if (arg is "--include" or "-i" or "--path")
            {
                var value = args[++index];
                var items = value.Split
                    (
                        Path.PathSeparator,
                        StringSplitOptions.TrimEntries
                        | StringSplitOptions.RemoveEmptyEntries
                    );
                foreach (var item in items)
                {
                    Paths.Add (item);
                }
            }
            else if (arg is "--")
            {
                scriptArgs = true;
            }
            else if (arg.StartsWith ("-"))
            {
                if (scriptArgs)
                {
                    ScriptArguments.Add (args[++index]);
                }
                else
                {
                    throw new BarsikException ($"Unknown option {arg}");
                }
            }
            else
            {
                if (scriptArgs)
                {
                    ScriptArguments.Add (args[++index]);
                }
                else
                {
                    ScriptFiles.Add (arg);
                }
            }
        }
    }

    /// <summary>
    /// Получение настроек по умолчанию.
    /// </summary>
    public static InterpreterSettings CreateDefault()
    {
        var result = new InterpreterSettings();

        // result.Tokenizer = KotikUtility.CreateTokenizerForBarsik
        //     (
        //         result.TokenizerSettings
        //     );
        // result.Grammar = AM.Kotik.Barsik.Grammar.CreateDefaultBarsikGrammar();

        result.Paths.Add ("include");

        result.UseNamespaces.Add ("System");
        result.UseNamespaces.Add ("System.Collections.Generic");
        result.UseNamespaces.Add ("System.IO");
        result.UseNamespaces.Add ("System.Text");

        // директивы по умолчанию
        // result.KnownDirectives.Add (new AssemblyDirective());
        // result.KnownDirectives.Add (new AstDirective());
        // result.KnownDirectives.Add (new EchoDirective());
        // result.KnownDirectives.Add (new IncludeDirective());
        // result.KnownDirectives.Add (new ModuleDirective());
        // result.KnownDirectives.Add (new NamespaceDirective());
        // result.KnownDirectives.Add (new PathDirective());
        // result.KnownDirectives.Add (new ShebangDirective());
        // result.KnownDirectives.Add (new UseDirective());
        // result.KnownDirectives.Add (new VariableDirective());

        return result;
    }

    /// <summary>
    /// Загрузка настроек интерпретатора из файла.
    /// </summary>
    public static InterpreterSettings FromFile
        (
            string fileName,
            bool withDefaults = true
        )
    {
        Sure.FileExists (fileName);

        var loaded = JsonUtility.ReadObjectFromFile<InterpreterSettings> (fileName);
        if (withDefaults)
        {
            var defaults = CreateDefault();
            loaded.Coalesce (defaults);
        }

        return loaded;
    }

    /// <summary>
    /// Сохранение настроек интерпретатора в файле.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var text = JsonUtility.SerializeIndented (this);
        File.WriteAllText (fileName, text);
    }

    #endregion
}
