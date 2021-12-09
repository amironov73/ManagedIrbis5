// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IMereSerializable.cs -- интерфейс объекта, умеющего самостоятельно сериализовываться
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Runtime.Mere
{
    /// <summary>
    /// Интерфейс объекта, умеющего самостоятельно сериализовываться.
    /// </summary>
    public interface IMereSerializable
    {
        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        void MereSerialize (BinaryWriter writer);

        /// <summary>
        /// Восстановление из потока.
        /// </summary>
        void MereDeserialize (BinaryReader reader);
    }
}
