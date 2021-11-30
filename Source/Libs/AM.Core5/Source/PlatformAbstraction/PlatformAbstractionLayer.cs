// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PlatformAbstraction.cs -- абстрагируемся от платформы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.PlatformAbstraction
{
    /// <summary>
    /// Абстрагируемся от платформы.
    /// </summary>
    public class PlatformAbstractionLayer
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Current PAL.
        /// </summary>
        public static PlatformAbstractionLayer Current = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Exit.
        /// </summary>
        public virtual void Exit
            (
                int exitCode
            )
        {
            Environment.Exit (exitCode);
        }

        /// <summary>
        /// Fail fast.
        /// </summary>
        public virtual void FailFast
            (
                string message
            )
        {
            Environment.FailFast (message);
        }

        /// <summary>
        /// Get environment variable.
        /// </summary>
        public virtual string? GetEnvironmentVariable
            (
                string variableName
            )
        {
            return Environment.GetEnvironmentVariable (variableName);
        }

        /// <summary>
        /// Get the machine name.
        /// </summary>
        public virtual string GetMachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Get random number generator.
        /// </summary>
        public virtual Random GetRandomGenerator()
        {
            return new Random();
        }

        /// <summary>
        /// Get current date and time.
        /// </summary>
        public virtual DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Get the operating system version.
        /// </summary>
        public virtual OperatingSystem OsVersion()
        {
            return Environment.OSVersion;
        }

        /// <summary>
        /// Get today date.
        /// </summary>
        public virtual DateTime Today()
        {
            return DateTime.Today;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
