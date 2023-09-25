// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* StandardTokenRefiner.cs -- стандартный обработчик списка токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Collections;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Стандартный для Barsik обработчик списка токенов.
/// </summary>
[PublicAPI]
public sealed class StandardTokenRefiner
    : ITokenRefiner
{
    #region Private members

    /// <summary>
    /// Зарезервированные слова Barsik.
    /// </summary>
    public static string[] ReservedWords =
    {
        "abstract", "and", "as", "async", "await", "base", "bool", "break",
        "by", "byte", "case", "catch", "char", "checked", "class", "const",
        "continue", "decimal", "default", "delegate", "descending", "do",
        "double", "else", "enum", "equals", "event", "explicit", "extern",
        "false", "finally", "fixed", "float", "for", "foreach", "from", "func",
        "goto", "group", "if", "implicit", "in", "int", "interface", "internal",
        "is", "join", "lambda", "let", "local", "lock", "long", "namespace",
        "new", "null", "object", "on", "operator", "or", "orderby", "out",
        "override", "params", "private", "protected", "public", "readonly", "ref",
        "return", "sbyte", "sealed", "select", "short", "sizeof", "stackalloc",
        "static", "string", "struct", "switch", "this", "throw", "true", "try",
        "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
        "virtual", "void", "volatile", "where", "while", "with"
    };

    #endregion

    #region ITokenRefiner members

    /// <inheritdoc cref="ITokenRefiner.RefineTokens"/>
    public List<Token> RefineTokens
        (
            List<Token> tokens
        )
    {
        Sure.NotNull (tokens);

        var result = new List<Token> (tokens.Count);
        var comparer = StringComparer.Ordinal;
        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];

            if (token.Kind is TokenKind.Whitespace or TokenKind.Comment)
            {
                // выбрасываем пробелы и комментарии
                continue;
            }

            if (token.Value is { } tokenValue
                && ReservedWords.ContainsValue (tokenValue, comparer))
            {
                tokens[index] = token.WithNewKind (TokenKind.ReservedWord);
                continue;
            }

            if (token is { Kind: TokenKind.Term, Value: "-" })
            {
                var next = tokens!.SafeAt (index + 1);
                if (next is not null && next.IsSignedNumber())
                {
                    var prev = tokens!.SafeAt (index - 1)?.Kind;
                    if (prev is null or TokenKind.Term)
                    {
                        result.Add (next.WithNewValue ("-" + next.Value));
                        index++;
                    }
                    else
                    {
                        result.Add (token);
                    }
                }
                else
                {
                    result.Add (token);
                }
            }
            else
            {
                result.Add (token);
            }
        }

        return result;
    }

    #endregion
}
