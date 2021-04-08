// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimplestMarcEditorTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.WinForms.Editors;

#endregion

#nullable enable

namespace IrbisFormsTests
{
    public sealed class SimplestMarcEditorTest
        : IIrbisFormsTest
    {
        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form
            {
                Size = new Size(800, 600)
            };

            var record = new Record();
            var field = new Field(700)
            {
                {'a', "Иванов"},
                {'b', "И. И."}
            };
            record.Fields.Add(field);

            field = new Field(701)
            {
                {'a', "Петров"},
                {'b', "П. П."}
            };
            record.Fields.Add(field);

            field = new Field(200)
            {
                {'a', "Заглавие"},
                {'e', "подзаголовочное"},
                {'f', "И. И. Иванов, П. П. Петров"}
            };
            record.Fields.Add(field);

            field = new Field(300, "Первое примечание");
            record.Fields.Add(field);
            field = new Field(300, "Второе примечание");
            record.Fields.Add(field);
            field = new Field(300, "Третье примечание");
            record.Fields.Add(field);

            var editor = new SimplestMarcEditor
            {
                Dock = DockStyle.Fill
            };
            form.Controls.Add(editor);
            editor.SetFields(record.Fields);

            form.ShowDialog(ownerWindow);

            editor.GetFields(record.Fields);
            string text = record.ToPlainText();
            PlainTextForm.ShowDialog(form, text);
        }
    }
}
