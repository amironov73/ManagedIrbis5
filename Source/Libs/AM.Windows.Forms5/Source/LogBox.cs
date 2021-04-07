// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LogBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Text.Output;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class LogBox
        : TextBox
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        public AbstractOutput Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LogBox()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            ReadOnly = true;
            BackColor = SystemColors.Window;
            Multiline = true;
            WordWrap = true;
            ScrollBars = ScrollBars.Vertical;

            Output = new TextBoxOutput(this);

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Set output
        /// </summary>
        public void SetOutput
            (
                AbstractOutput output
            )
        {
            Output = output;
        }

        #endregion
    }
}
