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
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Reporting
{
  /// <summary>
  /// This class represents a column header band.
  /// </summary>
  public class ColumnHeaderBand : BandBase
  {
    #region Properties
    /// <summary>
    /// This property is not relevant to this class.
    /// </summary>
    [Browsable(false)]
    public new bool StartNewPage
    {
      get { return base.StartNewPage; }
      set { base.StartNewPage = value; }
    }

    /// <summary>
    /// This property is not relevant to this class.
    /// </summary>
    [Browsable(false)]
    public new bool PrintOnBottom
    {
      get { return base.PrintOnBottom; }
      set { base.PrintOnBottom = value; }
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnHeaderBand"/> class with default settings.
    /// </summary>
    public ColumnHeaderBand()
    {
      FlagUseStartNewPage = false;
    }
  }
}
