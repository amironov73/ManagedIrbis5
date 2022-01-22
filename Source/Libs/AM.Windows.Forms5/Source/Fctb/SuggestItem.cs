// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SuggestItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// This Item does not check correspondence to current text fragment.
/// SuggestItem is intended for dynamic menus.
/// </summary>
public class SuggestItem
    : AutocompleteItem
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SuggestItem
        (
            string text,
            int imageIndex
        )
        : base (text, imageIndex)
    {
    }

    #endregion

    #region AutocompleteItem members

    /// <inheritdoc cref="AutocompleteItem.Compare"/>
    public override CompareResult Compare
        (
            string fragmentText
        )
    {
        return CompareResult.Visible;
    }

    #endregion
}
