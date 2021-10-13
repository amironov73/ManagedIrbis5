// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* IAsyncGblEngine.cs -- асинхронный интерфейс движка пакетной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Асинхронный интерфейс движка пакетной корректировки.
    /// </summary>
    public interface IAsyncGblEngine
        : IAsyncDisposable
    {

        /// <summary>
        /// Пакетная корректировка записей.
        /// </summary>
        Task<GblResult> CorrectRecordsAsync
            (
                GblContext context,
                IReadOnlyList<GblNode> program
            );

    } // interface IAsyncGblEngine

} // namespace ManagedIrbis.Gbl.Infrastructure
