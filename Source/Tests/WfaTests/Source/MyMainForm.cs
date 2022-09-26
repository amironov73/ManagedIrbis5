// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MyMainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Windows.Forms.AppServices;

#endregion

namespace WfaTests;

public partial class MyMainForm
    : MainForm
{
    public MyMainForm()
    {
        InitializeComponent();

        Text = "My Main Form";
    }
}
