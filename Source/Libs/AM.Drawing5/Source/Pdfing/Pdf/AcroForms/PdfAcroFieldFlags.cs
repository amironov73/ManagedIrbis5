// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfAcroFieldFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Specifies the flags of AcroForm fields.
/// </summary>
[Flags]
public enum PdfAcroFieldFlags
{
    // ----- Common to all fields -----------------------------------------------------------------

    /// <summary>
    /// If set, the user may not change the value of the field. Any associated widget
    /// annotations will not interact with the user; that is, they will not respond to
    /// mouse clicks or change their appearance in response to mouse motions. This
    /// flag is useful for fields whose values are computed or imported from a database.
    /// </summary>
    ReadOnly = 1 << (1 - 1),

    /// <summary>
    /// If set, the field must have a value at the time it is exported by a submit-form action.
    /// </summary>
    Required = 1 << (2 - 1),

    /// <summary>
    /// If set, the field must not be exported by a submit-form action.
    /// </summary>
    NoExport = 1 << (3 - 1),

    // ----- Specific to button fields ------------------------------------------------------------

    /// <summary>
    /// If set, the field is a pushbutton that does not retain a permanent value.
    /// </summary>
    Pushbutton = 1 << (17 - 1),

    /// <summary>
    /// If set, the field is a set of radio buttons; if clear, the field is a checkbox.
    /// This flag is meaningful only if the Pushbutton flag is clear.
    /// </summary>
    Radio = 1 << (16 - 1),

    /// <summary>
    /// (Radio buttons only) If set, exactly one radio button must be selected at all times;
    /// clicking the currently selected button has no effect. If clear, clicking
    /// the selected button deselects it, leaving no button selected.
    /// </summary>
    NoToggleToOff = 1 << (15 - 1),

    // ----- Specific to text fields --------------------------------------------------------------

    /// <summary>
    /// If set, the field may contain multiple lines of text; if clear, the field�s text
    /// is restricted to a single line.
    /// </summary>
    Multiline = 1 << (13 - 1),

    /// <summary>
    /// If set, the field is intended for entering a secure password that should
    /// not be echoed visibly to the screen. Characters typed from the keyboard
    /// should instead be echoed in some unreadable form, such as
    /// asterisks or bullet characters.
    /// To protect password confidentiality, viewer applications should never
    /// store the value of the text field in the PDF file if this flag is set.
    /// </summary>
    Password = 1 << (14 - 1),

    /// <summary>
    /// (PDF 1.4) If set, the text entered in the field represents the pathname of
    /// a file whose contents are to be submitted as the value of the field.
    /// </summary>
    FileSelect = 1 << (21 - 1),

    /// <summary>
    /// (PDF 1.4) If set, the text entered in the field will not be spell-checked.
    /// </summary>
    DoNotSpellCheckTextField = 1 << (23 - 1),

    /// <summary>
    /// (PDF 1.4) If set, the field will not scroll (horizontally for single-line
    /// fields, vertically for multiple-line fields) to accommodate more text
    /// than will fit within its annotation rectangle. Once the field is full, no
    /// further text will be accepted.
    /// </summary>
    DoNotScroll = 1 << (24 - 1),

    // ----- Specific to choice fields ------------------------------------------------------------

    /// <summary>
    /// If set, the field is a combo box; if clear, the field is a list box.
    /// </summary>
    Combo = 1 << (18 - 1),

    /// <summary>
    /// If set, the combo box includes an editable text box as well as a drop list;
    /// if clear, it includes only a drop list. This flag is meaningful only if the
    /// Combo flag is set.
    /// </summary>
    Edit = 1 << (19 - 1),

    /// <summary>
    /// If set, the field�s option items should be sorted alphabetically. This flag is
    /// intended for use by form authoring tools, not by PDF viewer applications;
    /// viewers should simply display the options in the order in which they occur
    /// in the Opt array.
    /// </summary>
    Sort = 1 << (20 - 1),

    /// <summary>
    /// (PDF 1.4) If set, more than one of the field�s option items may be selected
    /// simultaneously; if clear, no more than one item at a time may be selected.
    /// </summary>
    MultiSelect = 1 << (22 - 1),

    /// <summary>
    /// (PDF 1.4) If set, the text entered in the field will not be spell-checked.
    /// This flag is meaningful only if the Combo and Edit flags are both set.
    /// </summary>
    DoNotSpellCheckChoiseField = 1 << (23 - 1),
}
