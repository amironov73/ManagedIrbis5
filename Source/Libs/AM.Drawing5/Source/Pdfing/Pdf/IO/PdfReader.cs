// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM;

using PdfSharpCore.Exceptions;
using PdfSharpCore.Internal;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.Internal;
using PdfSharpCore.Pdf.IO.enums;

#endregion

namespace PdfSharpCore.Pdf.IO;

/// <summary>
/// Encapsulates the arguments of the PdfPasswordProvider delegate.
/// </summary>
public class PdfPasswordProviderArgs
{
    /// <summary>
    /// Sets the password to open the document with.
    /// </summary>
    public string? Password;

    /// <summary>
    /// When set to true the PdfReader.Open function returns null indicating that no PdfDocument was created.
    /// </summary>
    public bool Abort;
}

/// <summary>
/// A delegated used by the PdfReader.Open function to retrieve a password if the document is protected.
/// </summary>
public delegate void PdfPasswordProvider (PdfPasswordProviderArgs args);

/// <summary>
/// Represents the functionality for reading PDF documents.
/// </summary>
public static class PdfReader
{
    /// <summary>
    /// Determines whether the file specified by its path is a PDF file by inspecting the first eight
    /// bytes of the data. If the file header has the form «%PDF-x.y» the function returns the version
    /// number as integer (e.g. 14 for PDF 1.4). If the file header is invalid or inaccessible
    /// for any reason, 0 is returned. The function never throws an exception.
    /// </summary>
    public static int TestPdfFile
        (
            string path
        )
    {
        FileStream? stream = null;
        try
        {
            var realPath = Drawing.XPdfForm.ExtractPageNumber (path, out _);
            if (File.Exists (realPath)) // prevent unwanted exceptions during debugging
            {
                stream = new FileStream (realPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var bytes = new byte[1024];
                stream.Read (bytes, 0, 1024).NotUsed();
                return GetPdfFileVersion (bytes);
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
        finally
        {
            try
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }

        return 0;
    }

    /// <summary>
    /// Determines whether the specified stream is a PDF file by inspecting the first eight
    /// bytes of the data. If the data begins with «%PDF-x.y» the function returns the version
    /// number as integer (e.g. 14 for PDF 1.4). If the data is invalid or inaccessible
    /// for any reason, 0 is returned. The function never throws an exception.
    /// </summary>
    public static int TestPdfFile
        (
            Stream stream
        )
    {
        long pos = -1;
        try
        {
            pos = stream.Position;
            var bytes = new byte[1024];
            stream.Read (bytes, 0, 1024).NotUsed();
            return GetPdfFileVersion (bytes);
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
        finally
        {
            try
            {
                if (pos != -1)
                {
                    stream.Position = pos;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }

        return 0;
    }

    /// <summary>
    /// Determines whether the specified data is a PDF file by inspecting the first eight
    /// bytes of the data. If the data begins with «%PDF-x.y» the function returns the version
    /// number as integer (e.g. 14 for PDF 1.4). If the data is invalid or inaccessible
    /// for any reason, 0 is returned. The function never throws an exception.
    /// </summary>
    public static int TestPdfFile
        (
            byte[] data
        )
    {
        return GetPdfFileVersion (data);
    }

    /// <summary>
    /// Implements scanning the PDF file version.
    /// </summary>
    ///
    internal static int GetPdfFileVersion
        (
            byte[] bytes
        )
    {
        try
        {
            // Acrobat accepts headers like «%!PS-Adobe-N.n PDF-M.m»...
            var header = PdfEncoders.RawEncoding.GetString
                (bytes, 0, bytes.Length); // Encoding.ASCII.GetString(bytes);
            if (header[0] == '%' || header.IndexOf ("%PDF", StringComparison.Ordinal) >= 0)
            {
                var ich = header.IndexOf ("PDF-", StringComparison.Ordinal);
                if (ich > 0 && header[ich + 5] == '.')
                {
                    var major = header[ich + 4];
                    var minor = header[ich + 6];
                    if (major is >= '1' and < '2' && minor is >= '0' and <= '9')
                    {
                        return (major - '0') * 10 + (minor - '0');
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        // If it doesn't work with the specified encoding ...
        try
        {
            // The file might be incorrectly encoded as ASCII
            var header = System.Text.Encoding.ASCII.GetString (bytes);
            if (header[0] == '%' || header.IndexOf ("%PDF", StringComparison.Ordinal) >= 0)
            {
                var ich = header.IndexOf ("PDF-", StringComparison.Ordinal);
                if (ich > 0 && header[ich + 5] == '.')
                {
                    var major = header[ich + 4];
                    var minor = header[ich + 6];
                    if (major is >= '1' and < '2' && minor is >= '0' and <= '9')
                    {
                        return (major - '0') * 10 + (minor - '0');
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        return 0;
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            PdfDocumentOpenMode openmode
        )
    {
        return Open (path, null, openmode, null, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            PdfDocumentOpenMode openmode,
            PdfReadAccuracy accuracy
        )
    {
        return Open (path, null, openmode, null, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider provider
        )
    {
        return Open (path, null, openmode, provider, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider provider,
            PdfReadAccuracy accuracy
        )
    {
        return Open (path, null, openmode, provider, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            string? password,
            PdfDocumentOpenMode openmode
        )
    {
        return Open (path, password, openmode, null, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            string? password,
            PdfDocumentOpenMode openmode,
            PdfReadAccuracy accuracy
        )
    {
        return Open (path, password, openmode, null, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            string? password,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider? provider
        )
    {
        return Open (path, password, openmode, provider, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            string? password,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider? provider,
            PdfReadAccuracy accuracy
        )
    {
        PdfDocument? document;
        Stream? stream = null;
        try
        {
            stream = new FileStream (path, FileMode.Open, FileAccess.Read);
            document = Open (stream, password, openmode, provider, accuracy);
            if (document != null)
            {
                document._fullPath = Path.GetFullPath (path);
            }
        }
        finally
        {
            if (stream != null)
            {
                stream.Dispose();
            }
        }

        return document;
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path
        )
    {
        return Open (path, null, PdfDocumentOpenMode.Modify, null, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            PdfReadAccuracy accuracy
        )
    {
        return Open (path, null, PdfDocumentOpenMode.Modify, null, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            string? password
        )
    {
        return Open (path, password, PdfDocumentOpenMode.Modify, null, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            string path,
            string? password,
            PdfReadAccuracy accuracy
        )
    {
        return Open (path, password, PdfDocumentOpenMode.Modify, null, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            PdfDocumentOpenMode openmode
        )
    {
        return Open (stream, null, openmode, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            PdfDocumentOpenMode openmode,
            PdfReadAccuracy accuracy
        )
    {
        return Open (stream, null, openmode, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider passwordProvider
        )
    {
        return Open (stream, null, openmode, passwordProvider, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider passwordProvider,
            PdfReadAccuracy accuracy
        )
    {
        return Open (stream, null, openmode, passwordProvider, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            string? password,
            PdfDocumentOpenMode openmode
        )
    {
        return Open (stream, password, openmode, null, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            string? password,
            PdfDocumentOpenMode openmode,
            PdfReadAccuracy accuracy
        )
    {
        return Open (stream, password, openmode, null, accuracy);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            string? password,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider passwordProvider
        )
    {
        return Open (stream, password, openmode, passwordProvider, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            string? password,
            PdfDocumentOpenMode openmode,
            PdfPasswordProvider? passwordProvider,
            PdfReadAccuracy accuracy
        )
    {
        PdfDocument document;
        try
        {
            var lexer = new Lexer (stream);
            document = new PdfDocument (lexer);
            document._state |= DocumentState.Imported;
            document._openMode = openmode;
            document._fileSize = stream.Length;

            // Get file version.
            var header = new byte[1024];
            stream.Position = 0;
            stream.Read (header, 0, 1024).NotUsed();
            document._version = GetPdfFileVersion (header);
            if (document._version == 0)
            {
                throw new InvalidOperationException (PSSR.InvalidPdf);
            }

            document._irefTable.IsUnderConstruction = true;
            var parser = new Parser (document);

            // Read all trailers or cross-reference streams, but no objects.
            document._trailer = parser.ReadTrailer (accuracy);

            if (document._trailer == null)
            {
                ParserDiagnostics.ThrowParserException ("Invalid PDF file: no trailer found.");
            }

            Debug.Assert (document._irefTable.IsUnderConstruction);
            document._irefTable.IsUnderConstruction = false;

            // Is document encrypted?
            var xrefEncrypt = document._trailer!.Elements[PdfTrailer.Keys.Encrypt] as PdfReference;
            if (xrefEncrypt != null)
            {
                //xrefEncrypt.Value = parser.ReadObject(null, xrefEncrypt.ObjectID, false);
                var encrypt = parser.ReadObject (null, xrefEncrypt.ObjectID, false, false);

                encrypt.Reference = xrefEncrypt;
                xrefEncrypt.Value = encrypt;
                var securityHandler = document.SecurityHandler;
                TryAgain:
                var validity = securityHandler!.ValidatePassword (password);
                if (validity == PasswordValidity.Invalid)
                {
                    if (passwordProvider != null)
                    {
                        var args = new PdfPasswordProviderArgs();
                        passwordProvider (args);
                        if (args.Abort)
                        {
                            return null;
                        }

                        password = args.Password;
                        goto TryAgain;
                    }
                    else
                    {
                        if (password == null)
                        {
                            throw new PdfReaderException (PSSR.PasswordRequired);
                        }
                        else
                        {
                            throw new PdfReaderException (PSSR.InvalidPassword);
                        }
                    }
                }
                else if (validity == PasswordValidity.UserPassword && openmode == PdfDocumentOpenMode.Modify)
                {
                    if (passwordProvider != null)
                    {
                        var args = new PdfPasswordProviderArgs();
                        passwordProvider (args);
                        if (args.Abort)
                        {
                            return null;
                        }

                        password = args.Password;
                        goto TryAgain;
                    }
                    else
                    {
                        throw new PdfReaderException (PSSR.OwnerPasswordRequired);
                    }
                }
            }
            else
            {
                if (password != null)
                {
                    // Password specified but document is not encrypted.
                    // ignore
                }
            }

            var irefs2 = document._irefTable.AllReferences;
            var count2 = irefs2.Length;

            // 3rd: Create iRefs for all compressed objects.
            var objectStreams = new Dictionary<int, object?>();
            for (var idx = 0; idx < count2; idx++)
            {
                var iref = irefs2[idx];
                if (iref.Value is PdfCrossReferenceStream xrefStream)
                {
                    for (var idx2 = 0; idx2 < xrefStream.Entries.Count; idx2++)
                    {
                        var item = xrefStream.Entries[idx2];

                        // Is type xref to compressed object?
                        if (item.Type == 2)
                        {
                            //PdfReference irefNew = parser.ReadCompressedObject(new PdfObjectID((int)item.Field2), (int)item.Field3);
                            //document._irefTable.Add(irefNew);
                            var objectNumber = (int)item.Field2;
                            if (!objectStreams.ContainsKey (objectNumber))
                            {
                                objectStreams.Add (objectNumber, null);
                                var objectID = new PdfObjectID ((int)item.Field2);
                                parser.ReadIRefsFromCompressedObject (objectID);
                            }
                        }
                    }
                }
            }

            // 4th: Read compressed objects.
            for (var idx = 0; idx < count2; idx++)
            {
                var iref = irefs2[idx];
                if (iref.Value is PdfCrossReferenceStream xrefStream)
                {
                    for (var idx2 = 0; idx2 < xrefStream.Entries.Count; idx2++)
                    {
                        var item = xrefStream.Entries[idx2];

                        // Is type xref to compressed object?
                        if (item.Type == 2)
                        {
                            var irefNew = parser.ReadCompressedObject
                                (
                                    new PdfObjectID ((int)item.Field2),
                                    (int)item.Field3
                                );
                            irefNew.NotUsed();
                            Debug.Assert (document._irefTable.Contains (iref.ObjectID));

                            //document._irefTable.Add(irefNew);
                        }
                    }
                }
            }


            var irefs = document._irefTable.AllReferences;
            var count = irefs.Length;

            // Read all indirect objects.
            for (var idx = 0; idx < count; idx++)
            {
                var iref = irefs[idx];
                if (iref.Value == null!)
                {
#if DEBUG_
                        if (iref.ObjectNumber == 1074)
                            iref.GetType();
#endif
                    try
                    {
                        Debug.Assert (document._irefTable.Contains (iref.ObjectID));
                        var pdfObject = parser.ReadObject (null, iref.ObjectID, false, false);
                        Debug.Assert (pdfObject.Reference == iref);
                        pdfObject.Reference = iref;
                        Debug.Assert (pdfObject.Reference.Value != null, "Something went wrong.");
                    }
                    catch (PositionNotFoundException ex)
                    {
                        Debug.WriteLine (ex.Message);

                        if (accuracy == PdfReadAccuracy.Strict)
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine (ex.Message);

                        // 4STLA rethrow exception to notify caller.
                        throw;
                    }
                }
                else
                {
                    Debug.Assert (document._irefTable.Contains (iref.ObjectID));

                    //iref.GetType();
                }

                // Set maximum object number.
                document._irefTable._maxObjectNumber = Math.Max (document._irefTable._maxObjectNumber,
                    iref.ObjectNumber);
            }

            // Encrypt all objects.
            if (xrefEncrypt != null)
            {
                document.SecurityHandler!.EncryptDocument();
            }

            // Fix references of trailer values and then objects and irefs are consistent.
            document._trailer.Finish();

#if DEBUG_
    // Some tests...
                PdfReference[] reachables = document.xrefTable.TransitiveClosure(document.trailer);
                reachables.GetType();
                reachables = document.xrefTable.AllXRefs;
                document.xrefTable.CheckConsistence();
#endif

            if (openmode == PdfDocumentOpenMode.Modify)
            {
                // Create new or change existing document IDs.
                if (document.Internals.SecondDocumentID == "")
                {
                    document._trailer.CreateNewDocumentIDs();
                }
                else
                {
                    var agTemp = Guid.NewGuid().ToByteArray();
                    document.Internals.SecondDocumentID = PdfEncoders.RawEncoding.GetString (agTemp, 0, agTemp.Length);
                }

                // Change modification date
                document.Info.ModificationDate = DateTime.Now;

                // Remove all unreachable objects
                var removed = document._irefTable.Compact();
                if (removed != 0)
                {
                    Debug.WriteLine ("Number of deleted unreachable objects: " + removed);
                }

                // Force flattening of page tree
                var pages = document.Pages;
                Debug.Assert (pages != null);

                //bool b = document.irefTable.Contains(new PdfObjectID(1108));
                //b.GetType();

                document._irefTable.CheckConsistence();
                document._irefTable.Renumber();
                document._irefTable.CheckConsistence();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine (ex.Message);
            throw;
        }

        return document;
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream
        )
    {
        return Open (stream, PdfDocumentOpenMode.Modify, PdfReadAccuracy.Strict);
    }

    /// <summary>
    /// Opens an existing PDF document.
    /// </summary>
    public static PdfDocument? Open
        (
            Stream stream,
            PdfReadAccuracy accuracy
        )
    {
        return Open (stream, PdfDocumentOpenMode.Modify, accuracy);
    }
}
