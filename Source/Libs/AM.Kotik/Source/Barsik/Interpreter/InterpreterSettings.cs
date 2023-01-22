// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* InterpreterSettings.cs -- настройки интерпретатора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Text.Json.Serialization;

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
    /// Отладочная печать во время разбора скрипта.
    /// </summary>
    [JsonPropertyName ("debug-pa")]
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
    public bool DumpVariables { get; set; }

    /// <summary>
    /// Вычисление выражения, заданного в командной строке
    /// (перед любыми скриптами).
    /// </summary>
    [JsonPropertyName ("evaluate-expression")]
    public string? EvaluateExpression { get; set; }
    
    /// <summary>
    /// Загрузка сборок перед началом разбора и выполнения скриптов.
    /// </summary>
    [JsonPropertyName ("load-assemblies")]
    public string[]? LoadAssemblies { get; set; }


    /// <summary>
    /// Запуск REPL.
    /// </summary>
    [JsonPropertyName ("repl")]
    public bool ReplMode { get; set; }

    /// <summary>
    /// Настройки токенайзера.
    /// </summary>
    [JsonPropertyName ("tokenizer")]
    public TokenizerSettings TokenizerSettings { get; set; }

    /// <summary>
    /// Добавление пространств перед исполнением скриптов.
    /// </summary>
    [JsonPropertyName ("use-namespaces")]
    public string[]? UseNamespaces{ get; set; }

    #endregion

    #region Construciton

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public InterpreterSettings()
    {
        TokenizerSettings = TokenizerSettings.CreateDefault();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение настроек по умолчанию.
    /// </summary>
    public static InterpreterSettings CreateDefault()
    {
        return new InterpreterSettings();
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
