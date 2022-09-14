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

using System;

using PdfSharpCore.Pdf.IO;

#endregion

namespace PdfSharpCore.Pdf;

/// <summary>
/// The base class of all PDF objects and simple PDF types.
/// </summary>
public abstract class PdfItem
    : ICloneable
{
    // All simple types (i.e. derived from PdfItem but not from PdfObject) must be immutable.

    /// <inheritdoc cref="ICloneable.Clone"/>
    object ICloneable.Clone()
    {
        return Copy();
    }

    /// <summary>
    /// Creates a copy of this object.
    /// </summary>
    public PdfItem Clone()
    {
        return (PdfItem) Copy();
    }

    /// <summary>
    /// Implements the copy mechanism. Must be overridden in derived classes.
    /// </summary>
    protected virtual object Copy()
    {
        return MemberwiseClone();
    }

    /// <summary>
    /// When overridden in a derived class, appends a raw string representation of this object
    /// to the specified PdfWriter.
    /// </summary>
    internal abstract void WriteObject (PdfWriter writer);
}
