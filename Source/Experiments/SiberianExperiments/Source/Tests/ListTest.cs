// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimplestTest.cs -- простейший тест
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Windows.Forms;

// using ManagedIrbis.WinForms.Grid;

#endregion

#nullable enable

namespace SiberianExperiments
{
    public class ListTest
        : Form
    {
        #region Properties

        // private IList<Dummy> Values { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ListTest()
        {
            // Values = Dummy.GenerateList(100);
            // var grid = new ListGrid<Dummy> (Values)
            // {
            //     Dock = DockStyle.Fill
            // };
            // Controls.Add(grid);
            //
            // grid.AddPropertyColumn(nameof(Dummy.Number))
            //     .SetTitle("Number")
            //     .SetFillWidth(30)
            //     .SetMinWidth(30);
            // grid.AddPropertyColumn(nameof(Dummy.Text))
            //     .SetTitle("Text")
            //     .SetFillWidth(50)
            //     .SetMinWidth(30);
            // grid.AddPropertyColumn(nameof(Dummy.Status))
            //     .SetTitle("State")
            //     .SetFillWidth(20)
            //     .SetMinWidth(30);
            // grid.AutoSizeColumns();
        }

        #endregion
    }
}
