// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable UnusedMember.Global

/* FormsInteractivityProvider.cs -- WinForms-провайдер интерактивности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM.Interactivity;

#endregion

#nullable enable

namespace AM.Windows.Forms.Interactivity;

/// <summary>
/// WinForms-провайдер интерактивности.
/// </summary>
public sealed class FormsInteractivityProvider
    : IInteractivityProvider
{
    #region IInteractivityProvider members

    /// <inheritdoc cref="IInteractivityProvider.ShowMessage"/>
    public void ShowMessage
        (
            string messageText
        )
    {
        Sure.NotNullNorEmpty (messageText);

        MessageBox.Show (messageText);
    }

    /// <inheritdoc cref="IInteractivityProvider.QueryValue"/>
    public string? QueryValue
        (
            string prompt,
            string? defaultValue = null
        )
    {
        Sure.NotNullNorEmpty (prompt);

        var result = defaultValue;
        var dialogResult = InputBox.Query
            (
                "Input",
                prompt,
                ref result
            );

        return dialogResult == DialogResult.OK
            ? result
            : null;
    }

    #endregion
}
