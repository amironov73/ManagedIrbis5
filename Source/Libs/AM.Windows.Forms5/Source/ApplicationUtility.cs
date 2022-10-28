// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ApplicationUtility.cs -- вспомогательные методы уровня приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Вспомогательные методы уровня приложения.
/// </summary>
public static class ApplicationUtility
{
    #region Private members

    /// <summary>
    /// Здесь мы считаем фоновые потоки для UI.
    /// </summary>
    private static int _uiThreadCounter;

    private static void DiscoverExceptions
        (
            Task task
        )
    {
        task.GetAwaiter().GetResult();
    }

    private static void _RunFormInNewThread
        (
            object? obj
        )
    {
        var form = (Form) obj.ThrowIfNull();

        form.Visible = true;
        form.ShowDialog();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// (Почти) неблокирующая задержка.
    /// </summary>
    public static async Task IdleDelay
        (
            int milliseconds = 20
        )
    {
        Sure.Positive (milliseconds, nameof (milliseconds));

        await Task.Delay (milliseconds);
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static void Run
        (
            Action action
        )
    {
        Sure.NotNull (action);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        using (var task = Task.Factory.StartNew (action))
        {
            WaitFor (task);
            DiscoverExceptions (task);
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static void Run<T>
        (
            Action<T> action,
            T argument
        )
    {
        void Interim() => action (argument);

        Sure.NotNull (action);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        using (var task = Task.Factory.StartNew (Interim))
        {
            WaitFor (task);
            DiscoverExceptions (task);
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static void Run<T1, T2>
        (
            Action<T1, T2> action,
            T1 argument1,
            T2 argument2
        )
    {
        void Interim() => action (argument1, argument2);

        Sure.NotNull (action);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        using (var task = Task.Factory.StartNew (Interim))
        {
            WaitFor (task);
            DiscoverExceptions (task);
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static void Run<T1, T2, T3>
        (
            Action<T1, T2, T3> action,
            T1 argument1,
            T2 argument2,
            T3 argument3
        )
    {
        void Interim() => action (argument1, argument2, argument3);

        Sure.NotNull (action);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        using (var task = Task.Factory.StartNew (Interim))
        {
            WaitFor (task);
            DiscoverExceptions (task);
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static TResult Run<TResult>
        (
            Func<TResult> func
        )
    {
        Sure.NotNull (func);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        TResult result;
        using (var task = Task<TResult>.Factory.StartNew (func))
        {
            WaitFor (task);
            DiscoverExceptions (task);
            result = task.Result;
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");

        return result;
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static TResult Run<T1, TResult>
        (
            Func<T1, TResult> func,
            T1 argument
        )
    {
        TResult Interim() => func (argument);

        Sure.NotNull (func);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        TResult result;
        using (var task = Task<TResult>.Factory.StartNew (Interim))
        {
            WaitFor (task);
            DiscoverExceptions (task);
            result = task.Result;
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");

        return result;
    }

    /// <summary>
    /// Запуск некоторого кода в псевдо-асинхронном режиме.
    /// </summary>
    public static TResult Run<T1, T2, TResult>
        (
            Func<T1, T2, TResult> func,
            T1 argument1,
            T2 argument2
        )
    {
        TResult Interim() => func (argument1, argument2);

        Sure.NotNull (func);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": entering");

        TResult result;
        using (var task = Task<TResult>.Factory.StartNew (Interim))
        {
            WaitFor (task);
            DiscoverExceptions (task);
            result = task.Result;
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (Run) + ": leaving");

        return result;
    }

    /// <summary>
    /// Запуск формы в новом потоке.
    /// </summary>
    public static void RunNewThread
        (
            this Form form
        )
    {
        Sure.NotNull (form);

        var start = new ParameterizedThreadStart (_RunFormInNewThread);
        var thread = new Thread (start)
        {
            IsBackground = true,
            Name = "UiThread" + ++_uiThreadCounter
        };
        thread.SetApartmentState (ApartmentState.STA);
        thread.Start (form);
    }

    /// <summary>
    /// Запуск нового UI-потока.
    /// </summary>
    public static Thread RunNewUiThread
        (
            ThreadStart start
        )
    {
        Sure.NotNull (start);

        var result = new Thread (start)
        {
            IsBackground = true,
            Name = "UiThread" + ++_uiThreadCounter
        };
        result.SetApartmentState (ApartmentState.STA);
        result.Start();

        return result;
    }

    /// <summary>
    /// Небольшое ожидание.
    /// </summary>
    public static async Task SleepALittle()
    {
        Application.DoEvents();
        await IdleDelay();
    }

    /// <summary>
    /// Ожидание установки некоторого флага.
    /// </summary>
    public static void WaitFor
        (
            ref bool readyFlag
        )
    {
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": entering");

        while (!readyFlag)
        {
            SleepALittle().Wait();
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": leaving");
    }

    /// <summary>
    /// Ожидание установки некоторого флага.
    /// </summary>
    public static void WaitFor
        (
            Func<bool> readyCheck
        )
    {
        Sure.NotNull (readyCheck);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": entering");

        while (!readyCheck())
        {
            SleepALittle().Wait();
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": leaving");
    }

    /// <summary>
    /// Wait for flag.
    /// </summary>
    public static void WaitFor<T>
        (
            Func<T, bool> readyCheck,
            T argument
        )
        where T: class
    {
        Sure.NotNull (readyCheck);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": entering");

        while (!readyCheck (argument))
        {
            SleepALittle().Wait();
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": leaving");
    }

    /// <summary>
    /// Ожидание завершения указанной задачи.
    /// </summary>
    public static void WaitFor
        (
            this IAsyncResult handle
        )
    {
        Sure.NotNull (handle);
        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": entering");

        while (!handle.IsCompleted)
        {
            SleepALittle().Wait();
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": leaving");
    }

    /// <summary>
    /// Ожидание завершения нескольких задач.
    /// </summary>
    public static void WaitFor
        (
            bool waitAll,
            params WaitHandle[] handles
        )
    {
        Sure.NotNull (handles);

        if (handles.Length == 0)
        {
            return;
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": entering");

        bool complete;
        do
        {
            complete = waitAll
                ? WaitHandle.WaitAll (handles, 0)
                : WaitHandle.WaitAny (handles, 0) >= 0;

            if (!complete)
            {
                SleepALittle().Wait();
            }
        } while (!complete);

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": leaving");
    }

    /// <summary>
    /// Ожидание завершения нескольких задач.
    /// </summary>
    public static void WaitFor
        (
            bool waitAll,
            params Task[] tasks
        )
    {
        Sure.NotNull (tasks);

        if (tasks.Length == 0)
        {
            return;
        }

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": entering");
        bool complete;
        do
        {
            complete = waitAll
                ? Task.WaitAll (tasks, 0)
                : Task.WaitAny (tasks, 0) >= 0;

            if (!complete)
            {
                SleepALittle().Wait();
            }
        } while (!complete);

        Magna.Logger.LogTrace (nameof (ApplicationUtility) + "::" + nameof (WaitFor) + ": leaving");
    }

    /// <summary>
    /// Determines whether secondary screen present.
    /// </summary>
    /// <value><c>true</c> if secondary screen present;
    /// otherwise, <c>false</c>.</value>
    public static bool HaveSecondaryScreen => SecondaryScreen is not null;

    /// <summary>
    /// Gets the secondary screen.
    /// </summary>
    /// <value>The secondary screen or <c>null</c> if none.
    /// </value>
    public static Screen? SecondaryScreen
    {
        get
        {
            foreach (var screen in Screen.AllScreens)
            {
                if (!screen.Primary)
                {
                    return screen;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Moves given form window to the primary screen.
    /// </summary>
    public static void MoveToPrimaryScreen
        (
            Form form
        )
    {
        if (Screen.PrimaryScreen is { } primaryScreen)
        {
            MoveToScreen (primaryScreen, form);
        }
    }

    /// <summary>
    /// Moves form window to given screen.
    /// </summary>
    public static void MoveToScreen
        (
            Screen screen,
            Form form
        )
    {
        form.Location = screen.WorkingArea.Location;
    }

    /// <summary>
    /// Moves given form window to secondary screen (if present).
    /// </summary>
    /// <param name="form">The form.</param>
    /// <returns><c>true</c> on success, <c>false</c>
    /// on failure (e.g. no secondary screen present).</returns>
    public static bool MoveToSecondaryScreen
        (
            Form form
        )
    {
        var secondaryScreen = SecondaryScreen;
        if (secondaryScreen is not null)
        {
            MoveToScreen (secondaryScreen, form);
            return true;
        }

        return false;
    }

    #endregion
}
