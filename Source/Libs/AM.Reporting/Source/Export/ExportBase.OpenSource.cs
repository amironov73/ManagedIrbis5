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

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Export
{
    partial class ExportBase
    {
        #region Private Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="int0"></param>
        partial void ShowPerformance(int int0);

        protected ReportPage GetOverlayPage(ReportPage page)
        {
            return page;
        }

        private int GetPagesCount(List<int> pages)
        {
            return pages.Count;
        }

        internal const bool HAVE_TO_WORK_WITH_OVERLAY = false;

        #endregion Private Methods
    }
}
