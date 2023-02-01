// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* SourceTextSpan.cs -- фрагмент исходного кода скрипта, размеченный для подсветки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using AM.Kotik.Tokenizers;
using AM.Text;

namespace AM.Kotik.Highlighting;

/// <summary>
/// Фрагмент исходного кода скрипта, размеченный для подсветки.
/// </summary>
public sealed class SourceTextSpan<THighlight>
{
    #region Properties

    /// <summary>
    /// Токен, соответствующий фрагменту.
    /// </summary>
    public required Token Token { get; init; }
    
    /// <summary>
    /// Фрагмент исходного кода. Может не совпадать с содержимым токена.
    /// </summary>
    public required string Fragment { get; init; }
    
    /// <summary>
    /// Данные о подсветке.
    /// </summary>
    public required THighlight Highlight { get; init; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Highlight} {Fragment}";

    #endregion

    #region Public methods

    /// <summary>
    /// Преобразование токена во фрагмент текста.
    /// </summary>
    public static string TokenToFragment 
        (
            Token token
        )
    {
        return token.Kind switch
        {
            TokenKind.Char => $"\'{TextUtility.EscapeText (token.Value)}\'",
            TokenKind.String => $"\"{TextUtility.EscapeText (token.Value)}\"",
            TokenKind.External => $"`{token.Value}`",
            _ => token.Value!
        };
    }

    #endregion
}
