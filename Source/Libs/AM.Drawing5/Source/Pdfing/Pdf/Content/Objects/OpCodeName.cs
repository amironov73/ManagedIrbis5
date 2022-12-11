// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* OpCodeName.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#pragma warning disable 1591

namespace PdfSharpCore.Pdf.Content.Objects;

/// <summary>
/// The names of the op-codes.
/// </summary>
public enum OpCodeName
{
    Dictionary, // Name followed by dictionary.

    // I know that this is not useable in VB or other languages with no case sensitivity.
    b,
    B,
    bx,
    Bx,
    BDC,
    BI,
    BMC,
    BT,
    BX,
    c,
    cm,
    CS,
    cs,
    d,
    d0,
    d1,
    Do,
    DP,
    EI,
    EMC,
    ET,
    EX,
    f,
    F,
    fx,
    G,
    g,
    gs,
    h,
    i,
    ID,
    j,
    J,
    K,
    k,
    l,
    m,
    M,
    MP,
    n,
    q,
    Q,
    re,
    RG,
    rg,
    ri,
    s,
    S,
    SC,
    sc,
    SCN,
    scn,
    sh,
    Tx,
    Tc,
    Td,
    TD,
    Tf,
    Tj,
    TJ,
    TL,
    Tm,
    Tr,
    Ts,
    Tw,
    Tz,
    v,
    w,
    W,
    Wx,
    y,
    QuoteSingle,
    QuoteDbl,
}
