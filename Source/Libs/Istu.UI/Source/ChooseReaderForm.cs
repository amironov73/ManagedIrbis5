// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ChooseReaderForm.cs -- диалог выбора читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Windows.Forms;

using Istu.OldModel;

#endregion

#nullable enable

namespace Istu.UI
{
    /// <summary>
    /// Диалог выбора читателя из нескольких, подходящих по запросу.
    /// </summary>
    public partial class ChooseReaderForm
        : Form
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ChooseReaderForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Показ диалога выбора читателя.
        /// </summary>
        public static Reader? ChooseReader
            (
                IWin32Window owner,
                IEnumerable<Reader> readers
            )
        {
            using var form = new ChooseReaderForm ();
            form.bindingSource1.DataSource = readers;

            var result = form.ShowDialog (owner);
            if (result == DialogResult.OK)
            {
                return (Reader) form.bindingSource1.Current;
            }

            return null;
        }
    }
}
