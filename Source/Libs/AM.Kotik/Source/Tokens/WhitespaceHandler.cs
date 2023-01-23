// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* WhitespaceHandler.cs -- обработчик пробельных символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Обработчик пробельных символов.
/// </summary>
public abstract class WhitespaceHandler
{
    #region Protected members

    /// <summary>
    /// Навигатор по тексту.
    /// </summary>
    protected TextNavigator _navigator = null!;

    #endregion

    #region Public methods

    /// <summary>
    /// Пропуск пробельных символов в текущей позиции.
    /// </summary>
    public abstract void SkipWhitespace();

    /// <summary>
    /// Начало разбора текста.
    /// </summary>
    public virtual void StartParsing
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        _navigator = navigator;
    }

    #endregion
}
