// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssRectWord.cs -- слово внутри inline-блока
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Представляет слово внутри inline-блока.
/// </summary>
internal sealed class CssRectWord
    : CssRect
{
    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="owner">the CSS box owner of the word</param>
    /// <param name="text">the word chars </param>
    /// <param name="hasSpaceBefore">was there a whitespace before the word chars (before trim)</param>
    /// <param name="hasSpaceAfter">was there a whitespace after the word chars (before trim)</param>
    public CssRectWord
        (
            CssBox owner,
            string text,
            bool hasSpaceBefore,
            bool hasSpaceAfter
        )
        : base (owner)
    {
        Text = text;
        HasSpaceBefore = hasSpaceBefore;
        HasSpaceAfter = hasSpaceAfter;
    }

    /// <summary>
    /// was there a whitespace before the word chars (before trim)
    /// </summary>
    public override bool HasSpaceBefore { get; }

    /// <summary>
    /// was there a whitespace after the word chars (before trim)
    /// </summary>
    public override bool HasSpaceAfter { get; }

    /// <summary>
    /// Gets a bool indicating if this word is composed only by spaces.
    /// Spaces include tabs and line breaks
    /// </summary>
    public override bool IsSpaces
    {
        get
        {
            foreach (var c in Text)
            {
                if (!char.IsWhiteSpace (c))
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Gets if the word is composed by only a line break
    /// </summary>
    public override bool IsLineBreak => Text == "\n";

    /// <summary>
    /// Gets the text of the word
    /// </summary>
    public override string Text { get; }

    /// <summary>
    /// Represents this word for debugging purposes
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format
            (
                "{0} ({1} char{2})",
                Text.Replace (' ', '-').Replace ("\n", "\\n"),
                Text.Length,
                Text.Length != 1 ? "s" : string.Empty
            );
    }
}
