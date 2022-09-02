// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfObjectID.cs -- идентификатор объекта PDF
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Представляет идентификатор объекта PDF, пару объекта и номера поколения.
/// </summary>
[DebuggerDisplay ("{DebuggerDisplay}")]
public struct PdfObjectID
    : IComparable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfObjectID"/> class.
    /// </summary>
    /// <param name="objectNumber">The object number.</param>
    public PdfObjectID (int objectNumber)
    {
        Debug.Assert (objectNumber >= 1, "Object number out of range.");
        ObjectNumber = objectNumber;
        _generationNumber = 0;
#if DEBUG_
            // Just a place for a breakpoint during debugging.
            if (objectNumber == 5894)
                GetType();
#endif
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfObjectID"/> class.
    /// </summary>
    /// <param name="objectNumber">The object number.</param>
    /// <param name="generationNumber">The generation number.</param>
    public PdfObjectID (int objectNumber, int generationNumber)
    {
        Debug.Assert (objectNumber >= 1, "Object number out of range.");

        //Debug.Assert(generationNumber >= 0 && generationNumber <= 65535, "Generation number out of range.");
#if DEBUG_
            // iText creates generation numbers with a value of 65536...
            if (generationNumber > 65535)
                Debug.WriteLine(String.Format("Generation number: {0}", generationNumber));
#endif
        ObjectNumber = objectNumber;
        _generationNumber = (ushort)generationNumber;
    }

    /// <summary>
    /// Gets or sets the object number.
    /// </summary>
    public int ObjectNumber { get; }

    /// <summary>
    /// Gets or sets the generation number.
    /// </summary>
    public int GenerationNumber => _generationNumber;

    readonly ushort _generationNumber;

    /// <summary>
    /// Indicates whether this object is an empty object identifier.
    /// </summary>
    public bool IsEmpty => ObjectNumber == 0;

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    public override bool Equals (object? obj)
    {
        if (obj is PdfObjectID id)
        {
            if (ObjectNumber == id.ObjectNumber)
            {
                return _generationNumber == id._generationNumber;
            }
        }

        return false;
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        return ObjectNumber ^ _generationNumber;
    }

    /// <summary>
    /// Determines whether the two objects are equal.
    /// </summary>
    public static bool operator == (PdfObjectID left, PdfObjectID right)
    {
        return left.Equals (right);
    }

    /// <summary>
    /// Determines whether the tow objects not are equal.
    /// </summary>
    public static bool operator != (PdfObjectID left, PdfObjectID right)
    {
        return !left.Equals (right);
    }

    /// <summary>
    /// Returns the object and generation numbers as a string.
    /// </summary>
    public override string ToString()
    {
        return ObjectNumber.ToString (CultureInfo.InvariantCulture)
               + " "
               + _generationNumber.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates an empty object identifier.
    /// </summary>
    public static PdfObjectID Empty => new PdfObjectID();

    /// <summary>
    /// Compares the current object id with another object.
    /// </summary>
    public int CompareTo (object? obj)
    {
        if (obj is PdfObjectID id)
        {
            if (ObjectNumber == id.ObjectNumber)
            {
                return _generationNumber - id._generationNumber;
            }

            return ObjectNumber - id.ObjectNumber;
        }

        return 1;
    }

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>
    internal string DebuggerDisplay => $"id=({ToString()})";
}
