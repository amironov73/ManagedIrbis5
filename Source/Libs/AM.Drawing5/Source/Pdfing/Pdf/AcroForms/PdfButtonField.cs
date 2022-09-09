// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfButtonField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

using PdfSharpCore.Pdf.Annotations;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.AcroForms;

/// <summary>
/// Represents the base class for all button fields.
/// </summary>
public abstract class PdfButtonField
    : PdfAcroField
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfButtonField"/> class.
    /// </summary>
    protected PdfButtonField (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfButtonField"/> class.
    /// </summary>
    protected PdfButtonField (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Gets the name which represents the opposite of /Off.
    /// </summary>
    protected string? GetNonOffValue()
    {
        // Try to get the information from the appearance dictionaray.
        // Just return the first key that is not /Off.
        // I'm not sure what is the right solution to get this value.
        if (Elements[PdfAnnotation.Keys.AP] is PdfDictionary ap)
        {
            if (ap.Elements["/N"] is PdfDictionary n)
            {
                foreach (var name in n.Elements.Keys)
                {
                    if (name != "/Off")
                    {
                        return name;
                    }
                }
            }
        }

        return null;
    }

    internal override void GetDescendantNames
        (
            ref List<string> names,
            string partialName
        )
    {
        var t = Elements.GetString (PdfAcroField.Keys.T);

        // HACK: ???
        if (t == "")
        {
            t = "???";
        }

        Debug.Assert (t != "");
        if (t.Length > 0)
        {
            if (!String.IsNullOrEmpty (partialName))
            {
                names.Add (partialName + "." + t);
            }
            else
            {
                names.Add (t);
            }
        }
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// The description comes from PDF 1.4 Reference.
    /// </summary>
    public new class Keys : PdfAcroField.Keys
    {
        // Pushbuttons have no additional entries.
    }
}
