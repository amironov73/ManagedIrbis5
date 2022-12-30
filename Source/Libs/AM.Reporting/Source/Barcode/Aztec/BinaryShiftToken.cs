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

namespace AM.Reporting.Barcode.Aztec
{
    internal sealed class BinaryShiftToken : Token
    {
        private readonly short binaryShiftStart;
        private readonly short binaryShiftByteCount;

        public BinaryShiftToken (Token previous,
            int binaryShiftStart,
            int binaryShiftByteCount)
            : base (previous)
        {
            this.binaryShiftStart = (short)binaryShiftStart;
            this.binaryShiftByteCount = (short)binaryShiftByteCount;
        }

        public override void appendTo (BitArray bitArray, byte[] text)
        {
            for (var i = 0; i < binaryShiftByteCount; i++)
            {
                if (i == 0 || (i == 31 && binaryShiftByteCount <= 62))
                {
                    // We need a header before the first character, and before
                    // character 31 when the total byte code is <= 62
                    bitArray.appendBits (31, 5); // BINARY_SHIFT
                    if (binaryShiftByteCount > 62)
                    {
                        bitArray.appendBits (binaryShiftByteCount - 31, 16);
                    }
                    else if (i == 0)
                    {
                        // 1 <= binaryShiftByteCode <= 62
                        bitArray.appendBits (Math.Min (binaryShiftByteCount, (short)31), 5);
                    }
                    else
                    {
                        // 32 <= binaryShiftCount <= 62 and i == 31
                        bitArray.appendBits (binaryShiftByteCount - 31, 5);
                    }
                }

                bitArray.appendBits (text[binaryShiftStart + i], 8);
            }
        }

        public override string ToString()
        {
            return "<" + binaryShiftStart + "::" + (binaryShiftStart + binaryShiftByteCount - 1) + '>';
        }
    }
}
