// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MagnaApplicationEventArgs.cs -- аргументы для события для приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.AppServices;

/// <summary>
/// Аргументы для события приложения
/// </summary>
[Serializable]
public sealed class MagnaApplicationEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Код выхода.
    /// </summary>
    public int ExitCode { get; set; }

    #endregion
}
