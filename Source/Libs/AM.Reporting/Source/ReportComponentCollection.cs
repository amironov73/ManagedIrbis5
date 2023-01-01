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
    /// Holds the list of objects of <see cref="ReportComponentBase"/> type.
    /// </summary>
    public class ReportComponentCollection : ReportCollectionBase
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">Index of an element.</param>
        /// <returns>The element at the specified index.</returns>
        public ReportComponentBase this [int index]
        {
            get => List[index] as ReportComponentBase;
            set => List[index] = value;
        }

        internal ReportComponentCollection SortByTop()
        {
            var result = new ReportComponentCollection();
            CopyTo (result);
            result.InnerList.Sort (new TopComparer());
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportComponentCollection"/> class with default settings.
        /// </summary>
        public ReportComponentCollection() : this (null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportComponentCollection"/> class with specified owner.
        /// </summary>
        public ReportComponentCollection (Base owner) : base (owner)
        {
        }


        private class TopComparer : IComparer
        {
            public int Compare (object x, object y)
            {
                return (x as ReportComponentBase).Top.CompareTo ((y as ReportComponentBase).Top);
            }
        }
    }
}
