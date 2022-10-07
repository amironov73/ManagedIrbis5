// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FoundFacet.cs -- один найденный фасет в результатах поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis;

/*
    Из официальной документации:

    После числа найденных записей идут строки фасетов
    N#Termin
    Где N число записей с данным термином, причем термин
    возвращается вместе с префиксом! Заметим, что N <= CellCount

 */

/// <summary>
/// Один найденный фасет в результатах поиска.
/// </summary>
public sealed class FoundFacet
{
    #region Properties

    /// <summary>
    /// Число записей с данным термином.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Термин с префиксом.
    /// </summary>
    public string? Term { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор строки ответа сервера.
    /// </summary>
    /// <param name="line">Строка из ответа сервера.</param>
    public void Decode
        (
            ReadOnlySpan<char> line
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
