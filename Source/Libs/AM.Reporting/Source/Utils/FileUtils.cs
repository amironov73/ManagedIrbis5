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
using System.IO;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
  internal static class FileUtils
  {
    public static string GetRelativePath(string absPath, string baseDirectoryPath)
    {
      char[] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
      baseDirectoryPath = Path.GetFullPath(baseDirectoryPath);
      absPath = Path.GetFullPath(absPath);
      baseDirectoryPath = baseDirectoryPath.TrimEnd(separators);

      string[] bPath = baseDirectoryPath.Split(separators);
      string[] aPath = absPath.Split(separators);
      int indx = 0;
      while (indx < Math.Min(bPath.Length, aPath.Length))
      {
        if (String.Compare(aPath[indx], bPath[indx], true) != 0)
          break;
        indx++;
      }
      // no matches, return absPath
      if (indx == 0)
        return absPath;

      string result = "";
      for (int i = indx; i < bPath.Length; i++)
      {
        result += ".." + Path.DirectorySeparatorChar;
      }
      result += String.Join(Path.DirectorySeparatorChar.ToString(), aPath, indx, aPath.Length - indx);
      return result;
    }
  }
}
