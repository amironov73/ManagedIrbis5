// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* InsistentFile.cs -- настойчивое создание/удаление файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Настойчивое создание/удаление файлов.
    /// </summary>
    public static class InsistentFile
    {
        #region Constants

        /// <summary>
        /// Default value for <see cref="Timeout"/> property.
        /// </summary>
        public const int DefaultTimeout = 10 * 1000;

        /// <summary>
        /// Default value for <see cref="SleepInterval"/> property.
        /// </summary>
        public const int DefaultSleep = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Interval to sleep between subsequent attempts,
        /// milliseconds.
        /// </summary>
        public static int SleepInterval { get; set; }

        /// <summary>
        /// Timeout for operations, milliseconds.
        /// </summary>
        public static int Timeout { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        static InsistentFile()
        {
            SleepInterval = DefaultSleep;
            Timeout = DefaultTimeout;
        }

        #endregion

        #region Private members

        private static T _Evaluate<T>
            (
                Func<T> function
            )
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                Exception lastException;

                try
                {
                    var result = function();

                    return result;
                }
                catch (Exception exception)
                {
                    lastException = exception;

                    Magna.TraceException
                        (
                            nameof(InsistentFile) + "::" + nameof(_Evaluate),
                            exception
                        );
                }

                var elapsed = (int)stopwatch.ElapsedMilliseconds;
                if (elapsed > Timeout)
                {
                    Magna.Error
                        (
                            nameof(InsistentFile) + "::" + nameof(_Evaluate)
                            + ": timeout is exhausted"
                        );

                    throw new ArsMagnaException
                        (
                            nameof(InsistentFile) + ": timeout is exhausted",
                            lastException
                        );
                }

                Thread.Sleep(SleepInterval);
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc
        /// cref="File.Open(string,FileMode,FileAccess,FileShare)" />.
        public static FileStream Open
            (
                string path,
                FileMode mode,
                FileAccess access,
                FileShare share
            )
        {
            FileStream Func() => File.Open(path, mode, access, share);

            FileStream result = _Evaluate(Func);

            return result;
        }

        /// <summary>
        /// Open specified file for exclusive reading.
        /// </summary>
        public static FileStream OpenForExclusiveRead
            (
                string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                );
        }

        /// <summary>
        /// Open specified file for exclusive writing.
        /// </summary>
        public static FileStream OpenForExclusiveWrite
            (
                string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
        }

        /// <summary>
        /// Open specified file for shared reading.
        /// </summary>
        public static FileStream OpenForSharedRead
            (
                string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );
        }

        /// <summary>
        /// Open specified file for shared reading/writing.
        /// </summary>
        public static FileStream OpenForSharedWrite
            (
                string path
            )
        {
            return Open
                (
                    path,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite
                );
        }

        #endregion
    }
}
