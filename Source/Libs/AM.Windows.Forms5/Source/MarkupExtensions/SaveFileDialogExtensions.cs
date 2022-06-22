// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SaveFileDialogExtensions.cs -- методы расширения для SaveFileDialog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="SaveFileDialog"/>
/// </summary>
public static class SaveFileDialogExtensions
{
    #region Public methods

    /// <summary>
    /// Предлагать создать файл?
    /// </summary>
    public static SaveFileDialog CreatePrompt
        (
            this SaveFileDialog dialog,
            bool createPrompt = true
        )
    {
        Sure.NotNull (dialog);

        dialog.CreatePrompt = createPrompt;

        return dialog;
    }

    /// <summary>
    /// Спрашивать, если требуется перезаписать уже имеющийся файл?
    /// </summary>
    public static SaveFileDialog OverwritePrompt
        (
            this SaveFileDialog dialog,
            bool overwritePrompt = true
        )
    {
        Sure.NotNull (dialog);

        dialog.OverwritePrompt = overwritePrompt;

        return dialog;
    }

    #endregion
}
