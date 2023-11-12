// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ObjectRegistry.cs -- реестр объектов для сопоставления с командами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Commands;

/// <summary>
/// Реестр объектов для сопоставления с командами.
/// </summary>
[PublicAPI]
public sealed class ObjectRegistry
    : Dictionary<string, object>
{
    // пустое тело класса
}
