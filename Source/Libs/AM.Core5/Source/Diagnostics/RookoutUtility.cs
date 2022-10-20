// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* RookoutUtility.cs -- полезные методы для Rookout
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Rook;

#endregion

#nullable enable

namespace AM.Diagnostics;

/// <summary>
/// Полезные методы для Rookout.
/// </summary>
public static class RookoutUtility
{
    #region Properties

    /// <summary>
    /// Опции.
    /// </summary>
    public static RookOptions? Options;

    #endregion

    #region Public methods

    /// <summary>
    /// Настройка Rookout.
    /// </summary>
    public static void SetupRookout()
    {
        if (Options is null)
        {
            Options = new RookOptions
            {
                token = "1925fce85905b3e5c39fdf4d0092068ebd7bcc3b322db0c7344cc6c2f8dc1c28",
                labels = new Dictionary<string, string> { { "env", "dev" } }
            };
            API.Start (Options);
        }
    }

    #endregion
}
