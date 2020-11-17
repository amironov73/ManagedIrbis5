// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TestingPlatformAbstraction.cs -- заглушки для юнит-тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

#endregion

#nullable enable

namespace AM.PlatformAbstraction
{
    /// <summary>
    /// Заглушки для юнит-тестирования.
    /// </summary>
    public class TestingPlatformAbstraction
        : PlatformAbstractionLayer
    {
        #region Properties

        /// <summary>
        /// <see cref="Exit"/> method called?
        /// </summary>
        public bool ExitFlag { get; set; }

        /// <summary>
        /// <see cref="FailFast"/> method called?
        /// </summary>
        public bool FailFastFlag { get; set; }

        /// <summary>
        /// Value to use in <see cref="Now"/> function.
        /// </summary>
        public DateTime NowValue { get; set; }

        /// <summary>
        /// Environment variables.
        /// </summary>
        public CaseInsensitiveDictionary<string> Variables { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestingPlatformAbstraction()
        {
            Variables = new CaseInsensitiveDictionary<string>();
        }

        #endregion

        #region PlatformAbstractionLevel members

        /// <inheritdoc cref="PlatformAbstractionLayer.GetEnvironmentVariable" />
        public override string? GetEnvironmentVariable
            (
                string variableName
            )
        {
            Variables.TryGetValue(variableName, out string? result);

            return result;
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.GetMachineName" />
        public override string GetMachineName()
        {
            return "MACHINE";
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.GetRandomGenerator" />
        public override Random GetRandomGenerator()
        {
            return new Random(123);
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.Now" />
        public override DateTime Now()
        {
            return NowValue;
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.Exit" />
        public override void Exit(int exitCode)
        {
            ExitFlag = true;
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.FailFast" />
        public override void FailFast(string message)
        {
            FailFastFlag = true;
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.OsVersion" />
        public override OperatingSystem OsVersion()
        {
            Version windows7sp1 = new Version(6, 1, 7601, 65536);
            OperatingSystem result = new OperatingSystem
                (
                    PlatformID.Win32NT,
                    windows7sp1
                );

            return result;
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.Today" />
        public override DateTime Today()
        {
            return NowValue.Date;
        }

        #endregion
    }
}
