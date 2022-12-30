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

namespace AM.Reporting.Barcode.Aztec
{
    internal abstract class Token
    {
        public static Token EMPTY = new SimpleToken (null, 0, 0);

        protected Token (Token previous)
        {
            this.Previous = previous;
        }

        public Token Previous { get; }

        public Token add (int value, int bitCount)
        {
            return new SimpleToken (this, value, bitCount);
        }

        public Token addBinaryShift (int start, int byteCount)
        {
            var bitCount = (byteCount * 8) + (byteCount <= 31 ? 10 : byteCount <= 62 ? 20 : 21);
            return new BinaryShiftToken (this, start, byteCount);
        }

        public abstract void appendTo (BitArray bitArray, byte[] text);
    }
}
