﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IBarsikDebugger.cs -- интерфейс отладчика для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Lexey.Barsik.Ast;

#endregion

namespace AM.Lexey.Barsik.Diagnostics;

/// <summary>
/// Интерфейс отладчика для Барсика.
/// </summary>
public interface IBarsikDebugger
{
    /// <summary>
    /// Точки останова.
    /// </summary>
    Dictionary<StatementBase, Breakpoint> Breakpoints { get; }

    /// <summary>
    /// Загрузка файла скрипта с немедленным запуском на выполнение.
    /// </summary>
    ExecutionResult ExecuteFile (string filename);

    /// <summary>
    /// Вычисление значения переменной.
    /// </summary>
    void Evaluate (string? expression);

    /// <summary>
    /// Загрузка скрипта.
    /// </summary>
    void Load (string? fileName);

    /// <summary>
    /// Выполнение одного стейтмента без захода в функции.
    /// </summary>
    void Next();

    /// <summary>
    /// Пре-трассировка.
    /// </summary>
    void PreTrace
        (
            Context context,
            StatementBase statement
        );

    /// <summary>
    /// Вывод переменной.
    /// </summary>
    void Print (string? what);

    /// <summary>
    /// Пробуждение отладчика при наступлении какого-нибудь события.
    /// </summary>
    void Raise
        (
            Context context,
            StatementBase? statement
        );

    /// <summary>
    /// Запуск скрипта на выполнение до конца или до первой точки останова.
    /// </summary>
    void Run();

    /// <summary>
    /// Задание точки останова.
    /// </summary>
    void SetBreakPoint (int line);

    /// <summary>
    /// Выполнение одного стейтмента с заходом в функции.
    /// </summary>
    void Step();

    /// <summary>
    /// Трассировка.
    /// </summary>
    void Trace
        (
            Context context,
            StatementBase statement
        );
}
