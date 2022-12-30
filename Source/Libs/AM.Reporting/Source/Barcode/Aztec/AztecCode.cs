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
   /// <summary>
   /// Aztec 2D code representation
   /// </summary>
   /// <author>Rustam Abdullaev</author>
   internal sealed class AztecCode
   {
       private bool is_Compact;
       private int size;
       private int layers;
       private int codeWords;
       private BitMatrix matrix;

      /// <summary>
      /// Compact or full symbol indicator
      /// </summary>
      public bool isCompact
      {
          get { return is_Compact; }
          set { is_Compact = value; }
      }

      /// <summary>
      /// Size in pixels (width and height)
      /// </summary>
      public int Size
      {
          get { return size; }
          set { size = value; }
      }

      /// <summary>
      /// Number of levels
      /// </summary>
      public int Layers
      {
          get { return layers; }
          set { layers = value; }
      }

      /// <summary>
      /// Number of data codewords
      /// </summary>
      public int CodeWords
      {
          get { return codeWords; }
          set { codeWords = value; }
      }

      /// <summary>
      /// The symbol image
      /// </summary>
      public BitMatrix Matrix
      {
          get { return matrix; }
          set { matrix = value; }
      }
   }
}
