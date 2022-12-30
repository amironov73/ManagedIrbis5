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

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
  /// <summary>
  /// Represents the collection of <see cref="CubeSourceBase"/> objects.
  /// </summary>
  public class CubeSourceCollection : FRCollectionBase
  {
    /// <summary>
    /// Gets or sets a data source.
    /// </summary>
    /// <param name="index">The index of a data source in this collection.</param>
    /// <returns>The data source with specified index.</returns>
    public CubeSourceBase this[int index]
    {
      get { return List[index] as CubeSourceBase; }
      set { List[index] = value; }
    }

    /// <summary>
    /// Finds a CubeSource by its name.
    /// </summary>
    /// <param name="name">The name of a CubeSource.</param>
    /// <returns>The <see cref="CubeSourceBase"/> object if found; otherwise <b>null</b>.</returns>
    public CubeSourceBase FindByName(string name)
    {
      foreach (CubeSourceBase c in this)
      {
        if (c.Name == name)
          return c;
      }
      return null;
    }

    /// <summary>
    /// Finds a CubeSource by its alias.
    /// </summary>
    /// <param name="alias">The alias of a CubeSource.</param>
    /// <returns>The <see cref="CubeSourceBase"/> object if found; otherwise <b>null</b>.</returns>
    public CubeSourceBase FindByAlias(string alias)
    {
      foreach (CubeSourceBase c in this)
      {
        if (c.Alias == alias)
          return c;
      }
      return null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CubeSourceCollection"/> class with default settings.
    /// </summary>
    /// <param name="owner">The owner of this collection.</param>
    public CubeSourceCollection(Base owner) : base(owner)
    {
    }
  }
}
