// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* WorksheetLine.cs -- single line in the worksheet
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;
using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Single line in the worksheet.
    /// </summary>
    public class WorksheetLine
    {
        #region Properties

        /// <summary>
        /// Schema.
        /// </summary>
        public WorksheetItem? Schema { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string? Value { get; set; }

        #endregion
    }
}
