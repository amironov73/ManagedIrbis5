// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianTermGrid.cs --
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

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianTermGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for term count.
        /// </summary>
        public SiberianTermCountColumn CountColumn { get; private set; }

        /// <summary>
        /// Column for term text.
        /// </summary>
        public SiberianTermTextColumn TextColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianTermGrid()
        {
            HeaderHeight = 26;

            CountColumn = CreateColumn<SiberianTermCountColumn>();
            CountColumn.Title = "Count";
            CountColumn.MinWidth = 50;
            CountColumn.Width = 50;

            TextColumn = CreateColumn<SiberianTermTextColumn>();
            TextColumn.Title = "Terms";
            TextColumn.FillWidth = 100;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load given terms.
        /// </summary>
        public void Load
            (
                Term[] terms
            )
        {
            Rows.Clear();

            foreach (var term in terms)
            {
                CreateRow(term);
            }

            Goto(1, 0);
        }

        #endregion
    }
}
