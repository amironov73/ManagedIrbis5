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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;

using ManagedIrbis.WinForms.Grid;

#endregion

#nullable enable

namespace SiberianExperiments
{
    public sealed class SimplestTest
        : Form
    {
        #region Nested classes

        public class Dummy
        {
            public int Number { get; set; }
            public string? Text { get; set; }
            public bool Status => true;
        }

        #endregion

        #region Properties

        public List<Dummy> Values { get; } = new ();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SimplestTest()
        {
            var grid = new SiberianGrid
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(grid);

            grid.AddPropertyColumn(nameof(Dummy.Number))
                .SetTitle("Number")
                .SetFillWidth(30)
                .SetMinWidth(30);
            grid.AddPropertyColumn(nameof(Dummy.Text))
                .SetTitle("Text")
                .SetFillWidth(50)
                .SetMinWidth(30);
            grid.AddPropertyColumn(nameof(Dummy.Status))
                .SetTitle("State")
                .SetFillWidth(20)
                .SetMinWidth(30);
            grid.AutoSizeColumns();

            for (var i = 0; i < 100; i++)
            {
                grid.CreateRow(new Dummy
                {
                    Number = i + 1,
                    Text = "Text " + (i + 1).ToInvariantString()
                });
            }

        } // constructor

        #endregion
    }
}
