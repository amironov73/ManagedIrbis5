// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ImageImporterJpeg.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Pdf;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.Internal;

// ReSharper disable once InconsistentNaming
internal class ImageImporterJpeg
    : ImageImporterRoot, IImageImporter
{
    // TODO Find information about JPEG2000.

    // Notes: JFIF is big-endian.

    public ImportedImage? ImportImage
        (
            StreamReaderHelper stream,
            PdfDocument document
        )
    {
        try
        {
            stream.CurrentOffset = 0;

            // Test 2 magic bytes.
            if (TestFileHeader (stream))
            {
                // Skip over 2 magic bytes.
                stream.CurrentOffset += 2;

                var ipd = new ImagePrivateDataDct (stream.Data, stream.Length);
                ImportedImage ii = new ImportedImageJpeg (this, ipd, document);
                if (TestJfifHeader (stream, ii))
                {
                    bool colorHeader = false, infoHeader = false;

                    while (MoveToNextHeader (stream))
                    {
                        if (TestColorFormatHeader (stream, ii))
                        {
                            colorHeader = true;
                        }
                        else if (TestInfoHeader (stream, ii))
                        {
                            infoHeader = true;
                        }
                    }

                    if (colorHeader && infoHeader)
                        return ii;
                }
            }
        }

        // ReSharper disable once EmptyGeneralCatchClause
        catch (Exception)
        {
        }

        return null;
    }

    private bool TestFileHeader (StreamReaderHelper stream)
    {
        // File must start with 0xffd8.
        return stream.GetWord (0, true) == 0xffd8;
    }

    private bool TestJfifHeader (StreamReaderHelper stream, ImportedImage ii)
    {
        // The App0 header should be the first header in every JFIF file.
        if (stream.GetWord (0, true) == 0xffe0)
        {
            // Now check for text "JFIF".
            if (stream.GetDWord (4, true) == 0x4a464946)
            {
                int blockLength = stream.GetWord (2, true);
                if (blockLength >= 16)
                {
                    int version = stream.GetWord (9, true);
                    int units = stream.GetByte (11);
                    int densityX = stream.GetWord (12, true);
                    int densityY = stream.GetWord (14, true);

                    switch (units)
                    {
                        case 0: // Aspect ratio only.
                            ii.Information.HorizontalAspectRatio = densityX;
                            ii.Information.VerticalAspectRatio = densityY;
                            break;
                        case 1: // DPI.
                            ii.Information.HorizontalDPI = densityX;
                            ii.Information.VerticalDPI = densityY;
                            break;
                        case 2: // DPCM.
                            ii.Information.HorizontalDPM = densityX * 100;
                            ii.Information.VerticalDPM = densityY * 100;
                            break;
                    }

                    // More information here? More tests?
                    return true;
                }
            }
        }

        return false;
    }

    private bool TestColorFormatHeader (StreamReaderHelper stream, ImportedImage ii)
    {
        // The SOS header (start of scan).
        if (stream.GetWord (0, true) == 0xffda)
        {
            int components = stream.GetByte (4);
            if (components < 1 || components > 4 || components == 2)
                return false;

            // 1 for grayscale, 3 for RGB, 4 for CMYK.

            int blockLength = stream.GetWord (2, true);

            // Integrity check: correct size?
            if (blockLength != 6 + 2 * components)
                return false;

            // Eventually do more tests here.
            // Magic: we assume that all JPEG files with 4 components are RGBW (inverted CMYK) and not CMYK.
            // We add a test to tell CMYK from RGBW when we encounter a test file in CMYK format.
            ii.Information.ImageFormat = components == 3
                ? ImageInformation.ImageFormats.JPEG
                : (components == 1 ? ImageInformation.ImageFormats.JPEGGRAY : ImageInformation.ImageFormats.JPEGRGBW);

            return true;
        }

        return false;
    }

    private bool TestInfoHeader (StreamReaderHelper stream, ImportedImage ii)
    {
        // The SOF header (start of frame).
        int header = stream.GetWord (0, true);
        if (header is >= 0xffc0 and <= 0xffc3 ||
            header is >= 0xffc9 and <= 0xffcb)
        {
            // Lines in image.
            int sizeY = stream.GetWord (5, true);

            // Samples per line.
            int sizeX = stream.GetWord (7, true);

            // $THHO TODO: Check if we always get useful information here.
            ii.Information.Width = (uint)sizeX;
            ii.Information.Height = (uint)sizeY;

            return true;
        }

        return false;
    }

    private bool MoveToNextHeader (StreamReaderHelper stream)
    {
        int blockLength = stream.GetWord (2, true);

        int headerMagic = stream.GetByte (0);
        int headerType = stream.GetByte (1);

        if (headerMagic == 0xff)
        {
            // EOI: last header.
            if (headerType == 0xd9)
                return false;

            // Check for standalone markers.
            if (headerType == 0x01 || headerType is >= 0xd0 and <= 0xd7)
            {
                stream.CurrentOffset += 2;
                return true;
            }

            // Now assume header with block size.
            stream.CurrentOffset += 2 + blockLength;
            return true;
        }

        return false;
    }

    public ImageData PrepareImage (ImagePrivateData data)
    {
        throw new NotImplementedException();
    }

    //int GetJpgSizeTestCode(byte[] pData, uint FileSizeLow, out int pWidth, out int pHeight)
    //{
    //    pWidth = -1;
    //    pHeight = -1;

    //    int i = 0;


    //    if ((pData[i] == 0xFF) && (pData[i + 1] == 0xD8) && (pData[i + 2] == 0xFF) && (pData[i + 3] == 0xE0))
    //    {
    //        i += 4;

    //        // Check for valid JPEG header (null terminated JFIF)
    //        if ((pData[i + 2] == 'J') && (pData[i + 3] == 'F') && (pData[i + 4] == 'I') && (pData[i + 5] == 'F')
    //            && (pData[i + 6] == 0x00))
    //        {

    //            //Retrieve the block length of the first block since the first block will not contain the size of file
    //            int block_length = pData[i] * 256 + pData[i + 1];

    //            while (i < FileSizeLow)
    //            {
    //                //Increase the file index to get to the next block
    //                i += block_length;

    //                if (i >= FileSizeLow)
    //                {
    //                    //Check to protect against segmentation faults
    //                    return -1;
    //                }

    //                if (pData[i] != 0xFF)
    //                {
    //                    return -2;
    //                }

    //                if (pData[i + 1] == 0xC0)
    //                {
    //                    //0xFFC0 is the "Start of frame" marker which contains the file size
    //                    //The structure of the 0xFFC0 block is quite simple [0xFFC0][ushort length][uchar precision][ushort x][ushort y]
    //                    pHeight = pData[i + 5] * 256 + pData[i + 6];
    //                    pWidth = pData[i + 7] * 256 + pData[i + 8];

    //                    return 0;
    //                }
    //                else
    //                {
    //                    i += 2; //Skip the block marker

    //                    //Go to the next block
    //                    block_length = pData[i] * 256 + pData[i + 1];
    //                }
    //            }

    //            //If this point is reached then no size was found
    //            return -3;
    //        }
    //        else
    //        {
    //            return -4;
    //        } //Not a valid JFIF string
    //    }
    //    else
    //    {
    //        return -5;
    //    } //Not a valid SOI header

    //    //return -6;
    //}  // GetJpgSize
}

