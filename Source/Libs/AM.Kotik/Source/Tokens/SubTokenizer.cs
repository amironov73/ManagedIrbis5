// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SubTokenizer.cs -- абстрактный вложенный токенайзер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Абстрактный вложенный токенайзер.
/// </summary>
public abstract class SubTokenizer
{
    #region Properties

    /// <summary>
    /// Настройки.
    /// </summary>
    public virtual TokenizerSettings Settings { get; set; } = null!;

    #endregion

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
    /// Проверка, не является ли указанный текст зарезервированным словом.
    /// </summary>
    protected bool IsReservedWord
        (
            string text
        )
    {
        Sure.NotNull (text);

        foreach (var word in Settings.ReservedWords)
        {
            if (string.CompareOrdinal (word, text) == 0)
            {
                return true;
            }
        }

        return false;
    }

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

    #region Public methods

    /// <summary>
    /// Попытка разбора токена в текущей позиции.
    /// </summary>
    public abstract Token? Parse();

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

    /// <summary>
    /// Преобразует строку, содержащую escape-последовательности,
    /// к нормальному виду.
    /// </summary>
    public static string UnescapeText
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var navigator = new TextNavigator (text);
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (text.Length);

        while (!navigator.IsEOF)
        {
            var c = navigator.ReadChar();
            if (c == '\\')
            {
                var c2 = navigator.ReadChar();
                c2 = c2 switch
                {
                    'a' => '\a',
                    'b' => '\b',
                    'f' => '\f',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    'u' => ParseUnicode(),
                    'v' => '\v',
                    '\'' => '\'',
                    '"' => '"',
                    '\\' => '\\',
                    '0' => '\0',
                    _ => '?'
                };
                builder.Append (c2);
            }
            else
            {
                builder.Append (c);
            }
        }

        return builder.ReturnShared();

        char ParseUnicode()
        {
            return (char) int.Parse
                (
                    navigator.ReadString (4).Span,
                    NumberStyles.HexNumber
                );
        }
    }

    #endregion
}
