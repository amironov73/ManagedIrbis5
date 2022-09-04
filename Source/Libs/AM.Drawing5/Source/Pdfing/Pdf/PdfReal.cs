// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Globalization;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents a direct real value.
/// </summary>
[DebuggerDisplay ("({Value})")]
public sealed class PdfReal
    : PdfNumber
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfReal"/> class.
    /// </summary>
    public PdfReal()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfReal"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public PdfReal (double value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the value as double.
    /// </summary>
    /// <remarks>This class must behave like a value type.
    /// Therefore it cannot be changed.
    /// </remarks>
    public double Value => _value;

    readonly double _value;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return _value.ToString (Config.SignificantFigures3, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc cref="PdfItem.WriteObject"/>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.Write (this);
    }
}
