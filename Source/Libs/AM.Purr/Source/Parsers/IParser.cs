// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* IParser.cs -- интерфейс парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Интерфейс парсера.
/// </summary>
[PublicAPI]
public interface IParser<TResult>
{
    /// <summary>
    /// Метка для упрощения отладки.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Разбор входного потока (попытка).
    /// Является правилом хорошего тона, чтобы парсер
    /// восстанавливал состояние <paramref name="state"/>,
    /// если возвращает <c>false</c>.
    /// </summary>
    public bool TryParse
        (
            ParseState state,
            [MaybeNull] out TResult result
        );

}
