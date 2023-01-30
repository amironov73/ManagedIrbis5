// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Tokenizer.cs -- генерализованный токенайзер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Генерализованный токенайзер.
/// </summary>
public sealed class Tokenizer
{
    #region Properties

    /// <summary>
    /// Обработчик комментариев.
    /// </summary>
    public CommentHandler? CommentHandler { get; set; }

    /// <summary>
    /// Настройки токенизации.
    /// </summary>
    public TokenizerSettings Settings { get; }

    /// <summary>
    /// Токенайзеры для конкретных типов токенов.
    /// </summary>
    public List<SubTokenizer> Tokenizers { get; }

    /// <summary>
    /// Пересборщик токенов.
    /// </summary>
    public TokenRefiner? Refiner { get; set; }

    /// <summary>
    /// Обработчик пробелов.
    /// </summary>
    public WhitespaceHandler? WhitespaceHandler { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Tokenizer()
    {
        Settings = TokenizerSettings.CreateDefault();
        Tokenizers = new ();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Tokenizer
        (
            TokenizerSettings settings
        )
    {
        Sure.NotNull (settings);

        Settings = settings;
        Tokenizers = new ();
    }

    #endregion

    #region Private members

    private TextNavigator _navigator = null!;

    private bool IsEof => _navigator.IsEOF;

    #endregion

    #region Public methods

    /// <summary>
    /// Получение токенайзера с настройками по умолчанию.
    /// </summary>
    /// <returns></returns>
    public static Tokenizer CreateDefault()
    {
        var result = new Tokenizer (TokenizerSettings.CreateDefault())
        {
            Tokenizers =
            {
                new CharacterTokenizer(),
                new StringTokenizer(),
                new NumberTokenizer(),
                new IntegerTokenizer(), // integer должен быть после number
                new TermTokenizer(),
                new IdentifierTokenizer()
            },
            WhitespaceHandler = new StandardWhitespaceHandler(),
            CommentHandler = new StandardCommentHandler()
        };

        return result;
    }

    /// <summary>
    /// Разбор текста на токены.
    /// </summary>
    public List<Token> Tokenize
        (
            string text
        )
    {
        Sure.NotNull (text);

        var result = new List<Token>();
        _navigator = new TextNavigator (text);

        WhitespaceHandler?.StartParsing (_navigator);
        CommentHandler?.StartParsing (_navigator);
        foreach (var tokenizer in Tokenizers)
        {
            tokenizer.Settings = Settings;
            tokenizer.StartParsing (_navigator);
        }

        while (!IsEof)
        {
            WhitespaceHandler?.SkipWhitespace();
            if (IsEof)
            {
                break;
            }

            var comment = CommentHandler?.ParseComments();
            if (comment is not null)
            {
                result.Add (comment);
                continue;
            }

            if (IsEof)
            {
                break;
            }

            // после комментариев могут быть пробелы
            WhitespaceHandler?.SkipWhitespace();
            if (IsEof)
            {
                break;
            }

            Token? token = null;
            foreach (var tokenizer in Tokenizers)
            {
                token = tokenizer.Parse();
                if (token is not null)
                {
                    break;
                }
            }

            if (token is null)
            {
                throw new SyntaxException (_navigator);
            }

            result.Add (token);
        }

        if (Refiner is not null)
        {
            result = Refiner.RefineTokens (result);
        }

        return result;
    }

    #endregion
}
