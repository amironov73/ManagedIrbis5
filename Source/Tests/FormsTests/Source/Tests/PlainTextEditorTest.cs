// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* PlainTextEditorTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class PlainTextEditorTest
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
            Text = "Редактор плоского текста",
            Size = new Size (800, 600)
        };

        var editor = new PlainTextEditor
        {
            Dock = DockStyle.Fill,
            Text = "Значимость этих проблем настолько очевидна, что постоянный "
                   + "количественный рост и сфера нашей активности обеспечивает широкому "
                   + "кругу (специалистов) участие в формировании новых предложений. "
                   + "С другой стороны дальнейшее развитие различных форм деятельности "
                   + "позволяет оценить значение форм развития. Значимость этих проблем "
                   + "настолько очевидна, что рамки и место обучения кадров играет важную "
                   + "роль в формировании позиций, занимаемых участниками в отношении "
                   + "поставленных задач. Идейные соображения высшего порядка, а также "
                   + "укрепление и развитие структуры играет важную роль в формировании форм "
                   + "развития. Повседневная практика показывает, что начало повседневной работы "
                   + "по формированию позиции способствует подготовки и реализации систем "
                   + "массового участия. Разнообразный и богатый опыт укрепление и развитие "
                   + "структуры способствует подготовки и реализации модели развития."
        };
        form.Controls.Add (editor);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
