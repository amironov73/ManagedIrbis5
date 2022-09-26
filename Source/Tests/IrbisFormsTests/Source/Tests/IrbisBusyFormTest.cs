// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisBusyFormTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace IrbisFormsTests;

public sealed class IrbisBusyFormTest
    : IIrbisFormsTest
{
    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new BusyForm();
        form.ShowDialog(ownerWindow);
    }
}
