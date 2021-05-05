// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* CommandDictionary.cs -- контейнер для команд
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Commands
{
    /// <summary>
    /// Контейнер для команд.
    /// </summary>
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
        } // method Dispose

        #endregion

    } // class CommandDictionary

} // namespace AM.Commands
