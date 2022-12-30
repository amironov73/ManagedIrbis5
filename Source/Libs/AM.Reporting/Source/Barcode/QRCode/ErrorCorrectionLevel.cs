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

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Reporting.Barcode.QRCode
{
    /*/// <summary> <p>See ISO 18004:2006, 6.5.1. This enum encapsulates the four error correction levels
    /// defined by the QR code standard.</p>
    ///
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>*/
    internal sealed class ErrorCorrectionLevel
    {
        public int Bits { get; }

        public string Name { get; }

        // No, we can't use an enum here. J2ME doesn't support it.

        // <summary> L = ~7% correction</summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'L '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly ErrorCorrectionLevel L = new ErrorCorrectionLevel (0, 0x01, "L");

        // <summary> M = ~15% correction</summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'M '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly ErrorCorrectionLevel M = new ErrorCorrectionLevel (1, 0x00, "M");

        // <summary> Q = ~25% correction</summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'Q '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly ErrorCorrectionLevel Q = new ErrorCorrectionLevel (2, 0x03, "Q");

        // <summary> H = ~30% correction</summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'H '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly ErrorCorrectionLevel H = new ErrorCorrectionLevel (3, 0x02, "H");

        //UPGRADE_NOTE: Final was removed from the declaration of 'FOR_BITS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly ErrorCorrectionLevel[] FOR_BITS = new ErrorCorrectionLevel[] { M, L, H, Q };

        //UPGRADE_NOTE: Final was removed from the declaration of 'ordinal '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int ordinal_Renamed_Field;

        //UPGRADE_NOTE: Final was removed from the declaration of 'bits '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        //UPGRADE_NOTE: Final was removed from the declaration of 'name '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        private ErrorCorrectionLevel (int ordinal, int bits, string name)
        {
            ordinal_Renamed_Field = ordinal;
            this.Bits = bits;
            this.Name = name;
        }

        public int ordinal()
        {
            return ordinal_Renamed_Field;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
