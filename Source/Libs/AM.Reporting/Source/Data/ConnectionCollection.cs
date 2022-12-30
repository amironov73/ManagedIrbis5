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
    /// Represents the collection of <see cref="DataConnectionBase"/> objects.
    /// </summary>
    public class ConnectionCollection : FRCollectionBase
    {
        /// <summary>
        /// Gets or sets a data connection.
        /// </summary>
        /// <param name="index">The index of a data connection in this collection.</param>
        /// <returns>The data connection with specified index.</returns>
        public DataConnectionBase this [int index]
        {
            get => List[index] as DataConnectionBase;
            set => List[index] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionCollection"/> class with default settings.
        /// </summary>
        /// <param name="owner">The owner of this collection.</param>
        public ConnectionCollection (Base owner) : base (owner)
        {
        }
    }
}
