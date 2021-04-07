// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* InputLanguageUtilityTest.cs --
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
    public sealed class InputLanguageUtilityTest
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

            var indicator = new InputLanguageIndicator
                {
                    Location = new Point(10, 10)
                };
            form.Controls.Add(indicator);

            var englishButton = new Button
            {
                Location = new Point(40, 10),
                Text = "English"
            };
            form.Controls.Add(englishButton);

            var russianButton = new Button
            {
                Location = new Point(140, 10),
                Text = "Russian"
            };
            form.Controls.Add(russianButton);

            var textBox = new TextBox
            {
                Location = new Point(10, 100),
                Width = 200
            };
            form.Controls.Add(textBox);

            englishButton.Click += (sender, args) =>
            {
                InputLanguageUtility.SwitchToEnglish();
            };

            russianButton.Click += (sender, args) =>
            {
                InputLanguageUtility.SwitchToRussian();
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
