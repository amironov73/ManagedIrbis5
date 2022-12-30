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
    /*/// <summary> <p>See ISO 18004:2006, 6.4.1, Tables 2 and 3. This enum encapsulates the various modes in which
    /// data can be encoded to bits in the QR code standard.</p>
    ///
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>*/
    internal sealed class Mode
    {
        public int Bits { get; }

        public string Name { get; }

        // No, we can't use an enum here. J2ME doesn't support it.

        //UPGRADE_NOTE: Final was removed from the declaration of 'TERMINATOR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode
            TERMINATOR = new Mode (new int[] { 0, 0, 0 }, 0x00, "TERMINATOR"); // Not really a mode...

        //UPGRADE_NOTE: Final was removed from the declaration of 'NUMERIC '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode NUMERIC = new Mode (new int[] { 10, 12, 14 }, 0x01, "NUMERIC");

        //UPGRADE_NOTE: Final was removed from the declaration of 'ALPHANUMERIC '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode ALPHANUMERIC = new Mode (new int[] { 9, 11, 13 }, 0x02, "ALPHANUMERIC");

        //UPGRADE_NOTE: Final was removed from the declaration of 'STRUCTURED_APPEND '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode
            STRUCTURED_APPEND = new Mode (new int[] { 0, 0, 0 }, 0x03, "STRUCTURED_APPEND"); // Not supported

        //UPGRADE_NOTE: Final was removed from the declaration of 'BYTE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode BYTE = new Mode (new int[] { 8, 16, 16 }, 0x04, "BYTE");

        //UPGRADE_NOTE: Final was removed from the declaration of 'ECI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode ECI = new Mode (null, 0x07, "ECI"); // character counts don't apply

        //UPGRADE_NOTE: Final was removed from the declaration of 'KANJI '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode KANJI = new Mode (new int[] { 8, 10, 12 }, 0x08, "KANJI");

        //UPGRADE_NOTE: Final was removed from the declaration of 'FNC1_FIRST_POSITION '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode FNC1_FIRST_POSITION = new Mode (null, 0x05, "FNC1_FIRST_POSITION");

        //UPGRADE_NOTE: Final was removed from the declaration of 'FNC1_SECOND_POSITION '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly Mode FNC1_SECOND_POSITION = new Mode (null, 0x09, "FNC1_SECOND_POSITION");

        //UPGRADE_NOTE: Final was removed from the declaration of 'characterCountBitsForVersions '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private int[] characterCountBitsForVersions;

        //UPGRADE_NOTE: Final was removed from the declaration of 'bits '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        //UPGRADE_NOTE: Final was removed from the declaration of 'name '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        private Mode (int[] characterCountBitsForVersions, int bits, string name)
        {
            this.characterCountBitsForVersions = characterCountBitsForVersions;
            this.Bits = bits;
            this.Name = name;
        }

        /*/// <param name="version">version in question
        /// </param>
        /// <returns> number of bits used, in this QR Code symbol {@link Version}, to encode the
        /// count of characters that will follow encoded in this {@link Mode}
        /// </returns>*/
        public int getCharacterCountBits (Version version)
        {
            if (characterCountBitsForVersions == null)
            {
                throw new ArgumentException ("Character count doesn't apply to this mode");
            }

            var number = version.VersionNumber;
            int offset;
            if (number <= 9)
            {
                offset = 0;
            }
            else if (number <= 26)
            {
                offset = 1;
            }
            else
            {
                offset = 2;
            }

            return characterCountBitsForVersions[offset];
        }
    }
}
