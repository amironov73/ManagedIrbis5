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

namespace AM.Reporting.Data
{
  internal class VirtualDataSource : DataSourceBase
  {
    private int virtualRowsCount;

    public int VirtualRowsCount
    {
      get { return virtualRowsCount; }
      set { virtualRowsCount = value; }
    }

    #region Protected Methods
    /// <inheritdoc/>
    protected override object GetValue(Column column)
    {
      return null;
    }
    #endregion

    #region Public Methods
    public override void InitSchema()
    {
     // do nothing
    }

    public override void LoadData(ArrayList rows)
    {
      rows.Clear();
      for (int i = 0; i < virtualRowsCount; i++)
      {
        rows.Add(0);
      }
    }
    #endregion
  }
}
