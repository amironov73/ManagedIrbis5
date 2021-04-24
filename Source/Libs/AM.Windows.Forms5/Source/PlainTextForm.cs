// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* PlainTextForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class PlainTextForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Text editor.
        /// </summary>
        public PlainTextEditor Editor => _textControl;

        /// <inheritdoc cref="Control.Text" />
        public override string? Text
        {
            get => _textControl?.Text;
            set
            {
                if (!ReferenceEquals(_textControl, null))
                {
                    _textControl.Text = value ?? string.Empty;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextForm
            (
                string? text
            )
            : this()
        {
            Text = text;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add button to the toolbox.
        /// </summary>
        public void AddButton
            (
                ToolStripButton button
            )
        {
            Editor.AddButton(button);
        }

        /// <summary>
        /// Show the windows with the text.
        /// </summary>
        public static DialogResult ShowDialog
            (
                IWin32Window? owner,
                string? text
            )
        {
            using var form = new PlainTextForm(text);
            return form.ShowDialog(owner);
        }

        #endregion
    }
}
