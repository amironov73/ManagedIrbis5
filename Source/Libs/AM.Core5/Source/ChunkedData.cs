// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ChunkedData.cs -- данные как список чанков
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
    /// Данные как список чанков.
    /// </summary>
    public sealed class ChunkedData<T>
    {
        #region Properties

        /// <summary>
        /// Список чанков.
        /// </summary>
        public List<Memory<T>> Chunks { get; } = new ();

        /// <summary>
        /// Общий размер данных (байты).
        /// </summary>
        public int Size => Chunks.Sum (chunk => chunk.Length);

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление чанка.
        /// </summary>
        public ChunkedData<T> Append
            (
                Memory<T> chunk
            )
        {
            Chunks.Add (chunk);

            return this;

        } // method Append

        #endregion

    } // class ChunkeData

} // namespace AM
