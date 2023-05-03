// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StandardTokenRefiner.cs -- стандартный пересборщик токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Стандартный пересборщик токенов.
/// </summary>
[PublicAPI]
public sealed class StandardTokenRefiner
    : TokenRefiner
{
    #region TokenRefiner members

    /// <inheritdoc cref="TokenRefiner.RefineTokens"/>
    public override List<Token> RefineTokens
        (
            IList<Token> source
        )
    {
        Sure.NotNull (source);

        var result = new List<Token> (source.Count);
        for (var index = 0; index < source.Count; index++)
        {
            var token = source[index];
            if (token is { Kind: TokenKind.Term, Value: "-" })
            {
                var next = source!.SafeAt (index + 1);
                if (next is not null && next.IsSignedNumber())
                {
                    var prev = source!.SafeAt (index - 1)?.Kind;
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
