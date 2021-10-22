// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* IExemplarSink.cs -- интерфейс приемника экземпляров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Inventory
{
    /// <summary>
    /// Интерфейс приемника экземпляров.
    /// </summary>
    public interface IExemplarSink
        : IDisposable
    {
        /// <summary>
        /// Добавление экземпляров.
        /// </summary>
        void AddExemplars (IEnumerable<ExemplarInfo> exemplars);

    } // interface

} // namespace ManagedIrbis.Inventory
