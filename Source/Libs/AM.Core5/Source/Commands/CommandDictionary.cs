// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CommandDictionary.cs -- контейнер для команд
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Commands;

/// <summary>
/// Контейнер для команд.
/// </summary>
[PublicAPI]
public sealed class CommandDictionary
    : Dictionary<string, ICommand>,
        IDisposable
{
    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var command in Values)
        {
            command.Dispose();
        }
    }

    #endregion
}
