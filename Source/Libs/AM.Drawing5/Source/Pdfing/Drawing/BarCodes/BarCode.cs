// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

/* BarCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Represents the base class of all bar codes.
/// </summary>
public abstract class BarCode
    : CodeBase
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="BarCode"/> class.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="size"></param>
    /// <param name="direction"></param>
    protected BarCode
        (
            string text,
            XSize size,
            CodeDirection direction
        )
        : base (text, size, direction)
    {
        Text = text;
        Size = size;
        Direction = direction;
    }

    #endregion

    /// <summary>
    /// Creates a bar code from the specified code type.
    /// </summary>
    public static BarCode FromType (CodeType type, string text, XSize size, CodeDirection direction)
    {
        switch (type)
        {
            case CodeType.Code2of5Interleaved:
                return new Code2of5Interleaved (text, size, direction);

            case CodeType.Code3of9Standard:
                return new Code3of9Standard (text, size, direction);

            default:
                throw new InvalidEnumArgumentException ("type", (int)type, typeof (CodeType));
        }
    }

    /// <summary>
    /// Creates a bar code from the specified code type.
    /// </summary>
    public static BarCode FromType (CodeType type, string text, XSize size)
    {
        return FromType (type, text, size, CodeDirection.LeftToRight);
    }

    /// <summary>
    /// Creates a bar code from the specified code type.
    /// </summary>
    public static BarCode FromType (CodeType type, string text)
    {
        return FromType (type, text, XSize.Empty, CodeDirection.LeftToRight);
    }

    /// <summary>
    /// Creates a bar code from the specified code type.
    /// </summary>
    public static BarCode FromType (CodeType type)
    {
        return FromType (type, String.Empty, XSize.Empty, CodeDirection.LeftToRight);
    }

    /// <summary>
    /// When overridden in a derived class gets or sets the wide narrow ratio.
    /// </summary>
    public virtual double WideNarrowRatio
    {
        get { return 0; }
        set { }
    }

    /// <summary>
    /// Gets or sets the location of the text next to the bar code.
    /// </summary>
    public TextLocation TextLocation { get; set; }

    /// <summary>
    /// Gets or sets the length of the data that defines the bar code.
    /// </summary>
    public int DataLength { get; set; }

    /// <summary>
    /// Gets or sets the optional start character.
    /// </summary>
    public char StartChar { get; set; }

    /// <summary>
    /// Gets or sets the optional end character.
    /// </summary>
    public char EndChar { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the turbo bit is to be drawn.
    /// (A turbo bit is something special to Kern (computer output processing) company (as far as I know))
    /// </summary>
    public virtual bool TurboBit { get; set; }

    internal virtual void InitRendering (BarCodeRenderInfo info)
    {
        if (Text == null)
        {
            throw new InvalidOperationException (BcgSR.BarCodeNotSet);
        }

        if (Size.IsEmpty)
        {
            throw new InvalidOperationException (BcgSR.EmptyBarCodeSize);
        }
    }

    /// <summary>
    /// When defined in a derived class renders the code.
    /// </summary>
    protected internal abstract void Render (XGraphics gfx, XBrush brush, XFont font, XPoint position);
}
