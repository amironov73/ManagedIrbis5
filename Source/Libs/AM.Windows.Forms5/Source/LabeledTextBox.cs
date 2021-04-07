// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LabeledTextBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
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
    public partial class LabeledTextBox
        : UserControl
    {
        #region Properties

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>The label.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Label Label
        {
            [DebuggerStepThrough]
            get
            {
                return _label;
            }
        }

        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <value>The text box.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox
        {
            [DebuggerStepThrough]
            get
            {
                return _textBox;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledTextBox"/> class.
        /// </summary>
        public LabeledTextBox()
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
