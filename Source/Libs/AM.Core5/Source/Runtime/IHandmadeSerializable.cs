// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IHandmadeSerializable.cs -- object can be stored to a stream
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// The object can be stored to a stream and restored back.
    /// </summary>
    public interface IHandmadeSerializable
    {
        /// <summary>
        /// Restore the object from a stream.
        /// </summary>
        public void RestoreFromStream
        (
            BinaryReader reader
        );

        /// <summary>
        /// Store the object to a stream.
        /// </summary>
        public void SaveToStream
        (
            BinaryWriter writer
        );
    }
}