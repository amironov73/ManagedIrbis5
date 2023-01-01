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

#region Using directives

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    partial class Report
    {
        #region Private Methods

        private void ClearPreparedPages()
        {
            if (PreparedPages != null)
            {
                PreparedPages.Clear();
            }
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="password"></param>
        /// <returns>password</returns>
        private string ShowPaswordForm (string password)
        {
            return password;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="report"></param>
        partial void SerializeDesign (ReportWriter writer, Report report);

        /// <summary>
        /// Does nothing
        /// </summary>
        partial void InitDesign();

        /// <summary>
        /// Does nothing
        /// </summary>
        partial void ClearDesign();

        /// <summary>
        /// Does nothing
        /// </summary>
        partial void DisposeDesign();

        #endregion Private Methods
    }
}
