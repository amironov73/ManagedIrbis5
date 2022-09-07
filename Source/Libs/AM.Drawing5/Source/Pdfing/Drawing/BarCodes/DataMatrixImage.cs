// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DataMatrixImage.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

// ========================================================================================
// ========================================================================================
// ===== THIS CLASS IS A FAKE. THE OPEN SOURCE VERSION OF PDFSHARP DOES NOT IMPLEMENT =====
// ===== A DATAMATRIX CODE. THIS IS BECAUSE OF THE ISO COPYRIGHT.                     =====
// ========================================================================================
// ========================================================================================

// Even if it looks like a datamatrix code it is just random

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Creates the XImage object for a DataMatrix.
/// Important note for OpenSource version of PDFsharp:
///   The generated image object only contains random data.
///   If you need the correct implementation as defined in the ISO/IEC 16022:2000 specification,
///   please contact empira Software GmbH via www.PdfSharp.com.
/// </summary>
internal class DataMatrixImage
{
    public static XImage GenerateMatrixImage (string text, string encoding, int rows, int columns)
    {
        var dataMatrixImage = new DataMatrixImage (text, encoding, rows, columns);
        return dataMatrixImage.DrawMatrix();
    }

    public DataMatrixImage (string text, string encoding, int rows, int columns)
    {
        this.text = text;
        this.encoding = encoding;
        this.rows = rows;
        this.columns = columns;
    }

    string text;
    string encoding;
    int rows;
    int columns;

    /// <summary>
    /// Possible ECC200 Matrixes
    /// </summary>
    static Ecc200Block[] ecc200Sizes =
    {
        new (10, 10, 10, 10, 3, 3, 5),
        new (12, 12, 12, 12, 5, 5, 7),
        new (8, 18, 8, 18, 5, 5, 7),
        new (14, 14, 14, 14, 8, 8, 10),
        new (8, 32, 8, 16, 10, 10, 11),
        new (16, 16, 16, 16, 12, 12, 12),
        new (12, 26, 12, 26, 16, 16, 14),
        new (18, 18, 18, 18, 18, 18, 14),
        new (20, 20, 20, 20, 22, 22, 18),
        new (12, 36, 12, 18, 22, 22, 18),
        new (22, 22, 22, 22, 30, 30, 20),
        new (16, 36, 16, 18, 32, 32, 24),
        new (24, 24, 24, 24, 36, 36, 24),
        new (26, 26, 26, 26, 44, 44, 28),
        new (16, 48, 16, 24, 49, 49, 28),
        new (32, 32, 16, 16, 62, 62, 36),
        new (36, 36, 18, 18, 86, 86, 42),
        new (40, 40, 20, 20, 114, 114, 48),
        new (44, 44, 22, 22, 144, 144, 56),
        new (48, 48, 24, 24, 174, 174, 68),
        new (52, 52, 26, 26, 204, 102, 42),
        new (64, 64, 16, 16, 280, 140, 56),
        new (72, 72, 18, 18, 368, 92, 36),
        new (80, 80, 20, 20, 456, 114, 48),
        new (88, 88, 22, 22, 576, 144, 56),
        new (96, 96, 24, 24, 696, 174, 68),
        new (104, 104, 26, 26, 816, 136, 56),
        new (120, 120, 20, 20, 1050, 175, 68),
        new (132, 132, 22, 22, 1304, 163, 62),
        new (144, 144, 24, 24, 1558, 156, 62), // 156*4+155*2
        new (0, 0, 0, 0, 0, 0, 0) // terminate
    };

    public XImage DrawMatrix()
    {
        return CreateImage (DataMatrix(), this.rows, this.columns);
    }

    /// <summary>
    /// Creates the DataMatrix code.
    /// </summary>
    internal char[] DataMatrix()
    {
        var matrixColumns = this.columns;
        var matrixRows = this.rows;
        var matrix = new Ecc200Block (0, 0, 0, 0, 0, 0, 0);

        foreach (var eccmatrix in ecc200Sizes)
        {
            matrix = eccmatrix;
            if (matrix.Width != columns || matrix.Height != rows)
            {
                continue;
            }
            else
            {
                break;
            }
        }

        var grid = new char[matrixColumns * matrixRows];
        var rand = new Random();

        for (var ccol = 0; ccol < matrixColumns; ccol++)
        {
            grid[ccol] = (char)1;
        }

        for (var rrows = 1; rrows < matrixRows; rrows++)
        {
            grid[rrows * matrixRows] = (char)1;
            for (var ccol = 1; ccol < matrixColumns; ccol++)
            {
                grid[rrows * matrixRows + ccol] = (char)rand.Next (2);
            }
        }

        if (matrixColumns == 0)
        {
            throw new ApplicationException(); //No barcode produced;
        }

        return grid;
    }

    /// <summary>
    /// Encodes the DataMatrix.
    /// </summary>
    internal char[] Iec16022Ecc200
        (
            int columns,
            int rows,
            string encoding,
            int barcodelen,
            string barcode,
            int len,
            int max,
            int ecc
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a DataMatrix image object.
    /// </summary>
    /// <param name="code">A hex string like "AB 08 C3...".</param>
    /// <param name="size">I.e. 26 for a 26x26 matrix</param>
    public XImage CreateImage
        (
            char[] code, int size
        )
    {
        return CreateImage (code, size, size, 10);
    }

    /// <summary>
    /// Creates a DataMatrix image object.
    /// </summary>
    public XImage CreateImage (char[] code, int rows, int columns)
    {
        return CreateImage (code, rows, columns, 10);
    }

    /// <summary>
    /// Creates a DataMatrix image object.
    /// </summary>
    public XImage CreateImage
        (
            char[] code,
            int rows,
            int columns,
            int pixelsize
        )
    {
        throw new NotImplementedException();
    }
}

internal struct Ecc200Block
{
    public int Height;
    public int Width;
    public int CellHeight;
    public int CellWidth;
    public int Bytes;
    public int DataBlock;
    public int RSBlock;

    public Ecc200Block
        (
            int height,
            int width,
            int cellHeight,
            int cellWidth,
            int bytes,
            int dataBlock,
            int rsblock
        )
    {
        Height = height;
        Width = width;
        CellHeight = cellHeight;
        CellWidth = cellWidth;
        Bytes = bytes;
        DataBlock = dataBlock;
        RSBlock = rsblock;
    }
}
