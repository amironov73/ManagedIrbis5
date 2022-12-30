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
using System.Text;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Table
{
  /// <summary>
  /// Represents a collection of <see cref="TableRow"/> objects.
  /// </summary>
  public class TableRowCollection : FRCollectionBase
  {
    /// <summary>
    /// Gets a row with specified index.
    /// </summary>
    /// <param name="index">Index of a row.</param>
    /// <returns>The row with specified index.</returns>
    public TableRow this[int index]
    {
      get
      {
        TableRow row = List[index] as TableRow;
        row.SetIndex(index);
        return row;
      }
    }

    /// <inheritdoc/>
    protected override void OnInsert(int index, object value)
    {
      base.OnInsert(index, value);
      if (Owner != null)
        (Owner as TableBase).CorrectSpansOnRowChange(index, 1);
    }

    /// <inheritdoc/>
    protected override void OnRemove(int index, object value)
    {
      base.OnRemove(index, value);
      if (Owner != null)
        (Owner as TableBase).CorrectSpansOnRowChange(index, -1);
    }

    internal TableRowCollection(Base owner) : base(owner)
    {
    }
  }
}
