// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CommonDialogExtensions.cs -- методы расширения для CommonDialog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="CommonDialog"/>.
/// </summary>
public static class CommonDialogExtensions
{
    #region Public methods

    /// <summary>
    /// Подключение обработчика события
    /// <see cref="CommonDialog.HelpRequest"/>.
    /// </summary>
    /// <returns></returns>
    public static TCommonDialog OnHelpRequest<TCommonDialog>
        (
            this TCommonDialog dialog,
            EventHandler handler
        )
        where TCommonDialog: CommonDialog
    {
        Sure.NotNull (dialog);

        dialog.HelpRequest += handler;

        return dialog;
    }

    /// <summary>
    /// Получение результата отображения диалога.
    /// </summary>
    public static DialogInfo<TCommonDialog> Show<TCommonDialog>
        (
            this TCommonDialog dialog,
            IWin32Window? owner = null
        )
        where TCommonDialog: CommonDialog
    {
        Sure.NotNull (dialog);

        return new DialogInfo<TCommonDialog>
            (
                dialog,
                dialog.ShowDialog (owner)
            );
    }

    #endregion
}
