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
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
  internal static class ShortProperties
  {
    private static SortedList<string, string> FProps = new SortedList<string, string>();

    public static void Add(string shortName, string name)
    {
      if (!FProps.ContainsKey(shortName))
        FProps.Add(shortName, name);
    }

    public static string GetFullName(string name)
    {
      int i = FProps.IndexOfKey(name);
      return i == -1 ? name : FProps.Values[i];
    }

    public static string GetShortName(string name)
    {
      int i = FProps.IndexOfValue(name);
      if (i != -1)
        return FProps.Keys[i];
      else
        return name;
    }

    static ShortProperties()
    {
      Add("l", "Left");
      Add("t", "Top");
      Add("w", "Width");
      Add("h", "Height");
      Add("x", "Text");
    }
  }
}
