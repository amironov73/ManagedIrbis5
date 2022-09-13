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

#nullable enable

using AM;

namespace PdfSharpCore.Pdf;

/// <summary>
/// This class is intended for empira internal use only and may change or drop in future releases.
/// </summary>
public class PdfCustomValue
    : PdfDictionary
{
    #region Construction

    /// <summary>
    /// This function is intended for empira internal use only.
    /// </summary>
    public PdfCustomValue()
    {
        CreateStream (new byte[] { });
    }

    /// <summary>
    /// This function is intended for empira internal use only.
    /// </summary>
    public PdfCustomValue (byte[] bytes)
    {
        CreateStream (bytes);
    }

    internal PdfCustomValue (PdfDocument document)
        : base (document)
    {
        CreateStream (new byte[] { });
    }

    internal PdfCustomValue (PdfDictionary dict)
        : base (dict)
    {
        // TODO: uncompress stream
    }

    #endregion

    /// <summary>
    /// This property is intended for empira internal use only.
    /// </summary>
    public PdfCustomValueCompressionMode CompressionMode;

    /// <summary>
    /// This property is intended for empira internal use only.
    /// </summary>
    public byte[] Value
    {
        get => Stream.ThrowIfNull().Value;
        set => Stream.ThrowIfNull().Value = value;
    }
}
