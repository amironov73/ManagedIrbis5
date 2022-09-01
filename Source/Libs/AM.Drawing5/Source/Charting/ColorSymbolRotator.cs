// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ColorSymbolRotator.cs -- получение следующего цвета/символа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Color = System.Drawing.Color;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
///  Класс, используемый для получения следующего цвета/символа
/// для методов GraphPane.AddCurve.
/// </summary>
public class ColorSymbolRotator
{
    #region Static fields

    /// <summary>
    /// The <see cref="Color"/>s <see cref="ColorSymbolRotator"/>
    /// rotates through.
    /// </summary>
    public static readonly Color[] COLORS = new Color[]
    {
        Color.Red,
        Color.Blue,
        Color.Green,
        Color.Purple,
        Color.Cyan,
        Color.Pink,
        Color.LightBlue,
        Color.PaleVioletRed,
        Color.SeaGreen,
        Color.Yellow
    };

    /// <summary>
    /// The <see cref="SymbolType"/>s <see cref="ColorSymbolRotator"/>
    /// rotates through.
    /// </summary>
    public static readonly SymbolType[] SYMBOLS = new SymbolType[]
    {
        SymbolType.Circle,
        SymbolType.Diamond,
        SymbolType.Plus,
        SymbolType.Square,
        SymbolType.Star,
        SymbolType.Triangle,
        SymbolType.TriangleDown,
        SymbolType.XCross,
        SymbolType.HDash,
        SymbolType.VDash
    };

    private static ColorSymbolRotator? _staticInstance;

    #endregion

    #region Fields

    /// <summary>
    /// The index of the next color to be used. Note: may be
    /// > COLORS.Length, it is reset to 0 on the next call if it is.
    /// </summary>
    protected int colorIndex;

    /// <summary>
    /// The index of the next symbol to be used. Note: may be
    /// > SYMBOLS.Length, it is reset to 0 on the next call if it is.
    /// </summary>
    protected int symbolIndex;

    #endregion

    #region Properties

    /// <summary>
    /// Retrieves the next color in the rotation  Calling this
    /// method has the side effect of incrementing the color index.
    /// <seealso cref="NextSymbol"/>
    /// <seealso cref="NextColorIndex"/>
    /// </summary>
    public Color NextColor => COLORS[NextColorIndex];

    /// <summary>
    /// Retrieves the index of the next color to be used.  Calling this
    /// method has the side effect of incrementing the color index.
    /// </summary>
    public int NextColorIndex
    {
        get
        {
            if (colorIndex >= COLORS.Length)
            {
                colorIndex = 0;
            }

            return colorIndex++;
        }
        set => colorIndex = value;
    }

    /// <summary>
    /// Retrieves the next color in the rotation.  Calling this
    /// method has the side effect of incrementing the symbol index.
    /// <seealso cref="NextColor"/>
    /// <seealso cref="NextSymbolIndex"/>
    /// </summary>
    public SymbolType NextSymbol => SYMBOLS[NextSymbolIndex];

    /// <summary>
    /// Retrieves the index of the next symbol to be used.  Calling this
    /// method has the side effect of incrementing the symbol index.
    /// </summary>
    public int NextSymbolIndex
    {
        get
        {
            if (symbolIndex >= SYMBOLS.Length)
            {
                symbolIndex = 0;
            }

            return symbolIndex++;
        }
        set => symbolIndex = value;
    }

    /// <summary>
    /// Retrieves the <see cref="ColorSymbolRotator"/> instance used by the
    /// static methods.
    /// <seealso cref="StaticNextColor"/>
    /// <seealso cref="StaticNextSymbol"/>
    /// </summary>
    public static ColorSymbolRotator StaticInstance =>
        _staticInstance ??= new ColorSymbolRotator();

    /// <summary>
    /// Retrieves the next color from this class's static
    /// <see cref="ColorSymbolRotator"/> instance
    /// <seealso cref="StaticInstance"/>
    /// <seealso cref="StaticNextSymbol"/>
    /// </summary>
    public static Color StaticNextColor => StaticInstance.NextColor;

    /// <summary>
    /// Retrieves the next symbol type from this class's static
    /// <see cref="ColorSymbolRotator"/> instance
    /// <seealso cref="StaticInstance"/>
    /// <seealso cref="StaticNextColor"/>
    /// </summary>
    public static SymbolType StaticNextSymbol => StaticInstance.NextSymbol;

    #endregion
}
