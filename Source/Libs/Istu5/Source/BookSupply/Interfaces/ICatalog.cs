// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ICatalog.cs -- интерфейс электронного каталога для подсистемы книгообеспеченности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis;

#endregion

#nullable enable

namespace Istu.BookSupply.Interfaces;

/// <summary>
/// Интерфейс электронного каталога для подсистемы книгообеспеченности.
/// </summary>
public interface ICatalog
    : IDisposable
{
    /// <summary>
    /// Форматирование записи.
    /// </summary>
    string? FormatRecord
        (
            int mfn,
            string? format = null
        );

    /// <summary>
    /// Перечисление терминов с указанным префиксом.
    /// </summary>
    string[] ListTerms
        (
            string prefix
        );

    /// <summary>
    /// Чтение записи по ее MFN.
    /// </summary>
    Record? ReadRecord
        (
            int mfn
        );

    /// <summary>
    /// Прямой поиск записей (по поисковому словарю).
    /// </summary>
    int[] SearchRecords
        (
            string expression
        );
}
