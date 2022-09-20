// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable UnusedMember.Global

/* NullInteractivityProvider.cs -- нулевой провайдер интерактивности
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Interactivity;

/// <summary>
/// Нулевой провайдер интерактивности, например, для тестов.
/// </summary>
public sealed class NullInteractivityProvider
    : IInteractivityProvider
{
    #region IInteractivityProvider

    /// <inheritdoc cref="IInteractivityProvider.ShowMessage"/>
    public void ShowMessage
        (
            string messageText
        )
    {
        messageText.NotUsed();

        // пустое тело метода
    }

    /// <inheritdoc cref="IInteractivityProvider.QueryValue"/>
    public string? QueryValue
        (
            string prompt,
            string? defaultValue = null
        )
    {
        prompt.NotUsed();

        return defaultValue;
    }

    #endregion
}
