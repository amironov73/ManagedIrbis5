// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TokenRefiner.cs -- абстрактный пересборщик токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Абстрактный пересборщик токенов.
/// </summary>
public abstract class TokenRefiner
{
    #region Public methods

    /// <summary>
    /// Пересборка списка токенов.
    /// </summary>
    public abstract List<Token> RefineTokens
        (
            IList<Token> source
        );

    #endregion
}
