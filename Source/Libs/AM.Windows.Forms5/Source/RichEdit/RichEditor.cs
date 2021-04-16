// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RichEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    public partial class RichEditor
        : UserControl
    {
        #region Events

        public event EventHandler FileNameChanged;

        #endregion

        #region Properties

        private string _fileName;

        [DefaultValue ( null )]
        public string FileName
        {
            [DebuggerStepThrough]
            get
            {
                return _fileName;
            }
            [DebuggerStepThrough]
            set
            {
                if ( _fileName != value )
                {
                    _fileName = value;
                    if ( FileNameChanged != null )
                    {
                        FileNameChanged ( this, EventArgs.Empty );
                    }
                }
            }
        }

        public ToolStrip ToolStrip
        {
            [DebuggerStepThrough]
            get
            {
                return toolStrip;
            }
        }

        public RichTextBox RichTextBox
        {
            [DebuggerStepThrough]
            get
            {
                return rtfBox;
            }
        }

        public bool Modified
        {
            [DebuggerStepThrough]
            get
            {
                return rtfBox.Modified;
            }
            [DebuggerStepThrough]
            set
            {
                rtfBox.Modified = value;
            }
        }

        #endregion

        #region Construction

        public RichEditor ()
        {
            InitializeComponent ();
        }

        #endregion

        #region Private members

        private void _Refresh ()
        {
            bool canCopy = ( rtfBox.SelectionLength > 0 );
            cutButton.Enabled = canCopy;
            copyButton.Enabled = canCopy;
            colorButton.Text = rtfBox.SelectionColor.Name;
            Font selFont = rtfBox.SelectionFont;
            if ( selFont != null )
            {
                fontButton.Text = selFont.Name;
                boldButton.Checked = selFont.Bold;
                italicButton.Checked = selFont.Italic;
                underlineButton.Checked = selFont.Underline;
                strikeoutButton.Checked = selFont.Strikeout;
                sizeBox.SelectedItem = selFont.Size.ToString ();
            }
            HorizontalAlignment ha = rtfBox.SelectionAlignment;
            leftButton.Checked = ha == HorizontalAlignment.Left;
            centerButton.Checked = ha == HorizontalAlignment.Center;
            rightButton.Checked = ha == HorizontalAlignment.Right;
        }

        private void RichEditor_Load ( object sender, EventArgs e )
        {
            foreach ( FontFamily ff in FontFamily.Families )
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
                && ( saveFileDialog.ShowDialog () == DialogResult.OK ) )
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
                FontStyle newStyle = rtfBox.SelectionFont.Style ^ change;
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
            float newSize = float.Parse ( sizeBox.SelectedItem.ToString () );
            rtfBox.SelectionFont = new Font ( rtfBox.SelectionFont.Name, newSize );
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
            Font selFont = rtfBox.SelectionFont;
            Font newFont = null;
            if ( selFont != null )
            {
                try
                {
                    newFont = new Font
                       (
                           e.ClickedItem.Text,
                           selFont.Size,
                           selFont.Style
                       );
                }
                catch
                {
                    try
                    {
                        newFont = new Font
                           (
                               e.ClickedItem.Text,
                               selFont.Size
                           );
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                float fontSize = 12f;
                float.TryParse ( sizeBox.Text, out fontSize );
                try
                {
                    newFont = new Font
                        (
                            e.ClickedItem.Text,
                            fontSize
                        );
                }
                catch
                {
                }
            }
            if ( newFont != null )
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
            Color newColor = Color.FromName ( e.ClickedItem.Text );
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

        #region Public methods

        #endregion
    }
}
