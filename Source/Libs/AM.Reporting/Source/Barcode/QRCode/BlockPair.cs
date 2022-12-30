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

namespace FastReport.Barcode.QRCode
{
  internal sealed class BlockPair
  {
    public ByteArray DataBytes
    {
      get
      {
        return dataBytes;
      }
      
    }
    public ByteArray ErrorCorrectionBytes
    {
      get
      {
        return errorCorrectionBytes;
      }
      
    }
    
    //UPGRADE_NOTE: Final was removed from the declaration of 'dataBytes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
    private ByteArray dataBytes;
    //UPGRADE_NOTE: Final was removed from the declaration of 'errorCorrectionBytes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
    private ByteArray errorCorrectionBytes;
    
    internal BlockPair(ByteArray data, ByteArray errorCorrection)
    {
      dataBytes = data;
      errorCorrectionBytes = errorCorrection;
    }
  }
}