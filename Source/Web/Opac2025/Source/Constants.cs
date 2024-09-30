// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Constants.cs -- константы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Константы.
/// </summary>
[PublicAPI]
internal static class Constants
{
    #region Constants

    /// <summary>
    /// Архивирован.
    /// </summary>
    public const string Archived = "archived";

    /// <summary>
    /// Свежесозданный заказ.
    /// </summary>
    public const string New = "new";

    /// <summary>
    /// Заказ готов к выдаче.
    /// </summary>
    public const string Ready = "ready";

    /// <summary>
    /// Заказ выполнен (выдан читателю).
    /// </summary>
    public const string Done = "done";

    /// <summary>
    /// Заказ отменен.
    /// </summary>
    public const string Cancelled = "cancelled";

    /// <summary>
    /// Ошибка.
    /// </summary>
    public const string Error = "error";

    #endregion
}
