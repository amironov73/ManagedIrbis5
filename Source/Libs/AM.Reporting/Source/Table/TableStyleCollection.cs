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

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Table
{
  internal class TableStyleCollection : FRCollectionBase
  {
    private TableCell defaultStyle;

    public TableCell DefaultStyle
    {
      get { return defaultStyle; }
    }

    public TableCell this[int index]
    {
      get { return List[index] as TableCell; }
      set { List[index] = value; }
    }

    public TableCell Add(TableCell style)
    {
      for (int i = 0; i < Count; i++)
      {
        if (this[i].Equals(style))
          return this[i];
      }

      TableCell newStyle = new TableCell();
      newStyle.Assign(style);
      List.Add(newStyle);
      return newStyle;
    }

    public TableStyleCollection() : base(null)
    {
      defaultStyle = new TableCell();
    }
  }
}
