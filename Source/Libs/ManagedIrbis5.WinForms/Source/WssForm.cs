// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* WssForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    public partial class WssForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Current term.
        /// </summary>
        public Term? CurrentTerm => (Term?)_bindingSource.Current;

        /// <summary>
        /// Terms.
        /// </summary>
        public List<WorksheetLine> Lines => _lines;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WssForm()
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;

            _lines = new List<WorksheetLine>();
            _bindingSource = new BindingSource
            {
                DataSource = _lines
            };
            _grid.DataSource = _bindingSource;
        }

        #endregion

        #region Private members

        private readonly BindingSource _bindingSource;
        private readonly List<WorksheetLine> _lines;

        #endregion
    }
}
