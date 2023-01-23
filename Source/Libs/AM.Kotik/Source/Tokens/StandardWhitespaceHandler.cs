// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StandardWhitespaceHandler.cs -- стандартный обработчик пробелов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Стандартный обработчик пробелов.
/// </summary>
public sealed class StandardWhitespaceHandler
    : WhitespaceHandler
{
    #region WhitespaceHandler members

    /// <inheritdoc cref="WhitespaceHandler.SkipWhitespace"/>
    public override void SkipWhitespace()
    {
        _navigator.SkipWhitespace();
    }

    #endregion
}
