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

using System.Diagnostics;
using System.Collections.Generic;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Content.Objects;

/// <summary>
/// Represents a PDF content stream operator description.
/// </summary>
public sealed class OpCode
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="OpCode"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="opcodeName">The enum value of the operator.</param>
    /// <param name="operands">The number of operands.</param>
    /// <param name="postscript">The postscript equivalent, or null, if no such operation exists.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="description">The description from Adobe PDF Reference.</param>
    internal OpCode
        (
            string name,
            OpCodeName opcodeName,
            int operands,
            string? postscript,
            OpCodeFlags flags,
            string description
        )
    {
        Name = name;
        OpCodeName = opcodeName;
        Operands = operands;
        Postscript = postscript;
        Flags = flags;
        Description = description;
    }

    #endregion

    /// <summary>
    /// The name of the operator.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The enum value of the operator.
    /// </summary>
    public readonly OpCodeName OpCodeName;

    /// <summary>
    /// The number of operands. -1 indicates a variable number of operands.
    /// </summary>
    public readonly int Operands;

    /// <summary>
    /// The flags.
    /// </summary>
    public readonly OpCodeFlags Flags;

    /// <summary>
    /// The postscript equivalent, or null, if no such operation exists.
    /// </summary>
    public readonly string? Postscript;

    /// <summary>
    /// The description from Adobe PDF Reference.
    /// </summary>
    public readonly string Description;
}

/// <summary>
/// Static class with all PDF op-codes.
/// </summary>
public static class OpCodes
{
    /// <summary>
    /// Operators from name.
    /// </summary>
    /// <param name="name">The name.</param>
    public static COperator OperatorFromName (string name)
    {
        COperator? op = null;
        var opcode = StringToOpCode[name];
        if (opcode != null)
        {
            op = new COperator (opcode);
        }
        else
        {
            Debug.Assert (false, "Unknown operator in PDF content stream.");
        }

        return op;
    }

    /// <summary>
    /// Initializes the <see cref="OpCodes"/> class.
    /// </summary>
    static OpCodes()
    {
        StringToOpCode = new Dictionary<string, OpCode>();
        for (var idx = 0; idx < ops.Length; idx++)
        {
            var op = ops[idx];
            StringToOpCode.Add (op.Name, op);
        }
    }

    private static readonly Dictionary<string, OpCode> StringToOpCode;

    // ReSharper disable InconsistentNaming

    private static readonly OpCode Dictionary = new ("Dictionary", OpCodeName.Dictionary, -1, "name, dictionary",
        OpCodeFlags.None,
        "E.g.: /Name << ... >>");

    private static readonly OpCode b = new ("b", OpCodeName.b, 0, "closepath, fill, stroke", OpCodeFlags.None,
        "Close, fill, and stroke path using nonzero winding number");

    private static readonly OpCode B = new ("B", OpCodeName.B, 0, "fill, stroke", OpCodeFlags.None,
        "Fill and stroke path using nonzero winding number rule");

    private static readonly OpCode bx = new ("b*", OpCodeName.bx, 0, "closepath, eofill, stroke", OpCodeFlags.None,
        "Close, fill, and stroke path using even-odd rule");

    private static readonly OpCode Bx = new ("B*", OpCodeName.Bx, 0, "eofill, stroke", OpCodeFlags.None,
        "Fill and stroke path using even-odd rule");

    private static readonly OpCode BDC = new ("BDC", OpCodeName.BDC, -1, null, OpCodeFlags.None,
        "(PDF 1.2) Begin marked-content sequence with property list");

    private static readonly OpCode BI = new ("BI", OpCodeName.BI, 0, null, OpCodeFlags.None,
        "Begin inline image object");

    private static readonly OpCode BMC = new ("BMC", OpCodeName.BMC, 1, null, OpCodeFlags.None,
        "(PDF 1.2) Begin marked-content sequence");

    private static readonly OpCode BT = new ("BT", OpCodeName.BT, 0, null, OpCodeFlags.None,
        "Begin text object");

    private static readonly OpCode BX = new ("BX", OpCodeName.BX, 0, null, OpCodeFlags.None,
        "(PDF 1.1) Begin compatibility section");

    private static readonly OpCode c = new ("c", OpCodeName.c, 6, "curveto", OpCodeFlags.None,
        "Append curved segment to path (three control points)");

    private static readonly OpCode cm = new ("cm", OpCodeName.cm, 6, "concat", OpCodeFlags.None,
        "Concatenate matrix to current transformation matrix");

    private static readonly OpCode CS = new ("CS", OpCodeName.CS, 1, "setcolorspace", OpCodeFlags.None,
        "(PDF 1.1) Set color space for stroking operations");

    private static readonly OpCode cs = new ("cs", OpCodeName.cs, 1, "setcolorspace", OpCodeFlags.None,
        "(PDF 1.1) Set color space for nonstroking operations");

    private static readonly OpCode d = new ("d", OpCodeName.d, 2, "setdash", OpCodeFlags.None,
        "Set line dash pattern");

    private static readonly OpCode d0 = new ("d0", OpCodeName.d0, 2, "setcharwidth", OpCodeFlags.None,
        "Set glyph width in Type 3 font");

