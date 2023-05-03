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

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using AM.Json;
using AM.Text;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Генерализованный токенайзер.
/// </summary>
[JsonConverter (typeof (AnyTypeConverter<Tokenizer>))]
public class Tokenizer
{
    #region Properties

    /// <summary>
    /// Настройки токенизации.
    /// </summary>
    public TokenizerSettings Settings { get; protected set; }

    /// <summary>
    /// Токенайзеры для конкретных типов токенов.
    /// </summary>
    public List<Tokenizer> Tokenizers { get; }

    /// <summary>
    /// Пересборщик токенов.
    /// </summary>
    public TokenRefiner? Refiner { get; set; }

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

    /// <summary>
    /// Навигатор по символам.
    /// </summary>
    protected TextNavigator navigator = null!;

    /// <summary>
    /// Достигнут конец текста?
    /// </summary>
    protected bool IsEof => navigator.IsEOF;

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
    /// Считывание текущего символа без передвижения по тексту.
    /// </summary>
    protected char PeekChar() => navigator.PeekChar();

    /// <summary>
    /// Подсматривание на несколько символов вперед.
    /// </summary>
    protected char PeekChar (int delta) => navigator.LookAhead (delta);

    /// <summary>
    /// Считывание текущего символа с переходом к следующему.
    /// </summary>
    protected char ReadChar() => navigator.ReadChar();

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
                new WhitespaceTokenizer(),
                new CommentTokenizer(),
                new CharacterTokenizer(),
                new StringTokenizer(),
                new NumberTokenizer(),
                new IntegerTokenizer(),
                new TermTokenizer(),
                new IdentifierTokenizer()
            }
        };

        return result;
    }

    /// <summary>
    /// Разбор токена в текущей позиции.
    /// Метод должен быть переопределен в потомках.
    /// </summary>
    public virtual TokenizerResult Parse()
    {
        throw new NotImplementedException();
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
        navigator = new TextNavigator (text);

        foreach (var tokenizer in Tokenizers)
        {
            tokenizer.Settings = Settings;
            tokenizer.navigator = navigator;
        }

        AGAIN:
        while (!IsEof)
        {
            var token = TokenizerResult.Error;
            foreach (var tokenizer in Tokenizers)
            {
                token = tokenizer.Parse();
                if (token.IsSkip)
                {
                    goto AGAIN;
                }

                if (token.IsSucceed)
                {
                    break;
                }
            }

            if (!token.IsSucceed)
            {
                // ни один токенайзер не опознал текст,
                // нам подсунули плохой скрипт
                throw new SyntaxException (navigator);
            }

            result.Add (token.Token);
        }

        if (Refiner is not null)
        {
            result = Refiner.RefineTokens (result);
        }

        return result;
    }

    #endregion
}
