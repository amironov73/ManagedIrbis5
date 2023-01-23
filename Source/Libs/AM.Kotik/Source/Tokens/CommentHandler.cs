// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CommentHandler.cs -- абстрактный обработчик комментариев
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Абстрактный обработчик комментариев.
/// </summary>
public abstract class CommentHandler
{
    #region Protected members

    /// <summary>
    /// Навигатор по тексту.
    /// </summary>
    protected TextNavigator _navigator = null!;

    /// <summary>
    ///
    /// </summary>
    protected bool IsEof => _navigator.IsEOF;

    /// <summary>
    ///
    /// </summary>
    protected char PeekChar() => _navigator.PeekChar();

    /// <summary>
    ///
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    protected char PeekChar (int delta) => _navigator.LookAhead (delta);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected char ReadChar() => _navigator.ReadChar();

    #endregion

    #region Public members


    /// <summary>
    /// Парсинг комментариев.
    /// </summary>
    public abstract Token? ParseComments();

    /// <summary>
    /// Начало разбора текста.
    /// </summary>
    public void StartParsing
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        _navigator = navigator;
    }

    #endregion
}
