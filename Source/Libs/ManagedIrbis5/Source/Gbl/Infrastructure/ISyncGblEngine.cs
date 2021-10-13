// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ISyncGblEngine.cs -- синхронный интерфейс движка пакетной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Интерфейс движка пакетной корректировки записей.
    /// </summary>
    public interface ISyncGblEngine
        : IDisposable
    {
        /// <summary>
        /// Пакетная корректировка записей.
        /// </summary>
        GblResult CorrectRecords
            (
                GblContext context,
                IReadOnlyList<GblNode> program
            );

    } // interface ISyncGblEngine

} // namespace ManagedIrbis.Gbl.Infrastructure
