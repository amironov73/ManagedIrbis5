// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable UnusedMember.Global

/* ConsoleInteractivityProvider.cs -- консольный провайдер интерактивности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Interactivity;

/// <summary>
/// Консольный провайдер интерактивности.
/// </summary>
public sealed class ConsoleInteractivityProvider
    : IInteractivityProvider
{
    #region IInteractivityProvider

    /// <inheritdoc cref="IInteractivityProvider.ShowMessage"/>
    public void ShowMessage
        (
            string messageText
        )
    {
        Sure.NotNull (messageText);

        Console.WriteLine (messageText);
        Console.WriteLine ("Press ENTER to continue");
        Console.ReadLine();
    }

    /// <inheritdoc cref="IInteractivityProvider.QueryValue"/>
    public string? QueryValue
        (
            string prompt,
            string? defaultValue = null
        )
    {
        Sure.NotNullNorEmpty (prompt);

        Console.Write (prompt);
        Console.Write (' ');
        var result = Console.ReadLine();
        if (string.IsNullOrEmpty (result))
        {
            result = defaultValue;
        }

        return result;
    }

    #endregion
}
