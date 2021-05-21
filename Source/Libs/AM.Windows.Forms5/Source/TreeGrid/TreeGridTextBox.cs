// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridTextBox.cs
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
    public class TreeGridTextBox
        : TreeGridEditor
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TreeGridTextBox()
        {
            _textBox = new TextBox();
        }

        #endregion

        #region Private members

        private readonly TextBox _textBox;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public override Control Control
        {
            get { return _textBox; }
        }

        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public TextBox TextBox
        {
            get { return _textBox; }
        }

        #endregion

        #region TreeGridEditor members

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue
            (
                string? value
            )
        {
            _textBox.Text = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            return _textBox.Text;
        }

        /// <summary>
        /// Selects the text.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public override void SelectText(int start, int length)
        {
            _textBox.SelectionStart = start;
            _textBox.SelectionLength = length;
        }

        #endregion
    }
}
