// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ActionCode.cs -- код действия в ответ на нажатие горячей клавиши
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Код действия в ответ на нажатие горячей клавиши.
/// </summary>
public enum ActionCode
{
    /// <summary>
    ///
    /// </summary>
    None,

    /// <summary>
    ///
    /// </summary>
    AutocompleteMenu,

    /// <summary>
    ///
    /// </summary>
    AutoIndentChars,

    /// <summary>
    ///
    /// </summary>
    BookmarkLine,

    /// <summary>
    ///
    /// </summary>
    ClearHints,

    /// <summary>
    ///
    /// </summary>
    ClearWordLeft,

    /// <summary>
    ///
    /// </summary>
    ClearWordRight,

    /// <summary>
    ///
    /// </summary>
    CommentSelected,

    /// <summary>
    ///
    /// </summary>
    Copy,

    /// <summary>
    ///
    /// </summary>
    Cut,

    /// <summary>
    ///
    /// </summary>
    DeleteCharRight,

    /// <summary>
    ///
    /// </summary>
    FindChar,

    /// <summary>
    ///
    /// </summary>
    FindDialog,

    /// <summary>
    ///
    /// </summary>
    FindNext,

    /// <summary>
    ///
    /// </summary>
    GoDown,

    /// <summary>
    ///
    /// </summary>
    GoDownWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoDownColumnSelectionMode,

    /// <summary>
    ///
    /// </summary>
    GoEnd,

    /// <summary>
    ///
    /// </summary>
    GoEndWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoFirstLine,

    /// <summary>
    ///
    /// </summary>
    GoFirstLineWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoHome,

    /// <summary>
    ///
    /// </summary>
    GoHomeWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoLastLine,

    /// <summary>
    ///
    /// </summary>
    GoLastLineWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoLeft,

    /// <summary>
    ///
    /// </summary>
    GoLeftWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoLeftColumnSelectionMode,

    /// <summary>
    ///
    /// </summary>
    GoPageDown,

    /// <summary>
    ///
    /// </summary>
    GoPageDownWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoPageUp,

    /// <summary>
    ///
    /// </summary>
    GoPageUpWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoRight,

    /// <summary>
    ///
    /// </summary>
    GoRightWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoRightColumnSelectionMode,

    /// <summary>
    ///
    /// </summary>
    GoToDialog,

    /// <summary>
    ///
    /// </summary>
    GoNextBookmark,

    /// <summary>
    ///
    /// </summary>
    GoPrevBookmark,

    /// <summary>
    ///
    /// </summary>
    GoUp,

    /// <summary>
    ///
    /// </summary>
    GoUpWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoUpColumnSelectionMode,

    /// <summary>
    ///
    /// </summary>
    GoWordLeft,

    /// <summary>
    ///
    /// </summary>
    GoWordLeftWithSelection,

    /// <summary>
    ///
    /// </summary>
    GoWordRight,

    /// <summary>
    ///
    /// </summary>
    GoWordRightWithSelection,

    /// <summary>
    ///
    /// </summary>
    IndentIncrease,

    /// <summary>
    ///
    /// </summary>
    IndentDecrease,

    /// <summary>
    ///
    /// </summary>
    LowerCase,

    /// <summary>
    ///
    /// </summary>
    MacroExecute,

    /// <summary>
    ///
    /// </summary>
    MacroRecord,

    /// <summary>
    ///
    /// </summary>
    MoveSelectedLinesDown,

    /// <summary>
    ///
    /// </summary>
    MoveSelectedLinesUp,

    /// <summary>
    ///
    /// </summary>
    NavigateBackward,

    /// <summary>
    ///
    /// </summary>
    NavigateForward,

    /// <summary>
    ///
    /// </summary>
    Paste,

    /// <summary>
    ///
    /// </summary>
    Redo,

    /// <summary>
    ///
    /// </summary>
    ReplaceDialog,

    /// <summary>
    ///
    /// </summary>
    ReplaceMode,

    /// <summary>
    ///
    /// </summary>
    ScrollDown,

    /// <summary>
    ///
    /// </summary>
    ScrollUp,

    /// <summary>
    ///
    /// </summary>
    SelectAll,

    /// <summary>
    ///
    /// </summary>
    UnbookmarkLine,

    /// <summary>
    ///
    /// </summary>
    Undo,

    /// <summary>
    ///
    /// </summary>
    UpperCase,

    /// <summary>
    ///
    /// </summary>
    ZoomIn,

    /// <summary>
    ///
    /// </summary>
    ZoomNormal,

    /// <summary>
    ///
    /// </summary>
    ZoomOut,

    /// <summary>
    ///
    /// </summary>
    CustomAction1,

    /// <summary>
    ///
    /// </summary>
    CustomAction2,

    /// <summary>
    ///
    /// </summary>
    CustomAction3,

    /// <summary>
    ///
    /// </summary>
    CustomAction4,

    /// <summary>
    ///
    /// </summary>
    CustomAction5,

    /// <summary>
    ///
    /// </summary>
    CustomAction6,

    /// <summary>
    ///
    /// </summary>
    CustomAction7,

    /// <summary>
    ///
    /// </summary>
    CustomAction8,

    /// <summary>
    ///
    /// </summary>
    CustomAction9,

    /// <summary>
    ///
    /// </summary>
    CustomAction10,

    /// <summary>
    ///
    /// </summary>
    CustomAction11,

    /// <summary>
    ///
    /// </summary>
    CustomAction12,

    /// <summary>
    ///
    /// </summary>
    CustomAction13,

    /// <summary>
    ///
    /// </summary>
    CustomAction14,

    /// <summary>
    ///
    /// </summary>
    CustomAction15,

    /// <summary>
    ///
    /// </summary>
    CustomAction16,

    /// <summary>
    ///
    /// </summary>
    CustomAction17,

    /// <summary>
    ///
    /// </summary>
    CustomAction18,

    /// <summary>
    ///
    /// </summary>
    CustomAction19,

    /// <summary>
    ///
    /// </summary>
    CustomAction20
}
