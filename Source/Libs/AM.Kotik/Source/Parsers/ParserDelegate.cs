// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParserDelegate.cs -- описание делегата для парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Описание делегата для парсера.
/// </summary>
public delegate bool ParserDelegate<TResult>
    (
        ParseState state,
        [MaybeNullWhen (false)] out TResult result
    )
    where TResult: class;
