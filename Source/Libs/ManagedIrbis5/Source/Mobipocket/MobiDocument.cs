// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* MobiDocument.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using ManagedIrbis.Mobipocket.Headers;

#endregion

#nullable enable

namespace ManagedIrbis.Mobipocket;

/// <summary>
///
/// </summary>
public sealed class MobiDocument
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    public MobiDocument (string filePath)
    {
        FilePath = filePath;
    }

    /// <summary>
    ///
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    ///
    /// </summary>
    public string Title => GetTitle();

    /// <summary>
    ///
    /// </summary>
    public string Author => GetAuthor();

    /// <summary>
    ///
    /// </summary>
    public DateTime? PublishingDate => GetPublishingDate();

    /// <summary>
    ///
    /// </summary>
    public string Asin => GetAsin();

    private string GetAsin()
    {
        if (MobiHeader == null)
        {
            return "";
        }

        var firstAsin = MobiHeader.GetExthRecordValue (113);
        var secondAsin = MobiHeader.GetExthRecordValue (504);

        return firstAsin ?? secondAsin;
    }

    private string GetAuthor()
    {
        if (MobiHeader == null)
        {
            return "";
        }

        return MobiHeader.GetExthRecordValue (100);
    }

    private DateTime? GetPublishingDate()
    {
        if (MobiHeader == null)
        {
            return null;
        }

        var date = MobiHeader.GetExthRecordValue (106);

        return Convert.ToDateTime (date);
    }

    private string GetTitle()
    {
        if (MobiHeader == null)
        {
            return PdbHeader.Name;
        }

        return MobiHeader.FullName;
    }


    public PdbHeader PdbHeader { get; set; }
    public MobiHeader MobiHeader { get; set; }

    public void Write (FileStream outStream, string saveFilePath)
    {
        if (PdbHeader == null || MobiHeader == null)
        {
            throw new ApplicationException (
                "MobiDocument is not valid.  Verify that the PdbHeader and MobiHeaders are valid.");
        }

        if (!outStream.CanWrite)
        {
            throw new ArgumentException ("outStream is not writable", "outStream");
        }

        PdbHeader.Write (outStream);
        MobiHeader.Write (outStream);

        var readOffset = PdbHeader.OffsetAfterMobiHeader;
        var bytesRead = 0;
        var buffer = new byte[4096];
        var stream = File.OpenRead (FilePath);
        stream.Seek (readOffset, SeekOrigin.Begin);

        while ((bytesRead = stream.Read (buffer, 0, buffer.Length)) > 0)
        {
            outStream.Write (buffer, 0, bytesRead);
        }
    }
}
