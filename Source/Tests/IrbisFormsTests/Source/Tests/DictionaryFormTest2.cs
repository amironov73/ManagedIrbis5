// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DictionaryFormTest2.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace IrbisFormsTests
{
    public sealed class DictionaryFormTest2
        : IIrbisFormsTest
    {
        #region IUITest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var connection = IrbisFormsTest.GetConnection();
            {
                var adapter = new TermAdapter(connection, "K=");
                var chosen = DictionaryForm.ChooseTerm
                    (
                        ownerWindow,
                        adapter,
                        "бетон"
                    );
                if (!string.IsNullOrEmpty(chosen))
                {
                    MessageBox.Show("Chosen: " + chosen);
                }
            }
        }

        #endregion
    }
}
