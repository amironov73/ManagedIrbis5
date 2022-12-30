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

/*  /// <summary> A class which wraps a 2D array of bytes. The default usage is signed. If you want to use it as a
  /// unsigned container, it's up to you to do byteValue &amp; 0xff at each location.
  ///
  /// JAVAPORT: The original code was a 2D array of ints, but since it only ever gets assigned
  /// -1, 0, and 1, I'm going to use less memory and go with bytes.
  ///
  /// </summary>
  /// <author>  dswitkin@google.com (Daniel Switkin)
  /// </author>
  /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  /// </author>*/
  internal sealed class ByteMatrix
  {
    public int Height
    {
      get
      {
        return height;
      }

    }
    public int Width
    {
      get
      {
        return width;
      }

    }
    public sbyte[][] Array
    {
      get
      {
        return bytes;
      }

    }

    //UPGRADE_NOTE: Final was removed from the declaration of 'bytes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
    private sbyte[][] bytes;
    //UPGRADE_NOTE: Final was removed from the declaration of 'width '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
    private int width;
    //UPGRADE_NOTE: Final was removed from the declaration of 'height '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
    private int height;

    public ByteMatrix(int width, int height)
    {
      bytes = new sbyte[height][];
      for (int i = 0; i < height; i++)
      {
        bytes[i] = new sbyte[width];
      }
      this.width = width;
      this.height = height;
    }

    public sbyte get_Renamed(int x, int y)
    {
      return bytes[y][x];
    }

    public void  set_Renamed(int x, int y, sbyte value_Renamed)
    {
      bytes[y][x] = value_Renamed;
    }

    public void  set_Renamed(int x, int y, int value_Renamed)
    {
      bytes[y][x] = (sbyte) value_Renamed;
    }

    public void  clear(sbyte value_Renamed)
    {
      for (int y = 0; y < height; ++y)
      {
        for (int x = 0; x < width; ++x)
        {
          bytes[y][x] = value_Renamed;
        }
      }
    }
  }
}
