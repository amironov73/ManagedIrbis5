// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PrefixLength.cs -- форма записи имени класса
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Runtime;

/// <summary>
/// Форма записи имени класса при сериализации.
/// </summary>
public enum PrefixLength
{
    /// <summary>
    /// Только имя класса.
    /// </summary>
    Short,

    /// <summary>
    /// Имя класса с пространством имен.
    /// </summary>
    Moderate,

    /// <summary>
    /// Полное имя, включая сборку и пространство имен.
    /// </summary>
    Full
}
