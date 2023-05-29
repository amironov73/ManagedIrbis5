// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* PromptLogic.cs -- логика сборки элемента запроса из вариантов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace StableErection;

/// <summary>
/// Логика сборки элемента запроса из предлагаемых вариантов.
/// </summary>
[PublicAPI]
public enum PromptLogic
{
    /// <summary>
    /// Можно использовать не больше одного варианта.
    /// </summary>
    Single,

    /// <summary>
    /// Можно использовать произвольное количество вариантов.
    /// </summary>
    Multiple
}
