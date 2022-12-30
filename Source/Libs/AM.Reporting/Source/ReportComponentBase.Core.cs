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
    partial class ReportComponentBase
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="e">Draw event arguments.</param>
        public void DrawMarkers (FRPaintEventArgs e)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="source"></param>
        public virtual void AssignPreviewEvents (Base source)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected bool DrawIntersectBackground (FRPaintEventArgs e)
        {
            return false;
        }
    }
}
