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
using System.Data;
using System.Collections;

#endregion

#nullable enable

namespace FastReport.Data
{
  internal static class CubeHelper
  {
    public static CubeSourceBase GetCubeSource(Dictionary dictionary, string complexName)
    {
      if (String.IsNullOrEmpty(complexName))
        return null;
      string[] names = complexName.Split('.');
      return dictionary.FindByAlias(names[0]) as CubeSourceBase;
    }
  }
}
