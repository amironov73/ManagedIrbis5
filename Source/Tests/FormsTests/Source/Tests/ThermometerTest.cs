// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ThermometerTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class ThermometerTest
        : IFormsTest
    {
        #region IUITest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form
            {
                Size = new Size(800, 600)
            };

            var thermometer = new Thermometer
            {
                Location = new Point(10, 10),
                Size = new Size(50, 200),
                CurrentTemperature = 36.7
            };
            form.Controls.Add(thermometer);

            var trackBar = new TrackBar
            {
                Location = new Point(100, 10),
                Width = 300,
                Minimum = 0,
                Maximum = 100,
                Value = 37
            };
            form.Controls.Add(trackBar);

            trackBar.ValueChanged += (sender, args) =>
            {
                thermometer.CurrentTemperature = trackBar.Value;
            };


            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
