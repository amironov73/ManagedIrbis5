// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* SnippetAutocompleteItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Autocomplete item for code snippets
/// </summary>
/// <remarks>Snippet can contain special char ^ for caret position.</remarks>
public class SnippetAutocompleteItem
    : AutocompleteItem
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="snippet"></param>
    public SnippetAutocompleteItem (string snippet)
    {
        Text = snippet.Replace ("\r", string.Empty);
        ToolTipTitle = "Code snippet:";
        ToolTipText = Text;
    }

    #endregion

    #region Autocomplete members

    /// <inheritdoc cref="AutocompleteItem.GetTextForReplace"/>
    public override string GetTextForReplace()
    {
        return Text;
    }

    /// <inheritdoc cref="AutocompleteItem.OnSelected"/>
    public override void OnSelected
        (
            AutocompleteMenu popupMenu,
            SelectedEventArgs e
        )
    {
        e.Tb.BeginUpdate();
        e.Tb.Selection.BeginUpdate();

        //remember places
        var p1 = popupMenu.Fragment.Start;
        var p2 = e.Tb.Selection.Start;

        //do auto indent
        if (e.Tb.AutoIndent)
        {
            for (var iLine = p1.Line + 1; iLine <= p2.Line; iLine++)
            {
                e.Tb.Selection.Start = new Place (0, iLine);
                e.Tb.DoAutoIndent (iLine);
            }
        }

        e.Tb.Selection.Start = p1;

        //move caret position right and find char ^
        while (e.Tb.Selection.CharBeforeStart != '^')
            if (!e.Tb.Selection.GoRightThroughFolded())
            {
                break;
            }

        //remove char ^
        e.Tb.Selection.GoLeft (true);
        e.Tb.InsertText ("");

        //
        e.Tb.Selection.EndUpdate();
        e.Tb.EndUpdate();
    }

    /// <summary>
    /// Compares fragment text with this item
    /// </summary>
    public override CompareResult Compare
        (
            string fragmentText
        )
    {
        if (Text.StartsWith (fragmentText, StringComparison.InvariantCultureIgnoreCase) &&
            Text != fragmentText)
        {
            return CompareResult.Visible;
        }

        return CompareResult.Hidden;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="AutocompleteItem.ToString"/>
    public override string ToString()
    {
        return MenuText ?? Text.Replace ("\n", " ").Replace ("^", "");
    }

    #endregion

}
