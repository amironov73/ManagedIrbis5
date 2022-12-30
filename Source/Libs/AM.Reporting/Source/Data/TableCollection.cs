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
    /// Represents the collection of <see cref="TableDataSource"/> objects.
    /// </summary>
    public class TableCollection : FRCollectionBase
    {
        /// <summary>
        /// Gets or sets a data table.
        /// </summary>
        /// <param name="index">The index of a data table in this collection.</param>
        /// <returns>The data table with specified index.</returns>
        public TableDataSource this [int index]
        {
            get => List[index] as TableDataSource;
            set => List[index] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCollection"/> class with default settings.
        /// </summary>
        /// <param name="owner">The owner of this collection.</param>
        public TableCollection (Base owner) : base (owner)
        {
        }
    }
}
