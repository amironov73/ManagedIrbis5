﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IReportRenderer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;




#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>

    public interface IReportRenderer
    {
        /// <summary>
        /// Render the cell.
        /// </summary>
        void RenderCell
            (
                ReportCell cell
            );
    }
}
