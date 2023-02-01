// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* AggregatingTokenizer.cs -- агрегирующий токенайзер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Агрегирующий токенайзер.
/// </summary>
public sealed class AggregatingTokenizer
    : SubTokenizer
{
    #region Properties

    /// <inheritdoc cref="SubTokenizer.Settings"/>
    public override TokenizerSettings Settings
    {
        get => _settings;
        set
        {
            _settings = value;
            foreach (var tokenizer in Tokenizers)
            {
                tokenizer.Settings = value;
            }
        }
    }

    /// <summary>
    /// Список вложенных токенайзеров.
    /// </summary>
    public List<SubTokenizer> Tokenizers { get; } = new ();

    #endregion

    #region Private members

    private TokenizerSettings _settings = null!;

    #endregion

    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        foreach (var tokenizer in Tokenizers)
        {
            var result = tokenizer.Parse();
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    /// <inheritdoc cref="SubTokenizer.StartParsing"/>
    public override void StartParsing
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        foreach (var tokenizer in Tokenizers)
        {
            tokenizer.StartParsing (navigator);
        }
    }

    #endregion
}
