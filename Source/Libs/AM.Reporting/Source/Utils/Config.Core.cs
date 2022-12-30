// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Reporting.Utils
{
    static partial class Config
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating that the ASP.NET hosting permission level is set to full trust.
        /// </summary>
        public static bool FullTrust => true;

        /// <summary>
        /// Gets a value that determines whether to disable some functionality to run in web mode.
        /// </summary>
        /// <remarks>
        /// Use this property if you use AM.Reporting in ASP.Net. Set this property to <b>true</b> <b>before</b>
        /// you access any AM.Reporting.Net objects.
        /// </remarks>
        public static bool WebMode { get; set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        static partial void RestoreUIStyle();

        /// <summary>
        /// Does nothing
        /// </summary>
        static partial void SaveUIStyle();

        static partial void RestorePreviewSettings();

        static partial void SavePreviewSettings();

        static partial void SaveExportOptions();

        static partial void SaveAuthServiceUser();

        static partial void RestoreAuthServiceUser();

        private static void RestoreExportOptions()
        {
            var options = ExportsOptions.GetInstance();
            options.RestoreExportOptions();
        }

        #endregion Private Methods

        internal static void DoEvent()
        {
            // do nothing
        }
    }
}
