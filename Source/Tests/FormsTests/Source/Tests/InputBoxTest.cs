// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* InputBoxTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class InputBoxTest
        : IFormsTest
    {
        #region IFormsTest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            var theValue = "Default value";
            var dialogResult = InputBox.Query
                (
                    "Testing the components",
                    "Enter something",
                    "Please, enter something",
                    ref theValue
                );

            var text = string.Format
                (
                    "Result: {0}{1}Value: {2}",
                    dialogResult,
                    Environment.NewLine,
                    theValue
                );
            MessageBox.Show(text);
        }

        #endregion
    }
}
