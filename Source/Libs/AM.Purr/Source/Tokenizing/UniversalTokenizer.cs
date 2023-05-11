// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* UniversalTokenizer.cs -- универсальный токенизатор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Универсальный токенизатор,
/// пригодный для большинства простейших случаев.
/// </summary>
[PublicAPI]
public sealed class UniversalTokenizer
    : Tokenizer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public UniversalTokenizer()
    {
        Initialize();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UniversalTokenizer
        (
            params string[] knownTerms
        )
    {
        Sure.NotNull (knownTerms);

        Initialize();
        Settings.KnownTerms = knownTerms;
    }

    #endregion

    #region Private members

    private void Initialize()
    {
        Refiner = new StandardTokenRefiner();
        Tokenizers.Add (new WhitespaceTokenizer());
        Tokenizers.Add (new StringTokenizer());
        Tokenizers.Add (new NumberTokenizer());
        Tokenizers.Add (new IntegerTokenizer());
        Tokenizers.Add (new TermTokenizer());
        Tokenizers.Add (new IdentifierTokenizer());
    }

    #endregion
}
