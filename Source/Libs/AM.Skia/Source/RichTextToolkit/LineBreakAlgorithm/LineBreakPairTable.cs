// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Skia.RichTextKit;

internal static class LineBreakPairTable
{
    public const byte DI_BRK = 0; // Direct break opportunity
    public const byte IN_BRK = 1; // Indirect break opportunity
    public const byte CI_BRK = 2; // Indirect break opportunity for combining marks
    public const byte CP_BRK = 3; // Prohibited break for combining marks
    public const byte PR_BRK = 4; // Prohibited break

    // Based on example pair table from https://www.unicode.org/reports/tr14/tr14-37.html#Table2
    // - ZWJ special processing for LB8a
    // - CB manually added as per Rule LB20
    public static byte[][] table = new byte[][]
    {
        //           OP    , CL    , CP    , QU    , GL    , NS    , EX    , SY    , IS    , PR    , PO    , NU    , AL    , HL    , ID    , IN    , HY    , BA    , BB    , B2    , ZW    , CM    , WJ    , H2    , H3    , JL    , JV    , JT    , RI    , EB    , EM    , ZWJ   , CB
        new byte[]
        {
            PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK,
            PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, CP_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK,
            PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK
        }, // OP
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // CL
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // CP
        new byte[]
        {
            PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, CI_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK
        }, // QU
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, CI_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK
        }, // GL
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // NS
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // EX
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK,
            IN_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // SY
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // IS
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK
        }, // PR
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // PO
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // NU
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // AL
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // HL
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // ID
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // IN
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, DI_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // HY
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, DI_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // BA
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, CI_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK
        }, // BB
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, PR_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // B2
        new byte[]
        {
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK
        }, // ZW
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // CM
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, CI_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK
        }, // WJ
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            IN_BRK, IN_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // H2
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, IN_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // H3
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // JL
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            IN_BRK, IN_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // JV
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, IN_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // JT
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // RI
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, IN_BRK, DI_BRK
        }, // EB
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, IN_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // EM
        new byte[]
        {
            IN_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, PR_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, IN_BRK, IN_BRK,
            IN_BRK, DI_BRK, IN_BRK, IN_BRK, IN_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        }, // ZWJ
        new byte[]
        {
            DI_BRK, PR_BRK, PR_BRK, IN_BRK, IN_BRK, DI_BRK, PR_BRK, PR_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, PR_BRK, CI_BRK, PR_BRK, DI_BRK, DI_BRK, DI_BRK,
            DI_BRK, DI_BRK, DI_BRK, DI_BRK, DI_BRK, IN_BRK, DI_BRK
        } // CB
    };
}
