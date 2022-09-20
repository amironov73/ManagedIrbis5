// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IrbisPath.cs -- путь к файлам на сервере ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis;

/// <summary>
/// Путь к файлам на сервере ИРБИС64.
/// </summary>
public enum IrbisPath
{
    /// <summary>
    /// Общесистемный путь.
    /// </summary>
    System = 0,

    /// <summary>
    /// Путь размещения сведений о базах данных сервера ИРБИС64.
    /// </summary>
    Data = 1,

    /// <summary>
    /// Путь на мастер-файл базы данных.
    /// </summary>
    MasterFile = 2,

    /// <summary>
    /// Путь на словарь базы данных (как правило, совпадает с
    /// путем на мастер-файл).
    /// </summary>
    InvertedFile = 3,

    /// <summary>
    /// Путь на параметрию базы данных.
    /// </summary>
    ParameterFile = 10,

    /// <summary>
    /// Путь к полным текстам (как правило, полные тексты
    /// вынесены в отдельную папку или даже на отдельный диск).
    /// </summary>
    FullText = 11,

    /// <summary>
    /// Внутренний ресурс.
    /// </summary>
    InternalResource = 12
}
