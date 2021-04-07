// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* BusyControllerTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using AM;
using AM.Threading;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class BusyControllerTest
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

            var firstButton = new Button
            {
                Text = "Push me 1",
                Location = new Point(10, 100)
            };
            form.Controls.Add(firstButton);

            var secondButton = new Button
            {
                Text = "Push me 2",
                Location = new Point(200, 100)
            };
            form.Controls.Add(secondButton);

            var state = new BusyState();

            var stripe = new BusyStripe
            {
                Dock = DockStyle.Top,
                ForeColor = Color.LimeGreen,
                Height = 20,
                Text = "I am very busy"
            };
            stripe.SubscribeTo(state);
            form.Controls.Add(stripe);

            var controller = new BusyController
            {
                State = state
            };
            controller.Controls.Add(firstButton);
            controller.Controls.Add(secondButton);
            controller.ExceptionOccur += (sender, args) =>
            {
                ExceptionBox.Show(ownerWindow, args.Exception);
            };

            Action action = () =>
            {
                Thread.Sleep(3000);
            };

            firstButton.Click += (sender, args) =>
            {
                controller.Run(action);
            };

            secondButton.Click += (sender, args) =>
            {
                controller.RunAsync(action);
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
