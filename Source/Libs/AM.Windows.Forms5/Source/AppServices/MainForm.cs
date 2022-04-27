// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.AppServices;

/// <summary>
/// Главная форма приложения.
/// </summary>
public class MainForm
    : Form
{
    #region Properties

    /// <summary>
    /// Код возврата для приложения.
    /// </summary>
    public int ReturnCode { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MainForm()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="action">Действие, которое необходимо выполнить
    /// сразу после инициализации формы.</param>
    public MainForm
        (
            Action action
        )
        : this()
    {
        Sure.NotNull (action);

        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action();
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание формы с выполнением действия сразу после ее инициализации.
    /// </summary>
    public MainForm Create<T>
        (
            Action<T> action,
            T arg1
        )
    {
        Sure.NotNull (action);

        var result = new MainForm();
        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action (arg1);
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;

        return result;
    }

    /// <summary>
    /// Создание формы с выполнением действия сразу после ее инициализации.
    /// </summary>
    public MainForm Create<T1, T2>
        (
            Action<T1, T2> action,
            T1 arg1,
            T2 arg2
        )
    {
        Sure.NotNull (action);

        var result = new MainForm();
        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action (arg1, arg2);
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;

        return result;
    }

    /// <summary>
    /// Создание формы с выполнением действия сразу после ее инициализации.
    /// </summary>
    public MainForm Create<T1, T2, T3>
        (
            Action<T1, T2, T3> action,
            T1 arg1,
            T2 arg2,
            T3 arg3
        )
    {
        Sure.NotNull (action);

        var result = new MainForm();
        var timer = new Timer()
        {
            Interval = 10
        };
        timer.Tick += (_, _) =>
        {
            action (arg1, arg2, arg3);
            timer.Enabled = false;
            timer.Dispose();
        };
        timer.Enabled = true;

        return result;
    }

    /// <summary>
    /// Добавление списка для вывода логов.
    /// </summary>
    public void AddLogBox()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Закрытие главной формы приложения с указанным кодом возврата.
    /// </summary>
    public void Close
        (
            int returnCode
        )
    {
        ReturnCode = returnCode;

        Close();
    }

    #endregion
}