/// <summary>
/// Imported JPEG image.
/// </summary>
internal class ImportedImageJpeg : ImportedImage
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public ImportedImageJpeg
        (
            IImageImporter importer,
            ImagePrivateDataDct data,
            PdfDocument document
        )
        : base (importer, data, document)
    {
        // пустое тело конструктора
    }

    internal override ImageData PrepareImageData()
    {
        var data = (ImagePrivateDataDct)Data;
        var imageData = new ImageDataDct
        {
            Data = data.Data,
            Length = data.Length
        };

        return imageData;
    }
}

/// <summary>
/// Contains data needed for PDF. Will be prepared when needed.
/// </summary>
internal class ImageDataDct : ImageData
{
    /// <summary>
    /// Gets the data.
    /// </summary>
    public byte[] Data { get; internal set; }

    /// <summary>
    /// Gets the length.
    /// </summary>
    public int Length { get; internal set; }
}

/*internal*/
/// <summary>
/// Private data for JPEG images.
/// </summary>
internal class ImagePrivateDataDct
    : ImagePrivateData
{
    #region Properties

    /// <summary>
    /// Gets the data.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Gets the length.
    /// </summary>
    public int Length { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ImagePrivateDataDct
        (
            byte[] data,
            int length
        )
    {
        Data = data;
        Length = length;
    }

    #endregion
}
