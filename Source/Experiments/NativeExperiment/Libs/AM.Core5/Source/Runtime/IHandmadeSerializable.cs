// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IHandmadeSerializable.cs -- интерфейс объекта, умеющего сериализовываться самостоятельно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

namespace AM.Runtime;

/// <summary>
/// Интерфейс объекта, умеющего сериализовываться самостоятельно.
/// </summary>
public interface IHandmadeSerializable
{
    /// <summary>
    /// Восстановление состояния объекта из потока.
    /// </summary>
    public void RestoreFromStream
        (
            BinaryReader reader
        );

    /// <summary>
    /// Сохранение состояния объекта в поток.
    /// </summary>
    public void SaveToStream
        (
            BinaryWriter writer
        );
}
