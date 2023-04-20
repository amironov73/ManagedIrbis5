// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CommonContext.cs -- общая часть контекста исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Kotik.Barsik.Ast;
using AM.Kotik.Barsik.Diagnostics;
using AM.Kotik.Types;

using JetBrains.Annotations;

#endregion

#nullable enable


namespace AM.Kotik.Barsik;

/// <summary>
/// Общая часть контекста исполнения скрипта.
/// </summary>
[PublicAPI]
public sealed class CommonContext
{
    #region Properties

    /// <summary>
    /// Выходной поток, ассоциированный с интерпретатором.
    /// </summary>
    public TextWriter? Output { get; set; }

    /// <summary>
    /// Поток ошибок, ассоциированный с интерпретатором.
    /// </summary>
    public TextWriter? Error { get; set; }

    /// <summary>
    /// Входной поток, ассоциированный с интерпретатором.
    /// </summary>
    public TextReader? Input { get; set; }

    /// <summary>
    /// Настройки интерпретатора.
    /// Применяются перед началом разбора и исполнения скрипта.
    /// </summary>
    public InterpreterSettings Settings { get; set; } = null!;

    /// <summary>
    /// Произвольные пользовательские данные, свяазанные
    /// с контекстом выполнения скрипта.
    /// </summary>
    public BarsikDictionary Auxiliary { get; } = new ();

    /// <summary>
    /// Загруженные модули.
    /// </summary>
    public List<IBarsikModule> Modules { get; } = new ();

    /// <summary>
    /// Аргументы для скрипта.
    /// </summary>
    public List<string> Arguments { get; } = new ();

    /// <summary>
    /// Дефайны.
    /// </summary>
    public Dictionary<string, dynamic?> Defines { get; } = new ();

    /// <summary>
    /// Отладчик скрипта.
    /// </summary>
    public IBarsikDebugger? ScriptDebugger { get; set; }

    /// <summary>
    /// Поток для отладочного вывода при парсинге скрипта.
    /// </summary>
    public TextWriter? ParsingDebugOutput { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    public Dictionary<string, object?> UserData { get; } = new ();

    /// <summary>
    /// Разрешает типы и члены классов.
    /// </summary>
    public IResolver Resolver { get; set; } = null!;

    /// <summary>
    /// Обработчик внешнего кода.
    /// </summary>
    public ExternalCodeHandler? ExternalCodeHandler { get; set; }

    /// <summary>
    /// Здесь запоминаются вложенные скрипты.
    /// </summary>
    public Dictionary<string, ProgramNode> Inclusions { get; }= new
        (
            OperatingSystem.IsWindows()
                ? StringComparer.InvariantCultureIgnoreCase
                : StringComparer.InvariantCulture
        );

    #endregion
}
