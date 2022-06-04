// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* Parser`2.cs -- абстрактный парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kitten;

/// <summary>
/// Абстрактный парсер.
/// </summary>
/// <typeparam name="TToken">Тип токенов, чаще всего <see cref="char"/>.
/// </typeparam>
/// <typeparam name="TResult">Тип результата, например, узел
/// абстрактного синтаксического дерева.</typeparam>
public abstract class Parser<TToken, TResult>
{
    #region Public methods

    /// <summary>
    /// Разбор входного потока.
    /// </summary>
    public abstract bool TryParse
        (
            ParseState<TToken> state,
            [MaybeNullWhen (false)] out TResult result
        );

    #endregion
}
