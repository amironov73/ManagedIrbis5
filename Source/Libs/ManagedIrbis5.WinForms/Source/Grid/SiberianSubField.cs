// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianSubField.cs --
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

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianSubField
    {
        #region Properties

        /// <summary>
        /// Subfield code.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Original value.
        /// </summary>
        public string? OriginalValue { get; set; }

        /// <summary>
        /// Editing mode?
        /// </summary>
        public string? Mode { get; set; }

        /// <summary>
        /// Modified?
        /// </summary>
        public bool Modified { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Create <see cref="SiberianSubField"/> from
        /// <see cref="WorksheetItem"/>.
        /// </summary>
        public static SiberianSubField FromWorksheetItem
            (
                WorksheetItem item
            )
        {
            var result = new SiberianSubField
            {
                Code = item.Tag[0],
                Title = item.Title
            };

            return result;
        }


        #endregion

    }
}
