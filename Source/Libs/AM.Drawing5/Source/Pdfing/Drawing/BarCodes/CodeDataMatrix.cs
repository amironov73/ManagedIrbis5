// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* CodeDataMatrix.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Defines the DataMatrix 2D barcode. THIS IS AN EMPIRA INTERNAL IMPLEMENTATION. THE CODE IN
/// THE OPEN SOURCE VERSION IS A FAKE.
/// </summary>
public sealed class CodeDataMatrix
    : MatrixCode
{
    #region Properties

    /// <summary>
    /// Ширина отступа.
    /// </summary>
    public int QuietZone { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix()
        : this
            (
                code: string.Empty,
                encoding:string.Empty,
                rows: 26,
                columns: 26,
                quietZone: 0,
                size: XSize.Empty
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            int length
        )
        : this
            (
                code,
                encoding: string.Empty,
                rows: length,
                columns: length,
                quietZone: 0,
                size: XSize.Empty
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            int length,
            XSize size
        )
        : this
            (
                code,
                encoding: string.Empty,
                rows: length,
                columns: length,
                quietZone: 0,
                size
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            DataMatrixEncoding dmEncoding,
            int length,
            XSize size
        )
        : this
            (
                code,
                encoding: CreateEncoding (dmEncoding, code.Length),
                rows: length,
                columns: length,
                quietZone: 0,
                size
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            int rows,
            int columns
        )
        : this
            (
                code,
                encoding: string.Empty,
                rows,
                columns,
                quietZone: 0,
                size: XSize.Empty
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            int rows,
            int columns,
            XSize size
        )
        : this
            (
                code,
                encoding: string.Empty,
                rows,
                columns,
                quietZone: 0,
                size
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            DataMatrixEncoding dmEncoding,
            int rows,
            int columns,
            XSize size
        )
        : this
            (
                code,
                encoding: CreateEncoding (dmEncoding, code.Length),
                rows,
                columns,
                quietZone: 0,
                size
            )
    {
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            int rows,
            int columns,
            int quietZone
        )
        : this
            (
                code,
                encoding: string.Empty,
                rows,
                columns,
                quietZone,
                size: XSize.Empty
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of CodeDataMatrix.
    /// </summary>
    public CodeDataMatrix
        (
            string code,
            string encoding,
            int rows,
            int columns,
            int quietZone,
            XSize size
        )
        : base (code, encoding, rows, columns, size)
    {
        QuietZone = quietZone;
    }

    #endregion

    #region Private members

    private static string CreateEncoding
        (
            DataMatrixEncoding dmEncoding,
            int length
        )
    {
        var tempencoding = dmEncoding switch
        {
            DataMatrixEncoding.Ascii => new string ('a', length),
            DataMatrixEncoding.C40 => new string ('c', length),
            DataMatrixEncoding.Text => new string ('t', length),
            DataMatrixEncoding.X12 => new string ('x', length),
            DataMatrixEncoding.EDIFACT => new string ('e', length),
            DataMatrixEncoding.Base256 => new string ('b', length),
            _ => throw new ApplicationException()
        };

        return tempencoding;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Sets the encoding of the DataMatrix.
    /// </summary>
    public void SetEncoding
        (
            DataMatrixEncoding encoding
        )
    {
        Sure.Defined (encoding);

        Encoding = CreateEncoding (encoding, Text.Length);
    }

    #endregion

    #region MatrixCode members

    /// <inheritdoc cref="MatrixCode.Render"/>
    protected internal override void Render
        (
            XGraphics graphics,
            XBrush brush,
            XPoint position
        )
    {
        Sure.NotNull (graphics);

        var state = graphics.Save();

        switch (Direction)
        {
            case CodeDirection.RightToLeft:
                graphics.RotateAtTransform (180, position);
                break;

            case CodeDirection.TopToBottom:
                graphics.RotateAtTransform (90, position);
                break;

            case CodeDirection.BottomToTop:
                graphics.RotateAtTransform (-90, position);
                break;
        }

        var pos = position + CalcDistance (Anchor, AnchorType.TopLeft, Size);
        MatrixImage ??= DataMatrixImage.GenerateMatrixImage (Text, Encoding, Rows, Columns);

        if (QuietZone > 0)
        {
            var sizeWithZone = new XSize (Size.Width, Size.Height);
            sizeWithZone.Width = sizeWithZone.Width / (Columns + 2 * QuietZone) * Columns;
            sizeWithZone.Height = sizeWithZone.Height / (Rows + 2 * QuietZone) * Rows;

            var posWithZone = new XPoint (pos.X, pos.Y);
            posWithZone.X += Size.Width / (Columns + 2 * QuietZone) * QuietZone;
            posWithZone.Y += Size.Height / (Rows + 2 * QuietZone) * QuietZone;

            graphics.DrawRectangle (XBrushes.White, pos.X, pos.Y, Size.Width, Size.Height);
            graphics.DrawImage
                (
                    MatrixImage,
                    posWithZone.X,
                    posWithZone.Y,
                    sizeWithZone.Width,
                    sizeWithZone.Height
                );
        }
        else
        {
            graphics.DrawImage (MatrixImage, pos.X, pos.Y, Size.Width, Size.Height);
        }

        graphics.Restore (state);
    }

    /// <inheritdoc cref="MatrixCode.CheckCode"/>
    protected override void CheckCode
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var image = new DataMatrixImage (Text, Encoding, Rows, Columns);
        image.Iec16022Ecc200
            (
                Columns,
                Rows,
                Encoding,
                Text.Length,
                Text,
                0,
                0,
                0
            );
    }

    #endregion
}
