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

using System.IO;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents an object stream that contains compressed objects.
/// PDF 1.5.
/// </summary>
public class PdfObjectStream
    : PdfDictionary
{
    // Reference: 3.4.6  Object Streams / Page 100

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfObjectStream"/> class.
    /// </summary>
    public PdfObjectStream (PdfDocument document)
        : base (document)
    {
    }

    /// <summary>
    /// Initializes a new instance from an existing dictionary. Used for object type transformation.
    /// </summary>
    internal PdfObjectStream (PdfDictionary dict)
        : base (dict)
    {
        var n = Elements.GetInteger (Keys.N);
        var first = Elements.GetInteger (Keys.First);
        Stream.TryUnfilter();

        var parser = new Parser (null, new MemoryStream (Stream.Value));
        _header = parser.ReadObjectStreamHeader (n, first);
    }

    /// <summary>
    /// Reads the compressed object with the specified index.
    /// </summary>
    internal void ReadReferences (PdfCrossReferenceTable xrefTable)
    {
        ////// Create parser for stream.
        ////Parser parser = new Parser(_document, new MemoryStream(Stream.Value));
        for (var idx = 0; idx < _header.Length; idx++)
        {
            var objectNumber = _header[idx][0];
            var offset = _header[idx][1];

            var objectID = new PdfObjectID (objectNumber);

            // HACK: -1 indicates compressed object.
            var iref = new PdfReference (objectID, -1);
            ////iref.ObjectID = objectID;
            ////iref.Value = xrefStream;
            if (!xrefTable.Contains (iref.ObjectID))
            {
                xrefTable.Add (iref);
            }
            else
            {
                GetType();
            }
        }
    }

    /// <summary>
    /// Reads the compressed object with the specified index.
    /// </summary>
    internal PdfReference ReadCompressedObject (int index)
    {
        var parser = new Parser (_document, new MemoryStream (Stream.Value));
        var objectNumber = _header[index][0];
        var offset = _header[index][1];
        return parser.ReadCompressedObject (objectNumber, offset);
    }

    /// <summary>
    /// N pairs of integers.
    /// The first integer represents the object number of the compressed object.
    /// The second integer represents the absolute offset of that object in the decoded stream,
    /// i.e. the byte offset plus First entry.
    /// </summary>
    private readonly int[][] _header; // Reference: Page 102

    /// <summary>
    /// Predefined keys common to all font dictionaries.
    /// </summary>
    public class Keys : PdfStream.Keys
    {
        // Reference: TABLE 3.14  Additional entries specific to an object stream dictionary / Page 101

        /// <summary>
        /// (Required) The type of PDF object that this dictionary describes;
        /// must be ObjStmfor an object stream.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Required, FixedValue = "ObjStm")]
        public const string Type = "/Type";

        /// <summary>
        /// (Required) The number of compressed objects in the stream.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Required)]
        public const string N = "/N";

        /// <summary>
        /// (Required) The byte offset (in the decoded stream) of the first
        /// compressed object.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Required)]
        public const string First = "/First";

        /// <summary>
        /// (Optional) A reference to an object stream, of which the current object
        /// stream is considered an extension. Both streams are considered part of
        /// a collection of object streams (see below). A given collection consists
        /// of a set of streams whose Extendslinks form a directed acyclic graph.
        /// </summary>
        [KeyInfo (KeyType.Stream | KeyType.Optional)]
        public const string Extends = "/Extends";
    }
}
