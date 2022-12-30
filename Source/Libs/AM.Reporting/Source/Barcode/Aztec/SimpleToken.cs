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

namespace FastReport.Barcode.Aztec
{
   internal sealed class SimpleToken : Token
   {
      // For normal words, indicates value and bitCount
      private readonly short value;
      private readonly short bitCount;

      public SimpleToken(Token previous, int value, int bitCount)
         : base(previous)
      {
         this.value = (short) value;
         this.bitCount = (short) bitCount;
      }

      public override void appendTo(BitArray bitArray, byte[] text)
      {
         bitArray.appendBits(value, bitCount);
      }

      public override String ToString()
      {
         int value = this.value & ((1 << bitCount) - 1);
         value |= 1 << bitCount;
         return '<' + SupportClass.ToBinaryString(value | (1 << bitCount)).Substring(1) + '>';
      }
   }
}