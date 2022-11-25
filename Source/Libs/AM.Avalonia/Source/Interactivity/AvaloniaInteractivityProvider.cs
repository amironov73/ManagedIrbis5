// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable UnusedMember.Global

/* AvaloniaInteractivityProvider.cs -- провайдер интерактивности для Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.DTO;
using AM.Interactivity;

#endregion

#nullable enable

namespace AM.Avalonia.Interactivity;

/// <summary>
/// Провайдер интерактивности для Avalonia.
/// </summary>
public sealed class AvaloniaInteractivityProvider
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

        var window = MessageBoxManager.GetMessageBoxStandardWindow
                (
                    "Message",
                    messageText
                );
        window.Show(); // TODO ShowDialog
    }

    /// <inheritdoc cref="IInteractivityProvider.QueryValue"/>
    public string? QueryValue
        (
            string prompt,
            string? defaultValue = null
        )
    {
        Sure.NotNullNorEmpty (prompt);

        var parameters = new MessageBoxInputParams
        {
            ContentTitle = "Input",
            ContentMessage = prompt
        };
        var window = MessageBoxManager.GetMessageBoxInputWindow (parameters);
        window.Show();

        // TODO implement

        return null;
    }

    #endregion
}