    private static readonly OpCode d1 = new ("d1", OpCodeName.d1, 6, "setcachedevice", OpCodeFlags.None,
        "Set glyph width and bounding box in Type 3 font");

    private static readonly OpCode Do = new ("Do", OpCodeName.Do, 1, null, OpCodeFlags.None,
        "Invoke named XObject");

    private static readonly OpCode DP = new ("DP", OpCodeName.DP, 2, null, OpCodeFlags.None,
        "(PDF 1.2) Define marked-content point with property list");

    private static readonly OpCode EI = new ("EI", OpCodeName.EI, 0, null, OpCodeFlags.None,
        "End inline image object");

    private static readonly OpCode EMC = new ("EMC", OpCodeName.EMC, 0, null, OpCodeFlags.None,
        "(PDF 1.2) End marked-content sequence");

    private static readonly OpCode ET = new ("ET", OpCodeName.ET, 0, null, OpCodeFlags.None,
        "End text object");

    private static readonly OpCode EX = new ("EX", OpCodeName.EX, 0, null, OpCodeFlags.None,
        "(PDF 1.1) End compatibility section");

    private static readonly OpCode f = new ("f", OpCodeName.f, 0, "fill", OpCodeFlags.None,
        "Fill path using nonzero winding number rule");

    private static readonly OpCode F = new ("F", OpCodeName.F, 0, "fill", OpCodeFlags.None,
        "Fill path using nonzero winding number rule (obsolete)");

    private static readonly OpCode fx = new ("f*", OpCodeName.fx, 0, "eofill", OpCodeFlags.None,
        "Fill path using even-odd rule");

    private static readonly OpCode G = new ("G", OpCodeName.G, 1, "setgray", OpCodeFlags.None,
        "Set gray level for stroking operations");

    private static readonly OpCode g = new ("g", OpCodeName.g, 1, "setgray", OpCodeFlags.None,
        "Set gray level for nonstroking operations");

    private static readonly OpCode gs = new ("gs", OpCodeName.gs, 1, null, OpCodeFlags.None,
        "(PDF 1.2) Set parameters from graphics state parameter dictionary");

    private static readonly OpCode h = new ("h", OpCodeName.h, 0, "closepath", OpCodeFlags.None,
        "Close subpath");

    private static readonly OpCode i = new ("i", OpCodeName.i, 1, "setflat", OpCodeFlags.None,
        "Set flatness tolerance");

    private static readonly OpCode ID = new ("ID", OpCodeName.ID, 0, null, OpCodeFlags.None,
        "Begin inline image data");

    private static readonly OpCode j = new ("j", OpCodeName.j, 1, "setlinejoin", OpCodeFlags.None,
        "Set line join style");

    private static readonly OpCode J = new ("J", OpCodeName.J, 1, "setlinecap", OpCodeFlags.None,
        "Set line cap style");

    private static readonly OpCode K = new ("K", OpCodeName.K, 4, "setcmykcolor", OpCodeFlags.None,
        "Set CMYK color for stroking operations");

    private static readonly OpCode k = new ("k", OpCodeName.k, 4, "setcmykcolor", OpCodeFlags.None,
        "Set CMYK color for nonstroking operations");

    private static readonly OpCode l = new ("l", OpCodeName.l, 2, "lineto", OpCodeFlags.None,
        "Append straight line segment to path");

    private static readonly OpCode m = new ("m", OpCodeName.m, 2, "moveto", OpCodeFlags.None,
        "Begin new subpath");

    private static readonly OpCode M = new ("M", OpCodeName.M, 1, "setmiterlimit", OpCodeFlags.None,
        "Set miter limit");

    private static readonly OpCode MP = new ("MP", OpCodeName.MP, 1, null, OpCodeFlags.None,
        "(PDF 1.2) Define marked-content point");

    private static readonly OpCode n = new ("n", OpCodeName.n, 0, null, OpCodeFlags.None,
        "End path without filling or stroking");

    private static readonly OpCode q = new ("q", OpCodeName.q, 0, "gsave", OpCodeFlags.None,
        "Save graphics state");

    private static readonly OpCode Q = new ("Q", OpCodeName.Q, 0, "grestore", OpCodeFlags.None,
        "Restore graphics state");

    private static readonly OpCode re = new ("re", OpCodeName.re, 4, null, OpCodeFlags.None,
        "Append rectangle to path");

    private static readonly OpCode RG = new ("RG", OpCodeName.RG, 3, "setrgbcolor", OpCodeFlags.None,
        "Set RGB color for stroking operations");

    private static readonly OpCode rg = new ("rg", OpCodeName.rg, 3, "setrgbcolor", OpCodeFlags.None,
        "Set RGB color for nonstroking operations");

    private static readonly OpCode ri = new ("ri", OpCodeName.ri, 1, null, OpCodeFlags.None,
        "Set color rendering intent");

    private static readonly OpCode s = new ("s", OpCodeName.s, 0, "closepath,stroke", OpCodeFlags.None,
        "Close and stroke path");

