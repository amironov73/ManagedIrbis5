// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HotkeyMapping.cs -- словарь "горячая клавиша - действие"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using AM;

using KEYS = System.Windows.Forms.Keys;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Словарь "горячая клавиша - действие".
/// </summary>
public class HotkeyMapping
    : SortedDictionary<Keys, ActionCode>
{
    #region Public methods

    /// <summary>
    /// Инициализация по умолчанию.
    /// </summary>
    public virtual void InitDefault()
    {
        this[KEYS.Control | KEYS.G] = ActionCode.GoToDialog;
        this[KEYS.Control | KEYS.F] = ActionCode.FindDialog;
        this[KEYS.Alt | KEYS.F] = ActionCode.FindChar;
        this[KEYS.F3] = ActionCode.FindNext;
        this[KEYS.Control | KEYS.H] = ActionCode.ReplaceDialog;
        this[KEYS.Control | KEYS.C] = ActionCode.Copy;
        this[KEYS.Control | KEYS.Shift | KEYS.C] = ActionCode.CommentSelected;
        this[KEYS.Control | KEYS.X] = ActionCode.Cut;
        this[KEYS.Control | KEYS.V] = ActionCode.Paste;
        this[KEYS.Control | KEYS.A] = ActionCode.SelectAll;
        this[KEYS.Control | KEYS.Z] = ActionCode.Undo;
        this[KEYS.Control | KEYS.R] = ActionCode.Redo;
        this[KEYS.Control | KEYS.U] = ActionCode.UpperCase;
        this[KEYS.Shift | KEYS.Control | KEYS.U] = ActionCode.LowerCase;
        this[KEYS.Control | KEYS.OemMinus] = ActionCode.NavigateBackward;
        this[KEYS.Control | KEYS.Shift | KEYS.OemMinus] = ActionCode.NavigateForward;
        this[KEYS.Control | KEYS.B] = ActionCode.BookmarkLine;
        this[KEYS.Control | KEYS.Shift | KEYS.B] = ActionCode.UnbookmarkLine;
        this[KEYS.Control | KEYS.N] = ActionCode.GoNextBookmark;
        this[KEYS.Control | KEYS.Shift | KEYS.N] = ActionCode.GoPrevBookmark;
        this[KEYS.Alt | KEYS.Back] = ActionCode.Undo;
        this[KEYS.Control | KEYS.Back] = ActionCode.ClearWordLeft;
        this[KEYS.Insert] = ActionCode.ReplaceMode;
        this[KEYS.Control | KEYS.Insert] = ActionCode.Copy;
        this[KEYS.Shift | KEYS.Insert] = ActionCode.Paste;
        this[KEYS.Delete] = ActionCode.DeleteCharRight;
        this[KEYS.Control | KEYS.Delete] = ActionCode.ClearWordRight;
        this[KEYS.Shift | KEYS.Delete] = ActionCode.Cut;
        this[KEYS.Left] = ActionCode.GoLeft;
        this[KEYS.Shift | KEYS.Left] = ActionCode.GoLeftWithSelection;
        this[KEYS.Control | KEYS.Left] = ActionCode.GoWordLeft;
        this[KEYS.Control | KEYS.Shift | KEYS.Left] = ActionCode.GoWordLeftWithSelection;
        this[KEYS.Alt | KEYS.Shift | KEYS.Left] = ActionCode.GoLeftColumnSelectionMode;
        this[KEYS.Right] = ActionCode.GoRight;
        this[KEYS.Shift | KEYS.Right] = ActionCode.GoRightWithSelection;
        this[KEYS.Control | KEYS.Right] = ActionCode.GoWordRight;
        this[KEYS.Control | KEYS.Shift | KEYS.Right] = ActionCode.GoWordRightWithSelection;
        this[KEYS.Alt | KEYS.Shift | KEYS.Right] = ActionCode.GoRightColumnSelectionMode;
        this[KEYS.Up] = ActionCode.GoUp;
        this[KEYS.Shift | KEYS.Up] = ActionCode.GoUpWithSelection;
        this[KEYS.Alt | KEYS.Shift | KEYS.Up] = ActionCode.GoUpColumnSelectionMode;
        this[KEYS.Alt | KEYS.Up] = ActionCode.MoveSelectedLinesUp;
        this[KEYS.Control | KEYS.Up] = ActionCode.ScrollUp;
        this[KEYS.Down] = ActionCode.GoDown;
        this[KEYS.Shift | KEYS.Down] = ActionCode.GoDownWithSelection;
        this[KEYS.Alt | KEYS.Shift | KEYS.Down] = ActionCode.GoDownColumnSelectionMode;
        this[KEYS.Alt | KEYS.Down] = ActionCode.MoveSelectedLinesDown;
        this[KEYS.Control | KEYS.Down] = ActionCode.ScrollDown;
        this[KEYS.PageUp] = ActionCode.GoPageUp;
        this[KEYS.Shift | KEYS.PageUp] = ActionCode.GoPageUpWithSelection;
        this[KEYS.PageDown] = ActionCode.GoPageDown;
        this[KEYS.Shift | KEYS.PageDown] = ActionCode.GoPageDownWithSelection;
        this[KEYS.Home] = ActionCode.GoHome;
        this[KEYS.Shift | KEYS.Home] = ActionCode.GoHomeWithSelection;
        this[KEYS.Control | KEYS.Home] = ActionCode.GoFirstLine;
        this[KEYS.Control | KEYS.Shift | KEYS.Home] = ActionCode.GoFirstLineWithSelection;
        this[KEYS.End] = ActionCode.GoEnd;
        this[KEYS.Shift | KEYS.End] = ActionCode.GoEndWithSelection;
        this[KEYS.Control | KEYS.End] = ActionCode.GoLastLine;
        this[KEYS.Control | KEYS.Shift | KEYS.End] = ActionCode.GoLastLineWithSelection;
        this[KEYS.Escape] = ActionCode.ClearHints;
        this[KEYS.Control | KEYS.M] = ActionCode.MacroRecord;
        this[KEYS.Control | KEYS.E] = ActionCode.MacroExecute;
        this[KEYS.Control | KEYS.Space] = ActionCode.AutocompleteMenu;
        this[KEYS.Tab] = ActionCode.IndentIncrease;
        this[KEYS.Shift | KEYS.Tab] = ActionCode.IndentDecrease;
        this[KEYS.Control | KEYS.Subtract] = ActionCode.ZoomOut;
        this[KEYS.Control | KEYS.Add] = ActionCode.ZoomIn;
        this[KEYS.Control | KEYS.D0] = ActionCode.ZoomNormal;
        this[KEYS.Control | KEYS.I] = ActionCode.AutoIndentChars;
    }

    /// <summary>
    /// Разбор текстового представления.
    /// </summary>
    public static HotkeyMapping Parse
        (
            string? s
        )
    {
        Sure.NotNullNorEmpty (s);

        var result = new HotkeyMapping();
        result.Clear();
        var cult = Thread.CurrentThread.CurrentUICulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        var kc = new KeysConverter();

        foreach (var p in s!.Split (','))
        {
            var pp = p.Split ('=');
            var k = (Keys) kc.ConvertFromString (pp[0].Trim())!;
            var a = (ActionCode)Enum.Parse (typeof (ActionCode), pp[1].Trim());
            result[k] = a;
        }

        Thread.CurrentThread.CurrentUICulture = cult;

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var cult = Thread.CurrentThread.CurrentUICulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        var builder = new StringBuilder();
        var kc = new KeysConverter();
        foreach (var pair in this)
        {
            builder.Append ($"{kc.ConvertToString (pair.Key)}={pair.Value}, ");
        }

        if (builder.Length > 1)
        {
            builder.Remove (builder.Length - 2, 2);
        }

        Thread.CurrentThread.CurrentUICulture = cult;

        return builder.ToString();
    }

    #endregion
}
