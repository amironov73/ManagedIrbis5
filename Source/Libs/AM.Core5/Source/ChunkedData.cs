// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ChunkedData.cs -- данные как массив чанков
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Data in chunks.
    /// </summary>
    public class ChunkedData<T>
    {
        #region Properties

        /// <summary>
        /// List of chunks.
        /// </summary>
        public List<Memory<T>> Chunks { get; } = new List<Memory<T>>();

        /// <summary>
        /// Total size.
        /// </summary>
        public int Size
        {
            get
            {
                int result = Chunks.Sum(chunk => chunk.Length);
                return result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Append the chunk.
        /// </summary>
        public ChunkedData<T> Append
            (
                Memory<T> chunk
            )
        {
            Chunks.Add (chunk);
            return this;
        }


        #endregion
    }
}
