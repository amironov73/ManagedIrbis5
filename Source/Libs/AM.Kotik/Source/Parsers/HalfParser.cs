// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* HalfParser.cs -- полуготовый парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Полуготовый парсер, применяется при построении префиксных и постфиксных операций.
/// </summary>
public sealed class HalfParser<TResult>
    where TResult: class
{
    #region Properties

    /// <summary>
    /// Рабочий парсер.
    /// </summary>
    public Parser<TResult> Parser { get; }

    /// <summary>
    /// Функция, умеющая применять результат разбора,
    /// таким образом формируя цепочку префиксов или постфиксов.
    /// </summary>
    public Func<TResult, TResult, TResult> Applier { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HalfParser
        (
            Parser<TResult> parser,
            Func<TResult, TResult, TResult> applier
        )
    {
        Parser = parser;
        Applier = applier;
    }

    #endregion
}
