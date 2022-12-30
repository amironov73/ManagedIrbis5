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
using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Code
{
  internal class AssemblyCollection : CollectionBase
  {
    public AssemblyDescriptor this[int index]
    {
      get { return List[index] as AssemblyDescriptor; }
      set { List[index] = value; }
    }

    public void AddRange(AssemblyDescriptor[] range)
    {
      foreach (AssemblyDescriptor t in range)
      {
        Add(t);
      }
    }

    public int Add(AssemblyDescriptor value)
    {
      if (value == null)
        return -1;
      return List.Add(value);
    }

    public void Insert(int index, AssemblyDescriptor value)
    {
      if (value != null)
        List.Insert(index, value);
    }

    public void Remove(AssemblyDescriptor value)
    {
      List.Remove(value);
    }

    public int IndexOf(AssemblyDescriptor value)
    {
      return List.IndexOf(value);
    }

    public bool Contains(AssemblyDescriptor value)
    {
      return List.Contains(value);
    }
  }
}
