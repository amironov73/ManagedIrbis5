// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ParserDelegate.cs -- описание делегата для парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Описание делегата для парсера.
/// </summary>
[PublicAPI]
public delegate bool ParserDelegate<TResult>
    (
        ParseState state,
        out TResult result
    );