    private static readonly OpCode S = new ("S", OpCodeName.S, 0, "stroke", OpCodeFlags.None,
        "Stroke path");

    private static readonly OpCode SC = new ("SC", OpCodeName.SC, -1, "setcolor", OpCodeFlags.None,
        "(PDF 1.1) Set color for stroking operations");

    private static readonly OpCode sc = new ("sc", OpCodeName.sc, -1, "setcolor", OpCodeFlags.None,
        "(PDF 1.1) Set color for nonstroking operations");

    private static readonly OpCode SCN = new ("SCN", OpCodeName.SCN, -1, "setcolor", OpCodeFlags.None,
        "(PDF 1.2) Set color for stroking operations (ICCBased and special color spaces)");

    private static readonly OpCode scn = new ("scn", OpCodeName.scn, -1, "setcolor", OpCodeFlags.None,
        "(PDF 1.2) Set color for nonstroking operations (ICCBased and special color spaces)");

    private static readonly OpCode sh = new ("sh", OpCodeName.sh, 1, "shfill", OpCodeFlags.None,
        "(PDF 1.3) Paint area defined by shading pattern");

    private static readonly OpCode Tx = new ("T*", OpCodeName.Tx, 0, null, OpCodeFlags.None,
        "Move to start of next text line");

    private static readonly OpCode Tc = new ("Tc", OpCodeName.Tc, 1, null, OpCodeFlags.None,
        "Set character spacing");

    private static readonly OpCode Td = new ("Td", OpCodeName.Td, 2, null, OpCodeFlags.None,
        "Move text position");

    private static readonly OpCode TD = new ("TD", OpCodeName.TD, 2, null, OpCodeFlags.None,
        "Move text position and set leading");

    private static readonly OpCode Tf = new ("Tf", OpCodeName.Tf, 2, "selectfont", OpCodeFlags.None,
        "Set text font and size");

    private static readonly OpCode Tj = new ("Tj", OpCodeName.Tj, 1, "show", OpCodeFlags.TextOut,
        "Show text");

    private static readonly OpCode TJ = new ("TJ", OpCodeName.TJ, 1, null, OpCodeFlags.TextOut,
        "Show text, allowing individual glyph positioning");

    private static readonly OpCode TL = new ("TL", OpCodeName.TL, 1, null, OpCodeFlags.None,
        "Set text leading");

    private static readonly OpCode Tm = new ("Tm", OpCodeName.Tm, 6, null, OpCodeFlags.None,
        "Set text matrix and text line matrix");

    private static readonly OpCode Tr = new ("Tr", OpCodeName.Tr, 1, null, OpCodeFlags.None,
        "Set text rendering mode");

    private static readonly OpCode Ts = new ("Ts", OpCodeName.Ts, 1, null, OpCodeFlags.None,
        "Set text rise");

    private static readonly OpCode Tw = new ("Tw", OpCodeName.Tw, 1, null, OpCodeFlags.None,
        "Set word spacing");

    private static readonly OpCode Tz = new ("Tz", OpCodeName.Tz, 1, null, OpCodeFlags.None,
        "Set horizontal text scaling");

    private static readonly OpCode v = new ("v", OpCodeName.v, 4, "curveto", OpCodeFlags.None,
        "Append curved segment to path (initial point replicated)");

    private static readonly OpCode w = new ("w", OpCodeName.w, 1, "setlinewidth", OpCodeFlags.None,
        "Set line width");

    private static readonly OpCode W = new ("W", OpCodeName.W, 0, "clip", OpCodeFlags.None,
        "Set clipping path using nonzero winding number rule");

    private static readonly OpCode Wx = new ("W*", OpCodeName.Wx, 0, "eoclip", OpCodeFlags.None,
        "Set clipping path using even-odd rule");

    private static readonly OpCode y = new ("y", OpCodeName.y, 4, "curveto", OpCodeFlags.None,
        "Append curved segment to path (final point replicated)");

    private static readonly OpCode QuoteSingle = new ("'", OpCodeName.QuoteSingle, 1, null, OpCodeFlags.TextOut,
        "Move to next line and show text");

    private static readonly OpCode QuoteDbl = new ("\"", OpCodeName.QuoteDbl, 3, null, OpCodeFlags.TextOut,
        "Set word and character spacing, move to next line, and show text");

    /// <summary>
    /// Array of all OpCodes.
    /// </summary>
    private static readonly OpCode[] ops = // new OpCode[]
    {
        // Must be defined behind the code above to ensure that the values are initialized.
        Dictionary,
        b, B, bx, Bx, BDC, BI, BMC, BT, BX, c, cm, CS, cs, d, d0, d1, Do,
        DP, EI, EMC, ET, EX, f, F, fx, G, g, gs, h, i, ID, j, J, K, k, l, m, M, MP,
        n, q, Q, re, RG, rg, ri, s, S, SC, sc, SCN, scn, sh,
        Tx, Tc, Td, TD, Tf, Tj, TJ, TL, Tm, Tr, Ts, Tw, Tz, v, w, W, Wx, y,
        QuoteSingle, QuoteDbl
    };

    // ReSharper restore InconsistentNaming
}
