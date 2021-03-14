// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* DefectList.cs -- defect list for the field of the record
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Defect list for the field of the record.
    /// </summary>
    public sealed class DefectList
        : NonNullCollection<FieldDefect>,
        IHandmadeSerializable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DefectList()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DefectList
            (
                IEnumerable<FieldDefect> defects
            )
        {
            AddRange(defects);
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ClearItems();
            FieldDefect[] array = reader.ReadArray<FieldDefect>();
            AddRange(array);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteArray(ToArray());
        }

        #endregion

    } // class DefectList

} // namespace ManagedIrbis.Quality
