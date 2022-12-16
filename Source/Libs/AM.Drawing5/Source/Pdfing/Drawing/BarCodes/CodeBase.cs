// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CodeBase.cs -- базовый класс для всех поддерживаемых штрих-кодов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Базовый класс для всех поддерживаемых штрих-кодов.
/// </summary>
public abstract class CodeBase
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBase"/> class.
    /// </summary>
    protected CodeBase
        (
            string text,
            XSize size,
            CodeDirection direction
        )
    {
        _text = text;
        Size = size;
        Direction = direction;
    }

    #endregion

    //public static CodeBase FromType(CodeType type, string text, XSize size, CodeDirection direction)
    //{
    //  switch (type)
    //  {
    //    case CodeType.Code2of5Interleaved:
    //      return new Code2of5Interleaved(text, size, direction);

    //    case CodeType.Code3of9Standard:
    //      return new Code3of9Standard(text, size, direction);

    //    default:
    //      throw new InvalidEnumArgumentException("type", (int)type, typeof(CodeType));
    //  }
    //}

    //public static CodeBase FromType(CodeType type, string text, XSize size)
    //{
    //  return FromType(type, text, size, CodeDirection.LeftToRight);
    //}

    //public static CodeBase FromType(CodeType type, string text)
    //{
    //  return FromType(type, text, XSize.Empty, CodeDirection.LeftToRight);
    //}

    //public static CodeBase FromType(CodeType type)
    //{
    //  return FromType(type, String.Empty, XSize.Empty, CodeDirection.LeftToRight);
    //}

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    public XSize Size { get; set; }

    /// <summary>
    /// Gets or sets the text the bar code shall represent.
    /// </summary>
    public string Text
    {
        get => _text;
        set
        {
            CheckCode (value);
            _text = value;
        }
    }

    private string _text;

    /// <summary>
    /// Always MiddleCenter.
    /// </summary>
    public AnchorType Anchor { get; set; }

    /// <summary>
    /// Gets or sets the drawing direction.
    /// </summary>
    public CodeDirection Direction { get; set; }

    /// <summary>
    /// When implemented in a derived class, determines whether the specified string can be used as Text
    /// for this bar code type.
    /// </summary>
    /// <param name="text">The code string to check.</param>
    /// <returns>True if the text can be used for the actual barcode.</returns>
    protected abstract void CheckCode (string text);

    /// <summary>
    /// Calculates the distance between an old anchor point and a new anchor point.
    /// </summary>
    /// <param name="oldType"></param>
    /// <param name="newType"></param>
    /// <param name="size"></param>
    public static XVector CalcDistance
        (
            AnchorType oldType,
            AnchorType newType,
            XSize size
        )
    {
        if (oldType == newType)
            return new XVector();

        XVector result;
        Delta delta = Deltas[(int)oldType, (int)newType];
        result = new XVector (size.Width / 2 * delta.X, size.Height / 2 * delta.Y);
        return result;
    }

    struct Delta
    {
        public Delta (int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly int X;
        public readonly int Y;
    }

    static readonly Delta[,] Deltas = new Delta[9, 9]
    {
        {
            new (0, 0), new (1, 0), new (2, 0), new (0, 1), new (1, 1), new (2, 1),
            new (0, 2), new (1, 2), new (2, 2)
        },
        {
            new (-1, 0), new (0, 0), new (1, 0), new (-1, 1), new (0, 1),
            new (1, 1), new (-1, 2), new (0, 2), new (1, 2)
        },
        {
            new (-2, 0), new (-1, 0), new (0, 0), new (-2, 1), new (-1, 1),
            new (0, 1), new (-2, 2), new (-1, 2), new (0, 2)
        },
        {
            new (0, -1), new (1, -1), new (2, -1), new (0, 0), new (1, 0),
            new (2, 0), new (0, 1), new (1, 1), new (2, 1)
        },
        {
            new (-1, -1), new (0, -1), new (1, -1), new (-1, 0), new (0, 0),
            new (1, 0), new (-1, 1), new (0, 1), new (1, 1)
        },
        {
            new (-2, -1), new (-1, -1), new (0, -1), new (-2, 0), new (-1, 0),
            new (0, 0), new (-2, 1), new (-1, 1), new (0, 1)
        },
        {
            new (0, -2), new (1, -2), new (2, -2), new (0, -1), new (1, -1),
            new (2, -1), new (0, 0), new (1, 0), new (2, 0)
        },
        {
            new (-1, -2), new (0, -2), new (1, -2), new (-1, -1), new (0, -1),
            new (1, -1), new (-1, 0), new (0, 0), new (1, 0)
        },
        {
            new (-2, -2), new (-1, -2), new (0, -2), new (-2, -1), new (-1, -1),
            new (0, -1), new (-2, 0), new (-1, 0), new (0, 0)
        },
    };
}
