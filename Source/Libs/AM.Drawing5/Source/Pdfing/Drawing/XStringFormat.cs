// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XStringFormat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Represents the text layout information.
/// </summary>
public class XStringFormat
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XStringFormat"/> class.
    /// </summary>
    public XStringFormat()
    {
        // пустое тело конструктора
    }

    //TODO public StringFormat(StringFormat format);
    //public StringFormat(StringFormatFlags options);
    //public StringFormat(StringFormatFlags options, int language);
    //public object Clone();
    //public void Dispose();
    //private void Dispose(bool disposing);
    //protected override void Finalize();
    //public float[] GetTabStops(out float firstTabOffset);
    //public void SetDigitSubstitution(int language, StringDigitSubstitute substitute);
    //public void SetMeasurableCharacterRanges(CharacterRange[] ranges);
    //public void SetTabStops(float firstTabOffset, float[] tabStops);
    //public override string ToString();

    /// <summary>
    /// Gets or sets horizontal text alignment information.
    /// </summary>
    public XStringAlignment Alignment { get; set; }

    //public int DigitSubstitutionLanguage { get; }
    //public StringDigitSubstitute DigitSubstitutionMethod { get; }
    //public StringFormatFlags FormatFlags { get; set; }
    //public static StringFormat GenericDefault { get; }
    //public static StringFormat GenericTypographic { get; }
    //public HotkeyPrefix HotkeyPrefix { get; set; }

    /// <summary>
    /// Gets or sets the line alignment.
    /// </summary>
    public XLineAlignment LineAlignment { get; set; }
}
