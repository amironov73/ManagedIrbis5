// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace IrbisFormsTests
{
    /// <summary>
    /// Main form.
    /// </summary>
    public partial class MainForm
        : Form
    {
        #region Properties

        public IrbisFormsTest? CurrentTest => _listBox.SelectedItem as IrbisFormsTest;

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        private void _exitItem_Click
            (
                object sender,
                EventArgs e
            )
        {
            Close();
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            var tests = IrbisFormsTest
                .LoadFromFile("tests.json")
                .OrderBy(test => test.Title)
                .ToArray();

            // ReSharper disable CoVariantArrayConversion
            _listBox.Items.AddRange(tests);
            // ReSharper restore CoVariantArrayConversion
        }

        private void _listBox_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                var test = CurrentTest;
                test?.RunTest(this);
            }
            catch (Exception ex)
            {
                //ExceptionBox.Show(ex);
                MessageBox.Show(ex.ToString());
            }
        }

        private void _listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                _listBox_DoubleClick(sender, e);
            }
        }

    } // class MainForm

} // namespace FormsTests
