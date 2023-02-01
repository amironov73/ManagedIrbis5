// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ScriptHighlighter.cs -- раскрашиватель текста скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.Collections.Generic;

using AM.Kotik.Tokenizers;

namespace AM.Kotik.Highlighting;

/// <summary>
/// Раскрашиватель текста скрипта.
/// </summary>
public sealed class ScriptHighlighter<THighlight>
{
    #region Properties
    
    /// <summary>
    /// Токенайзер.
    /// </summary>
    public required Tokenizer Tokenizer { get; init; }

    /// <summary>
    /// Цвет по умолчанию.
    /// </summary>
    public required THighlight MainColor { get; init; }

    /// <summary>
    /// Карта цветов.
    /// </summary>
    public required Dictionary<string, THighlight> Colors { get; init; }

    #endregion

    #region Public methods
    
    /// <summary>
    /// Разбор исходного кода скрипта с разбиением
    /// на подсвеченные фрагменты.
    /// </summary>
    /// <returns></returns>
    public List<SourceTextSpan<THighlight>> Highlight
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        var tokens = Tokenizer.Tokenize (sourceCode);
        var result = new List<SourceTextSpan<THighlight>>();
        foreach (var token in tokens)
        {
            if (!Colors.TryGetValue (token.Kind, out var color))
            {
                color = MainColor;
            }

            var span = new SourceTextSpan<THighlight>
            {
                Token = token,
                Fragment = SourceTextSpan<THighlight>.TokenToFragment (token),
                Highlight = color
            };
            result.Add (span);
        }

        return result;
    }

    #endregion
}
