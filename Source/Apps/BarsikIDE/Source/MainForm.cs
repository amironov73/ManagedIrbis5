// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма программы
 * Ars Magna project, http://arsmagna.ru
 */

#pragma warning disable IDE1006 // Naming Styles

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AM.Scripting;
using AM.Scripting.Barsik;
using AM.Scripting.WinForms;
using AM.Text.Output;

using Fctb;

#endregion

#nullable enable

namespace BarsikIDE
{
    /// <summary>
    /// Главная форма программы.
    /// </summary>
    public partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
            _syntaxTextBox.Language = Language.CSharp;

            _output = new OutputWriter(_logBox.Output);
            _interpreter = new Interpreter
                (
                    TextReader.Null,
                    _output,
                    _output
                );
            _interpreter.WithStdLib();
            _interpreter.WithWinForms();
        }

        #endregion

        #region Private members

        private readonly Interpreter _interpreter;
        private readonly TextWriter _output;

        private void _openButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_openFileDialog.ShowDialog (this) == DialogResult.OK)
            {
                var fileName = _openFileDialog.FileName;
                var sourceCode = File.ReadAllText (fileName);
                _syntaxTextBox.Text = sourceCode;
            }
        }

        private void _runButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            RunScript();
        }

        private void RunScript()
        {
            var sourceCode = _syntaxTextBox.Text.Trim();
            try
            {
                var executionResult = _interpreter.Execute (sourceCode);
                var exitCode = executionResult.ExitCode;
                if (exitCode != 0)
                {
                    _output.WriteLine ($"Exit code: {exitCode}");
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine (ex.ToString());
            }
        }

        private void _PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyCode == Keys.F5)
            {
                RunScript();
                e.IsInputKey = false;
            }
        }
        private void _saveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            var sourceCode = _syntaxTextBox.Text;
            if (_saveFileDialog.ShowDialog (this) == DialogResult.OK)
            {
                File.WriteAllText (_saveFileDialog.FileName, sourceCode);
            }
        }

        #endregion
    }
}
