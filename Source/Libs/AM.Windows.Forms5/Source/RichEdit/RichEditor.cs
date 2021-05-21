// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RichEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class RichEditor
        : UserControl
    {
        #region Events

        /// <summary>
        ///
        /// </summary>
        public event EventHandler? FileNameChanged;

        #endregion

        #region Properties

        private string? _fileName;

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(null)]
        public string? FileName
        {
            get => _fileName;
            [DebuggerStepThrough]
            set
            {
                if ( _fileName != value )
                {
                    _fileName = value;

                    FileNameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Панель инструментов.
        /// </summary>
        public ToolStrip ToolStrip => toolStrip;

        /// <summary>
        /// Редактор.
        /// </summary>
        public RichTextBox RichTextBox => rtfBox;

        /// <summary>
        ///
        /// </summary>
        public bool Modified
        {
            get => rtfBox.Modified;
            set => rtfBox.Modified = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RichEditor ()
        {
            InitializeComponent ();
        }

        #endregion

        #region Private members

        private void _Refresh ()
        {
            var canCopy = rtfBox.SelectionLength > 0;
            cutButton.Enabled = canCopy;
            copyButton.Enabled = canCopy;
            colorButton.Text = rtfBox.SelectionColor.Name;
            var selFont = rtfBox.SelectionFont;
            if (selFont != null)
            {
                fontButton.Text = selFont.Name;
                boldButton.Checked = selFont.Bold;
                italicButton.Checked = selFont.Italic;
                underlineButton.Checked = selFont.Underline;
                strikeoutButton.Checked = selFont.Strikeout;
                sizeBox.SelectedItem = selFont.Size.ToString(CultureInfo.InvariantCulture);
            }

            var ha = rtfBox.SelectionAlignment;
            leftButton.Checked = ha == HorizontalAlignment.Left;
            centerButton.Checked = ha == HorizontalAlignment.Center;
            rightButton.Checked = ha == HorizontalAlignment.Right;
        }

        private void RichEditor_Load ( object sender, EventArgs e )
        {
            foreach ( var ff in FontFamily.Families )
            {
                fontMenu.Items.Add ( ff.Name );
            }
            _Refresh ();
        }

        private void newButton_Click ( object sender, EventArgs e )
        {
            rtfBox.Clear ();
            FileName = null;
            _Refresh ();
        }

        private void openButton_Click ( object sender, EventArgs e )
        {
            if ( openFileDialog.ShowDialog () == DialogResult.OK )
            {
                FileName = openFileDialog.FileName;
                rtfBox.LoadFile ( FileName );
            }
            _Refresh ();
        }

        private void saveButton_Click ( object sender, EventArgs e )
        {
            if ( string.IsNullOrEmpty ( FileName )
                && saveFileDialog.ShowDialog () == DialogResult.OK )
            {
                FileName = saveFileDialog.FileName;
            }
            if ( !string.IsNullOrEmpty ( FileName ) )
            {
                rtfBox.SaveFile ( FileName );
            }
        }

        private void cutButton_Click ( object sender, EventArgs e )
        {
            rtfBox.Cut ();
        }

        private void copyButton_Click ( object sender, EventArgs e )
        {
            rtfBox.Copy ();
        }

        private void pasteButton_Click ( object sender, EventArgs e )
        {
            rtfBox.Paste ();
        }

        private void _ChangeFontStyle ( FontStyle change )
        {
            if ( rtfBox.SelectionFont != null )
            {
                var newStyle = rtfBox.SelectionFont.Style ^ change;
                rtfBox.SelectionFont = new Font ( rtfBox.SelectionFont, newStyle );
                _Refresh ();
            }
        }

        private void boldButton_Click ( object sender, EventArgs e )
        {
            _ChangeFontStyle ( FontStyle.Bold );
        }

        private void italicButton_Click ( object sender, EventArgs e )
        {
            _ChangeFontStyle ( FontStyle.Italic );
        }

        private void underlineButton_Click ( object sender, EventArgs e )
        {
            _ChangeFontStyle ( FontStyle.Underline );
        }

        private void strikeoutButton_Click ( object sender, EventArgs e )
        {
            _ChangeFontStyle ( FontStyle.Strikeout );
        }

        private void leftButton_Click ( object sender, EventArgs e )
        {
            rtfBox.SelectionAlignment = HorizontalAlignment.Left;
            _Refresh ();
        }

        private void centerButton_Click ( object sender, EventArgs e )
        {
            rtfBox.SelectionAlignment = HorizontalAlignment.Center;
            _Refresh ();
        }

        private void rightButton_Click ( object sender, EventArgs e )
        {
            rtfBox.SelectionAlignment = HorizontalAlignment.Right;
            _Refresh ();
        }

        private void undoButton_Click ( object sender, EventArgs e )
        {
            rtfBox.Undo ();
            _Refresh ();
        }

        private void redoButton_Click ( object sender, EventArgs e )
        {
            rtfBox.Redo ();
            _Refresh ();
        }

        private void sizeBox_SelectedIndexChanged ( object sender, EventArgs e )
        {
            var selectedItem = sizeBox.SelectedItem;
            if (selectedItem is not null)
            {
                var text = selectedItem.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    var newSize = float.Parse(text);
                    rtfBox.SelectionFont = new Font(rtfBox.SelectionFont.Name, newSize);
                }
            }
        }

        private void rtfBox_TextChanged ( object sender, EventArgs e )
        {
            _Refresh ();
        }

        private void fontButton_DropDownItemClicked
            (
                object sender,
                ToolStripItemClickedEventArgs e
            )
        {
            var selectionFont = rtfBox.SelectionFont;
            Font? newFont = null;
            if (selectionFont != null)
            {
                try
                {
                    newFont = new Font
                       (
                           e.ClickedItem.Text,
                           selectionFont.Size,
                           selectionFont.Style
                       );
                }
                catch
                {
                    try
                    {
                        newFont = new Font
                           (
                               e.ClickedItem.Text,
                               selectionFont.Size
                           );
                    }
                    catch (Exception exception)
                    {
                        Magna.TraceException("Change font", exception);
                    }
                }
            }
            else
            {
                var fontSize = sizeBox.Text.ParseSingle();
                if (fontSize != 0f)
                {
                    try
                    {
                        newFont = new Font
                            (
                                e.ClickedItem.Text,
                                fontSize
                            );
                    }
                    catch (Exception exception)
                    {
                        Magna.TraceException("Change font", exception);
                    }
                }
            }

            if (newFont != null)
            {
                rtfBox.SelectionFont = newFont;
            }
            _Refresh ();
        }

        private void colorButton_DropDownItemClicked
            (
                object sender,
                ToolStripItemClickedEventArgs e
            )
        {
            var newColor = Color.FromName ( e.ClickedItem.Text );
            rtfBox.SelectionColor = newColor;
            _Refresh ();
        }

        private void rtfBox_SelectionChanged ( object sender, EventArgs e )
        {
            _Refresh ();
        }

        private void printButton_Click ( object sender, EventArgs e )
        {
            MessageBox.Show("Not implemented");
            // rtfBox.Print ();
        }

        #endregion

        private string _textToFind = string.Empty;

        private void findButton_Click ( object sender, EventArgs e )
        {
            if ( InputBox.Query
                    (
                        "Find text",
                        "Specify what to find",
                        ref _textToFind
                    )
                == DialogResult.OK )
            {
                rtfBox.Find ( _textToFind );
            }
        }

        private void fontButton_Click ( object sender, EventArgs e )
        {
            //fontButton.DropDown.Show ();
            fontDialog.Font = rtfBox.SelectionFont;
            if ( fontDialog.ShowDialog () == DialogResult.OK )
            {
                rtfBox.SelectionFont = fontDialog.Font;
                _Refresh ();
            }
        }

        private void colorButton_Click ( object sender, EventArgs e )
        {
            //colorButton.DropDown.Show ();
            colorDialog.Color = rtfBox.SelectionColor;
            if ( colorDialog.ShowDialog () == DialogResult.OK )
            {
                rtfBox.SelectionColor = colorDialog.Color;
                _Refresh ();
            }
        }

    } // class RichEditor

} // namespace AM.Windows.Forms
