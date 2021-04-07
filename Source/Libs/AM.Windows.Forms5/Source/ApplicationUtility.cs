// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
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

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Вспомогательные методы уровня приложения.
    /// </summary>
    public static class ApplicationUtility
    {
        #region Private members

        private static void DiscoverExceptions
            (
                Task task
            )
        {
            task.GetAwaiter().GetResult();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// (Almost) non-blocking Delay.
        /// </summary>
        public static async Task IdleDelay
            (
                int milliseconds = 20
            )
        {
            Sure.Positive(milliseconds, nameof(milliseconds));

            await Task.Delay(milliseconds);

        } // method IdleDelay

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run
            (
                Action action
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            using (var task = Task.Factory.StartNew(action))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );
        } // method Run

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run<T>
            (
                Action<T> action,
                T argument
            )
        {
            void Interim() => action(argument);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            using (var task = Task.Factory.StartNew(Interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );
        } // method Run

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run<T1, T2>
            (
                Action<T1, T2> action,
                T1 argument1,
                T2 argument2
            )
        {
            void Interim() => action(argument1, argument2);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            using (var task = Task.Factory.StartNew(Interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );
        } // method Run

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run<T1, T2, T3>
            (
                Action<T1, T2, T3> action,
                T1 argument1,
                T2 argument2,
                T3 argument3
            )
        {
            void Interim() => action(argument1, argument2, argument3);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            using (var task = Task.Factory.StartNew(Interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );
        } // method Run

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static TResult Run<TResult>
            (
                Func<TResult> func
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            TResult result;
            using (var task = Task<TResult>.Factory.StartNew(func))
            {
                WaitFor(task);
                DiscoverExceptions(task);
                result = task.Result;
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );

            return result;
        } // method Run

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static TResult Run<T1, TResult>
            (
                Func<T1, TResult> func,
                T1 argument
            )
        {
            TResult Interim() => func(argument);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            TResult result;
            using (var task = Task<TResult>.Factory.StartNew(Interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
                result = task.Result;
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );

            return result;
        } // methor Run

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static TResult Run<T1, T2, TResult>
            (
                Func<T1, T2, TResult> func,
                T1 argument1,
                T2 argument2
            )
        {
            TResult Interim() => func(argument1, argument2);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": entering"
                );

            TResult result;
            using (var task = Task<TResult>.Factory.StartNew(Interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
                result = task.Result;
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(Run)
                    + ": leaving"
                );

            return result;
        } // method Run

        /// <summary>
        /// Небольшое ожидание
        /// </summary>
        public static async Task SleepALittle()
        {
            Application.DoEvents();
            await IdleDelay();
        } // method SleepALittle

        /// <summary>
        /// Wait for flag.
        /// </summary>
        public static void WaitFor
            (
                ref bool readyFlag
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": entering"
                );

            while (!readyFlag)
            {
                SleepALittle().Wait();
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": leaving"
                );
        } // method WaitFor

        /// <summary>
        /// Wait for flag.
        /// </summary>
        public static void WaitFor
            (
                Func<bool> readyCheck
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": entering"
                );

            while (!readyCheck())
            {
                SleepALittle().Wait();
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": leaving"
                );
        } // method WaitFor

        /// <summary>
        /// Wait for flag.
        /// </summary>
        public static void WaitFor<T>
            (
                Func<T, bool> readyCheck,
                T argument
            )
            where T : class
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": entering"
                );

            while (!readyCheck(argument))
            {
                SleepALittle().Wait();
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": leaving"
                );
        } // method WaitFor

        /// <summary>
        /// Wait for task.
        /// </summary>
        public static void WaitFor
            (
                IAsyncResult handle
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": entering"
                );

            while (!handle.IsCompleted)
            {
                SleepALittle().Wait();
            }

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": leaving"
                );
        } // method WaitFor

        /// <summary>
        /// Wait for some tasks.
        /// </summary>
        public static void WaitFor
            (
                bool waitAll,
                params WaitHandle[] handles
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": entering"
                );

            bool complete;
            do
            {
                complete = waitAll
                    ? WaitHandle.WaitAll(handles, 0)
                    : WaitHandle.WaitAny(handles, 0) >= 0;

                if (!complete)
                {
                    SleepALittle().Wait();
                }

            } while (!complete);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": leaving"
                );
        } // method WaitFor

        /// <summary>
        /// Wait for some tasks.
        /// </summary>
        public static void WaitFor
            (
                bool waitAll,
                params Task[] tasks
            )
        {
            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": entering"
                );

            bool complete;
            do
            {
                complete = waitAll
                    ? Task.WaitAll(tasks, 0)
                    : Task.WaitAny(tasks, 0) >= 0;

                if (!complete)
                {
                    SleepALittle().Wait();
                }
            }
            while (!complete);

            Magna.Trace
                (
                    nameof(ApplicationUtility) + "::" + nameof(WaitFor)
                    + ": leaving"
                );
        } // method WaitFor

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
                foreach (Screen screen in Screen.AllScreens)
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
            MoveToScreen(Screen.PrimaryScreen, form);
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
            Screen secondaryScreen = SecondaryScreen;
            if (secondaryScreen != null)
            {
                MoveToScreen(secondaryScreen, form);
                return true;
            }

            return false;
        }

        #endregion

    } // class ApplicationUtility

} // namespace AM.Windows.Forms
