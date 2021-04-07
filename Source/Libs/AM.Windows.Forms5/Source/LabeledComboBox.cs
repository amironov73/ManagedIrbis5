// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LabeledComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public partial class LabeledComboBox
        : UserControl
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Gets the combo box.
        /// </summary>
        /// <value>The combo box.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox ComboBox => _comboBox;

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label Label => _label;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledComboBox"/> class.
        /// </summary>
        public LabeledComboBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Control members

        #endregion
    }
}
