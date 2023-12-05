// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* TokenCounter.cs -- подсчет LoRA и токенов в подсказках
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using AM.Collections;

#endregion

namespace EasyCaption;

/// <summary>
/// Подсчет LoRA и токенов в подсказке.
/// </summary>
internal sealed partial class TokenCounter
{
    #region Properties

    public string Text { get; set; }

    public DictionaryCounter<string, int> Counter { get; set; }

    #endregion

    #region Construction

    public TokenCounter
        (
            string text,
            DictionaryCounter<string, int> counter
        )
    {
        Text = text;
        Counter = counter;
    }

    #endregion

    #region Private members

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex LoraRegex();

    private string Evaluator
        (
            Match match
        )
    {
        Counter.Increment (match.Value);

        return string.Empty;
    }

    #endregion

    #region Public methods

    public void CountLorasAndTokens()
    {
        const StringSplitOptions options = StringSplitOptions.TrimEntries
            | StringSplitOptions.RemoveEmptyEntries;

        var regex = LoraRegex();
        var text = regex.Replace (Text, Evaluator);
        var tags = text.Split (',', options);
        foreach (var tag in tags)
        {
            Counter.Increment (tag);
        }
    }

    #endregion
}
