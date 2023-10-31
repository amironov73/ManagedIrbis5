// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

#region Using directives

using System;
using System.Threading.Tasks;

using Avalonia.Input;

#endregion

namespace AM.Avalonia;

/// <summary>
/// Устанавливает курсор "песочные часы" для длительной операции.
/// </summary>
public sealed class WaitCursor
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Элемент, для которого устанавливается курсор ожидания.
    /// </summary>
    public InputElement Element { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WaitCursor
        (
            InputElement element
        )
    {
        Element = element;
        while (true)
        {
            var parent = Element.Parent as InputElement;
            if (parent is null)
            {
                break;
            }

            Element = parent;
        }

        _previousCursor = element.Cursor ?? Cursor.Default;
        _previousState = element.IsEnabled;
        Element.Cursor = new Cursor (StandardCursorType.Wait);
    }

    #endregion

    #region Previous cursor

    private readonly Cursor _previousCursor;
    private readonly bool _previousState;


    #endregion

    #region Public methods

    /// <summary>
    /// Запуск асинхронного действия с показом "песочных часов".
    /// </summary>
    public static async Task RunActionAsync
        (
            InputElement element,
            Func<Task> action
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        await action();
    }

    /// <summary>
    /// Запуск асинхронного действия с показом "песочных часов".
    /// </summary>
    public static async Task RunActionAsync<T1>
        (
            InputElement element,
            Func<T1, Task> action,
            T1 argument1
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        await action (argument1);
    }

    /// <summary>
    /// Запуск асинхронного действия с показом "песочных часов".
    /// </summary>
    public static async Task RunActionAsync<T1, T2>
        (
            InputElement element,
            Func<T1, T2, Task> action,
            T1 argument1,
            T2 argument2
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        await action (argument1, argument2);
    }

    /// <summary>
    /// Запуск асинхронного действия с показом "песочных часов".
    /// </summary>
    public static async Task RunActionAsync<T1, T2, T3>
        (
            InputElement element,
            Func<T1, T2, T3, Task> action,
            T1 argument1,
            T2 argument2,
            T3 argument3
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        await action (argument1, argument2, argument3);
    }

    /// <summary>
    /// Запуск асинхронной функции с показом "песочных часов".
    /// </summary>
    public static async Task<TResult> RunFuncAsync<TResult>
        (
            InputElement element,
            Func<Task<TResult>> action
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        return await action();
    }

    /// <summary>
    /// Запуск асинхронной функции с показом "песочных часов".
    /// </summary>
    public static async Task<TResult> RunFuncAsync<T1, TResult>
        (
            InputElement element,
            Func<T1, Task<TResult>> action,
            T1 argument1
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        return await action (argument1);
    }

    /// <summary>
    /// Запуск асинхронной функции с показом "песочных часов".
    /// </summary>
    public static async Task<TResult> RunFuncAsync<T1, T2, TResult>
        (
            InputElement element,
            Func<T1, T2, Task<TResult>> action,
            T1 argument1,
            T2 argument2
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        return await action (argument1, argument2);
    }

    /// <summary>
    /// Запуск асинхронной функции с показом "песочных часов".
    /// </summary>
    public static async Task<TResult> RunFuncAsync<T1, T2, T3, TResult>
        (
            InputElement element,
            Func<T1, T2, T3, Task<TResult>> action,
            T1 argument1,
            T2 argument2,
            T3 argument3
        )
    {
        Sure.NotNull (element);
        Sure.NotNull (action);

        using var cursor = new WaitCursor (element);
        return await action (argument1, argument2, argument3);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Element.Cursor = _previousCursor;
        Element.IsEnabled = _previousState;
    }

    #endregion
}
