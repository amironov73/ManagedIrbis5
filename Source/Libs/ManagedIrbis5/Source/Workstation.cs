// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Workstation.cs -- коды АРМов ИРБИС64.
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis;

/// <summary>
/// Коды АРМ ИРБИС64.
/// Не известные серверу ИРБИС64 коды АРМ
/// приводят к ошибке при регистрации на сервере.
/// </summary>
public enum Workstation
    : byte
{
    /// <summary>
    /// Администратор.
    /// </summary>
    Administrator = (byte)'A',

    /// <summary>
    /// Каталогизатор.
    /// </summary>
    Cataloger = (byte)'C',

    /// <summary>
    /// Комплектатор.
    /// </summary>
    Acquisitions = (byte)'M',

    /// <summary>
    /// Читатель.
    /// </summary>
    Reader = (byte)'R',

    /// <summary>
    /// Книговыдача.
    /// </summary>
    Circulation = (byte)'B',

    /// <summary>
    /// Тоже книговыдача.
    /// </summary>
    Bookland = (byte)'B',

    /// <summary>
    /// Книгообеспеченность.
    /// </summary>
    Provision = (byte)'K',

    /// <summary>
    /// Java апплет.
    /// </summary>
    JavaApplet = (byte)'J',

    /// <summary>
    /// Не задан.
    /// </summary>
    None = 0
}
