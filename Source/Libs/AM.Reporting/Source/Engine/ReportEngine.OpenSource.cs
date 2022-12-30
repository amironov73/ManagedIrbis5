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

namespace AM.Reporting.Engine
{
    partial class ReportEngine
    {
        private void InitializePages()
        {
            for (int i = 0; i < Report.Pages.Count; i++)
            {
                ReportPage page = Report.Pages[i] as ReportPage;
                if (page != null)
                    PreparedPages.AddSourcePage(page);
            }
        }

        partial void TranslateObjects(BandBase parentBand);

        internal void TranslatedObjectsToBand(BandBase band)
        {
	   // Avoid compilation errors
        }
    }
}
