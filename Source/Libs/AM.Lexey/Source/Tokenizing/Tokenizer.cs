// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Tokenizer.cs -- разбивает исходный код на токены
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Разбивает исходный код на токены.
/// </summary>
[PublicAPI]
public sealed class Tokenizer
{
    #region Properties

    /// <summary>
    /// Рефайнер (опционально).
    /// </summary>
    public ITokenRefiner? Refiner { get; set; }

    /// <summary>
    /// Распознаватели для отдельных видов токенов.
    /// Порядок распознавателей может быть важен!
    /// </summary>
    public IList<ITokenRecognizer> Recognizers { get; } = new List<ITokenRecognizer>();

    #endregion

    #region Public methods

    public List<Token> Parse
        (
            string text
        )
    {
        var result = new List<Token>();

        if (string.IsNullOrEmpty (text))
        {
            return result;
        }

        var navigator = new TextNavigator (text);
        while (!navigator.IsEOF)
        {
            Token? token = default;
            foreach (var recognizer in Recognizers)
            {
                token = recognizer.RecognizeToken (navigator);
                if (token is not null)
                {
                    break;
                }
            }

            if (token is null)
            {
                throw new SyntaxException (navigator);
            }
        }

        if (Refiner is not null)
        {
            result = Refiner.RefineTokens (result);
        }

        return result;
    }

    #endregion
}
