// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Tokentat.cs -- статистика использования указанного токена
 * Ars Magna project, http://arsmagna.ru
 */

namespace EasyCaption;

/// <summary>
/// Статистика использования указанного токена.
/// </summary>
internal sealed class TokenStat
{
    #region Properties

    /// <summary>
    /// Текст метки.
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    /// Количество использований.
    /// </summary>
    public int Count { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Tag}: {Count}";

    #endregion
}
