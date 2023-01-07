// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Reporting.Engine
{
    partial class ReportEngine
    {
        private void InitializePages()
        {
            for (var i = 0; i < Report.Pages.Count; i++)
            {
                if (Report.Pages[i] is ReportPage page)
                {
                    PreparedPages.AddSourcePage (page);
                }
            }
        }

        partial void TranslateObjects (BandBase parentBand);

        internal void TranslatedObjectsToBand (BandBase band)
        {
            // Avoid compilation errors
        }
    }
}
