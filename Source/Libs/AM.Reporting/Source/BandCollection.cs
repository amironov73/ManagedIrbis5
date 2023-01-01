// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BandCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// Represents a collection of bands.
/// </summary>
public class BandCollection
    : FRCollectionBase
{
    #region Properties

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">Index of an element.</param>
    /// <returns>The element at the specified index.</returns>
    public BandBase? this [int index]
    {
        get => List[index] as BandBase;
        set => List[index] = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="BandCollection"/> class with default settings.
    /// </summary>
    public BandCollection()
        : this (null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BandCollection"/> class with specified owner.
    /// </summary>
    /// <param name="owner">Owner that owns this collection.</param>
    public BandCollection
        (
            Base? owner
        )
        : base (owner)
    {
        // пустое тело конструктора
    }

    #endregion
}
