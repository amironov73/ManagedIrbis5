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
  
  /*/// <summary> A base class which covers the range of exceptions which may occur when encoding a barcode using
  /// the Writer framework.
  /// 
  /// </summary>
  /// <author>  dswitkin@google.com (Daniel Switkin)
  /// </author>
  /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
  /// </author>*/
  internal sealed class WriterException:System.Exception
  {
    
    public WriterException():base()
    {
    }
    
    public WriterException(System.String message):base(message)
    {
    }
  }
}