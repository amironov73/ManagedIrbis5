// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* MethodAutocompleteItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// This autocomplete item appears after dot
/// </summary>
public class MethodAutocompleteItem
    : AutocompleteItem
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MethodAutocompleteItem
        (
            string text
        )
        : base (text)
    {
        _lowercaseText = Text.ToLower();
    }

    #endregion

    #region Private members

    private string? _firstPart;
    private readonly string? _lowercaseText;

    #endregion

    #region AutocompleteItem members

    /// <inheritdoc cref="AutocompleteItem.Compare"/>
    public override CompareResult Compare
        (
            string fragmentText
        )
    {
        var i = fragmentText.LastIndexOf ('.');
        if (i < 0)
            return CompareResult.Hidden;
        var lastPart = fragmentText.Substring (i + 1);
        _firstPart = fragmentText.Substring (0, i);

        if (lastPart == "") return CompareResult.Visible;
        if (Text.StartsWith (lastPart, StringComparison.InvariantCultureIgnoreCase))
            return CompareResult.VisibleAndSelected;
        if (_lowercaseText.Contains (lastPart.ToLower()))
            return CompareResult.Visible;

        return CompareResult.Hidden;
    }

    /// <inheritdoc cref="AutocompleteItem.GetTextForReplace"/>
    public override string GetTextForReplace()
    {
        return _firstPart + "." + Text;
    }

    #endregion
}
