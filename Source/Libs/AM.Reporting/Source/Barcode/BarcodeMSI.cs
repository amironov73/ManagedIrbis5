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
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace FastReport.Barcode
{
  /// <summary>
  /// Generates the MSI barcode.
  /// </summary>
  public class BarcodeMSI : LinearBarcodeBase
  {
    private static string[] tabelle_MSI = {
      "51515151",    //0
      "51515160",    //1
      "51516051",    //2
      "51516060",    //3
      "51605151",    //4
      "51605160",    //5
      "51606051",    //6
      "51606060",    //7
      "60515151",    //8
      "60515160"     //9
    };

    private int quersumme(int x)
    {
      int sum = 0;

      while (x > 0)
      {
        sum += x % 10;
        x /= 10;
      }

      return sum;
    }

    internal override string GetPattern()
    {
      string result = "60";    // Startcode
      int check_even = 0;
      int check_odd = 0;

      for (int i = 0; i < text.Length; i++)
      {
        if ((i % 2) != 0)
          check_odd = check_odd * 10 + CharToInt(text[i]);
        else
          check_even += CharToInt(text[i]);

        result += tabelle_MSI[CharToInt(text[i])];
      }

      int checksum = quersumme(check_odd * 2) + check_even;

      checksum %= 10;
      if (checksum > 0)
        checksum = 10 - checksum;

      if (CalcCheckSum)
        result += tabelle_MSI[checksum];

      result += "515"; //Stopcode
      return result;
    }
  }
}
