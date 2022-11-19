// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ClientVersion.cs -- версия клиента (сборки, содержащей данный класс)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Версия клиента (сборки, содержащей данный класс).
/// </summary>
public static class ClientVersion
{
    #region Properties

    /// <summary>
    /// Собственно версия клиента.
    /// </summary>
    public static readonly Version Version
        = typeof (ClientVersion)
            .Assembly
            .GetName()
            .Version
            ?? new Version();

    #endregion
}
