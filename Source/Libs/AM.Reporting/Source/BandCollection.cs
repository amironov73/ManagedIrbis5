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
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
  /// <summary>
  /// Represents a collection of bands.
  /// </summary>
  public class BandCollection : FRCollectionBase
  {
    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">Index of an element.</param>
    /// <returns>The element at the specified index.</returns>
    public BandBase this[int index]
    {
      get { return List[index] as BandBase; }
      set { List[index] = value; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BandCollection"/> class with default settings.
    /// </summary>
    public BandCollection() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BandCollection"/> class with specified owner.
    /// </summary>
    /// <param name="owner">Owner that owns this collection.</param>
    public BandCollection(Base owner) : base(owner)
    {
    }
  }
}
