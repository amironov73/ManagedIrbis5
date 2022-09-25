// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PickListTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class PickListTest
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
            Size = new Size (800, 600)
        };

        var pickList = new PickList
        {
            Location = new Point (10, 10),
            Size = new Size (300, 200)
        };
        pickList.AvailableItems.AddRange (new object[]
        {
            "first",
            "second",
            "third",
            "fourth",
            "fifth",
            "sixth"
        });
        form.Controls.Add (pickList);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
