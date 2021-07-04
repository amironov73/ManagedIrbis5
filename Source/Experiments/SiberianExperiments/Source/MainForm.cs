// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MainForm.cs -- главная форма
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

#endregion

#nullable enable

namespace SiberianExperiments
{
    /// <summary>
    /// Главная форма.
    /// </summary>
    public sealed partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            using var form = new SimplestTest();
            form.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using var form = new ListTest();
            form.ShowDialog(this);
        }

    } // class MainForm

} // namespace SiberianExperiments
