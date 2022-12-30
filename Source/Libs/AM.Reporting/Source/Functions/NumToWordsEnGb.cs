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
using System.Text;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace FastReport.Functions
{
  internal class NumToWordsEnGb : NumToWordsEn
  {
    private static WordInfo milliards = new WordInfo("milliard");
    private static WordInfo trillions = new WordInfo("billion");

    protected override WordInfo GetMilliards()
    {
      return milliards;
    }

    protected override WordInfo GetTrillions()
    {
      return trillions;
    }
  }
}