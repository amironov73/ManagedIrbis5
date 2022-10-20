// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BbkModel.cs -- модель для окна поиска в эталоне ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;
using AM.Collections;

using Avalonia.Controls;
using Avalonia.Input;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using RestfulIrbis.RslServices;

#endregion

#nullable enable

namespace FindBbk;

/// <summary>
/// Модель для окна поиска в эталоне ББК от РГБ.
/// </summary>
public sealed class BbkModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Окно.
    /// </summary>
    internal Window window = null!;

    /// <summary>
    /// Искомое понятие.
    /// </summary>
    [Reactive]
    public string? LookingFor { get; set; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    [Reactive]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Найденные записи в эталоне ББК.
    /// </summary>
    [Reactive]
    public BbkEntry[]? Found { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Осуществление поиска.
    /// </summary>
    public async Task PerformSearch()
    {
        ErrorMessage = null;
        Found = Array.Empty<BbkEntry>();

        var query = LookingFor.SafeTrim();
        if (string.IsNullOrEmpty (query))
        {
            return;
        }

        var client = new EthalonBbkClient();
        window.Cursor = new Cursor (StandardCursorType.Wait);
        var html = await client.GetRawHtmlAsync (query);
        Found = client.ParseEntries (html);
        if (Found.IsNullOrEmpty())
        {
            ErrorMessage = "Ничего не найдено";
        }

        window.Cursor = Cursor.Default;
    }

    #endregion
}
