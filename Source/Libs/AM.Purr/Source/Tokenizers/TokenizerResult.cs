// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TokenizerResult.cs -- результат работы токенайзера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Results;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Результат работы токенайзера.
/// </summary>
[PublicAPI]
public sealed class TokenizerResult
    : OneOf<Token, Skip, Error>
{
    #region Properties

    /// <summary>
    /// Доступ к токену.
    /// </summary>
    public Token Token => As1();

    /// <summary>
    /// Проверка на успешное выполнение.
    /// </summary>
    public bool IsSucceed => Is1;

   /// <summary>
    /// Проверка: пропуск.
    /// </summary>
    public bool IsSkip => Is2;

    /// <summary>
    /// Проверка: ошибка.
    /// </summary>
    public bool IsError => Is3;

    /// <summary>
    /// Результат: ошибка.
    /// Токенайзер не распознал текст в текущей позиции.
    /// </summary>
    public static readonly TokenizerResult Error = new (new Error());

    /// <summary>
    /// Результат: пропуск. Токенайзер распознал текст,
    /// но его можно пропустить.
    /// </summary>
    public static readonly TokenizerResult Skip = new (new Skip());

    #endregion

    #region Construction

    // конструкторы требуются базовым классом
    private TokenizerResult (Token value) : base (value) { }

    private TokenizerResult (Skip value) : base (value) { }

    private TokenizerResult (Error value) : base (value) { }

    #endregion

    #region Public methods

    /// <summary>
    /// Конструирование успешного результата.
    /// </summary>
    public static TokenizerResult Success (Token token) => new (token);

    #endregion
}
