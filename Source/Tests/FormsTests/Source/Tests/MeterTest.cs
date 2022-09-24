// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* MeterTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class MeterTest
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

        var meter = new Meter
        {
            Location = new Point (10, 10),
            Size = new Size (400, 200),
            MinimalValue = 0.0f,
            MaximalValue = 100.0f,
            Value = 50.0f
        };
        form.Controls.Add (meter);

        var trackBar = new TrackBar
        {
            Location = new Point (10, 230),
            Size = new Size (400, 30),
            Minimum = 0,
            Maximum = 100,
            Value = 50
        };
        form.Controls.Add (trackBar);
        trackBar.ValueChanged += (_, _) =>
        {
            meter.Value = trackBar.Value;
        };

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
