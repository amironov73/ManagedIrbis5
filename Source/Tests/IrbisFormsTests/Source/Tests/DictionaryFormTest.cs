// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DictionaryFormTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace IrbisFormsTests
{
    public sealed class DictionaryFormTest
        : IIrbisFormsTest
    {
        #region IUITest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var connection = IrbisFormsTest.GetConnection();

            var adapter = new TermAdapter(connection, "K=");
            adapter.Fill();

            using var form = new DictionaryForm(adapter);
            if (form.ShowDialog(ownerWindow) == DialogResult.OK)
            {
                var chosenTerm = form.ChosenTerm;
                MessageBox.Show("Chosen: " + chosenTerm);
            }
        }

        #endregion
    }
}
