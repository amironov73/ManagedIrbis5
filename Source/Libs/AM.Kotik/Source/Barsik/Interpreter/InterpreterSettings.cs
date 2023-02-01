// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* InterpreterSettings.cs -- настройки интерпретатора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using AM.Kotik.Barsik.Diagnostics;
using AM.Kotik.Barsik.Directives;
using AM.Kotik.Tokenizers;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Настройки интерпретатора.
/// </summary>
public sealed class InterpreterSettings
{
    #region Properties

    /// <summary>
    /// Запускать отладчик.
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
    public IBarsikDumper? VariableDumper { get; set; }

    /// <summary>
    /// Вычисление выражения, заданного в командной строке
    /// (перед любыми скриптами).
    /// </summary>
    [JsonPropertyName ("evaluate-expression")]
    public string? EvaluateExpression { get; set; }

    /// <summary>
    /// Известные директивы.
    /// </summary>
    public List<DirectiveBase> KnownDirectives { get; set; }

    /// <summary>
    /// Загрузка сборок перед началом разбора и выполнения скриптов.
    /// </summary>
    [JsonPropertyName ("load-assemblies")]
    public List<string> LoadAssemblies { get; set; }

    /// <summary>
    /// Файлы скриптов, подлежащие выполнению.
    /// </summary>
    public List<string> ScriptFiles { get; set; }

    /// <summary>
    /// Запуск REPL.
    /// </summary>
    [JsonPropertyName ("repl")]
    public bool ReplMode { get; set; }

    /// <summary>
    /// Настройки токенайзера.
    /// </summary>
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

    /// <summary>
    /// Грамматика.
    /// </summary>
    [JsonIgnore]
    public IGrammar Grammar { get; set; }

    /// <summary>
    /// Токенайзер.
    /// </summary>
    [JsonIgnore]
    public Tokenizer Tokenizer { get; set; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public InterpreterSettings()
    {
        Tokenizer = new Tokenizer();
        Grammar = new Grammar();
        KnownDirectives = new ();
        LoadAssemblies = new ();
        ScriptFiles = new ();
        UseNamespaces = new ();
        MainPrompt = "> ";
        SecondaryPrompt = "... ";
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение настроек по умолчанию.
    /// </summary>
    public static InterpreterSettings CreateDefault()
    {
        var result = new InterpreterSettings();

        result.Tokenizer = KotikUtility.CreateTokenizerForBarsik (result.TokenizerSettings);
        result.Grammar = AM.Kotik.Barsik.Grammar.CreateDefaultBarsikGrammar();

        // директивы по умолчанию
        result.KnownDirectives.Add (new AssemblyDirective());
        result.KnownDirectives.Add (new AstDirective());
        result.KnownDirectives.Add (new EchoDirective());
        result.KnownDirectives.Add (new ModuleDirective());
        result.KnownDirectives.Add (new NamespaceDirective());
        result.KnownDirectives.Add (new VariableDirective());

        return result;
    }

    /// <summary>
    /// Получение настроек из аргументов командной строки.
    /// </summary>
    public static InterpreterSettings FromCommandLine
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        var result = CreateDefault();
        if (args.Length == 0)
        {
            result.ReplMode = true;
        }

        for (var index = 0; index < args.Length; index++)
        {
            var arg = args[index];

            if (arg == "--dump-variables")
            {
                result.VariableDumper = new StandardDumper();
            }
            else if (arg == "--dump-ast")
            {
                result.DumpAst = true;
            }
            else if (arg == "--grammar")
            {
                // TODO установка нестандартной грамматики
                index++;
            }
            else if (arg == "--repl")
            {
                result.ReplMode = true;
            }
            else if (arg == "--debug-parser")
            {
                result.DebugParser = true;
            }
            else if (arg == "--external")
            {
                // TODO установка обработчика внешнего кода
                index++;
            }
            else if (arg == "--use-namespace")
            {
                result.UseNamespaces.Add (args[index + 1]);
                index++;
            }
            else if (arg == "--load-assembly")
            {
                result.LoadAssemblies.Add (args[index + 1]);
                index++;
            }
            else if (arg == "--debugger")
            {
                result.StartDebugger = true;
            }
            else if (arg == "--eval")
            {
                result.EvaluateExpression = string.Join (' ', args.Skip (index + 1));
                break;
            }
            else if (arg.StartsWith ("-"))
            {
                throw new BarsikException ($"Unknown option {arg}");
            }
            else
            {
                result.ScriptFiles.Add (arg);
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка настроек из файла.
    /// </summary>
    public static InterpreterSettings Load
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        throw new NotImplementedException();
    }

    #endregion
}
