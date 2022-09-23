// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BusyStripeTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class BusyStripeTest
    : IFormsTest
{
    #region IFormsTest members

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new Form
        {
            Size = new Size(800, 600)
        };

        var stripe = new BusyStripe
        {
            Size = new Size(10, 30),
            Dock = DockStyle.Top,
            Text = "Some text",
            ForeColor = Color.Aqua
        };
        form.Controls.Add(stripe);

        var button = new Button
        {
            Text = "Toggle on/off",
            Location = new Point(10, 40),
            Width = 100
        };
        button.Click += (sender, args) =>
        {
            stripe.Moving = !stripe.Moving;
        };
        form.Controls.Add(button);

        form.ShowDialog(ownerWindow);
    }

    #endregion

}
