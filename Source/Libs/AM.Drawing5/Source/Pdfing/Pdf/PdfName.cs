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
using System.Collections.Generic;
using System.Diagnostics;

using AM;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents a PDF name value.
/// </summary>
[DebuggerDisplay ("({Value})")]
public sealed class PdfName
    : PdfItem
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfName"/> class.
    /// </summary>
    public PdfName()
    {
        Value = "/"; // Empty name.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfName"/> class.
    /// Parameter value always must start with a '/'.
    /// </summary>
    public PdfName (string value)
    {
        Sure.NotNullNorEmpty (value);

        if (value[0] != '/')
        {
            throw new ArgumentException (PSSR.NameMustStartWithSlash);
        }

        Value = value;
    }

    #endregion

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        return Value.Equals (obj);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Gets the name as a string.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Determines whether the specified name and string are equal.
    /// </summary>
    public static bool operator == (PdfName? name, string? str)
    {
        if (ReferenceEquals (name, null))
        {
            return str == null;
        }

        return name.Value == str;
    }

    /// <summary>
    /// Determines whether the specified name and string are not equal.
    /// </summary>
    public static bool operator != (PdfName? name, string? str)
    {
        if (ReferenceEquals (name, null))
        {
            return str != null;
        }

        return name.Value != str;
    }

    /// <summary>
    /// Represents the empty name.
    /// </summary>
    public static readonly PdfName Empty = new PdfName ("/");

    /// <summary>
    /// Writes the name including the leading slash.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        // TODO: what if unicode character are part of the name?
        writer.Write (this);
    }

    /// <summary>
    /// Gets the comparer for this type.
    /// </summary>
    public static PdfXNameComparer Comparer => new PdfXNameComparer();

    /// <summary>
    /// Implements a comparer that compares PdfName objects.
    /// </summary>
    public sealed class PdfXNameComparer
        : IComparer<PdfName>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        public int Compare (PdfName? left, PdfName? right)
        {
            if (left != null)
            {
                if (right != null)
                {
                    return string.Compare (left.Value, right.Value, StringComparison.Ordinal);
                }

                return -1;
            }

            if (right != null)
            {
                return 1;
            }

            return 0;
        }
    }
}
