// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo

/* SearcherModel.cs -- модель поиска в ЭК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;
using AM.Avalonia;
using AM.Collections;

using Avalonia.Controls;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace EasySearcher;

/// <summary>
/// Модель поиска в ЭК
/// </summary>
public sealed class SearcherModel
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
    public string[]? Found { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Осуществление поиска.
    /// </summary>
    public async Task PerformSearch()
    {
        ErrorMessage = null;
        Found = Array.Empty<string>();

        var query = LookingFor.SafeTrim();
        if (string.IsNullOrEmpty (query))
        {
            return;
        }

        var searcher = new BookSearcher();
        Found = await WaitCursor.RunFuncAsync
            (
                window,
                async (theSearcher, theQuery) => await theSearcher.SearchForBooksAsync (theQuery),
                searcher,
                query
            );

        if (Found.IsNullOrEmpty())
        {
            ErrorMessage = "Ничего не найдено";
        }
    }

    #endregion
}
