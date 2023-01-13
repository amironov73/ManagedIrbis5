// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Parser.cs -- базовы класс для парсеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Базовый класс для парсеров.
/// </summary>
public abstract class Parser<TResult>
    where TResult: class
{
    #region Public methods

    /// <summary>
    /// Разбор входного потока.
    /// </summary>
    public abstract bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult? result
        );

    #endregion

}
