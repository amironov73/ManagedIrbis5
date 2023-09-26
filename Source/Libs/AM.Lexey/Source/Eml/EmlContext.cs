// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EmlContext.cs -- контекст исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Eml;

/// <summary>
/// Контекст исполнения скрипта
/// </summary>
[PublicAPI]
public sealed class EmlContext
{
    #region Properties

    /// <summary>
    /// Перечень пространств имен.
    /// </summary>
    public List<string> Namespaces { get; } = new ();

    #endregion
}
