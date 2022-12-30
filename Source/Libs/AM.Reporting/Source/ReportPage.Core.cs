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
    partial class ReportPage
    {
        #region Private Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="reportPage"></param>
        partial void AssignPreview (ReportPage reportPage);

        /// <summary>
        /// Does nothing
        /// </summary>
        partial void InitPreview();

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="reportPage"></param>
        partial void WritePreview (FRWriter writer, ReportPage reportPage);

        #endregion Private Methods
    }
}
