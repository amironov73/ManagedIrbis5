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

namespace AM.Reporting.Data
{
  /// <summary>
  /// Represents a datasource based on <b>DataView</b> class.
  /// </summary>
  /// <remarks>
  /// This class is used to support AM.Reporting.Net infrastructure, do not use it directly.
  /// If you want to use data from <b>DataView</b> object, call the
  /// <see cref="AM.Reporting.Report.RegisterData(DataView, string)"/> method of the <b>Report</b>.
  /// </remarks>
  public class SliceCubeSource : CubeSourceBase
  {
    #region Properties
    #endregion

    #region Private Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Public Methods
    #endregion
  }
}
