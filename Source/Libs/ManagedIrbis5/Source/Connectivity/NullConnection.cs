// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* NullConnection.cs -- пустой клиент для нужд тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Пустой клиент для нужд тестирования.
    /// Не выполняет никаких осмысленных действий.
    /// </summary>
    public sealed class NullConnection
        : ConnectionBase
    {
        #region ConnectionBase members

        /// <inheritdoc cref="IAsyncConnection.ExecuteAsync"/>
        public override Task<Response?> ExecuteAsync(Query query) =>
            Task.FromResult<Response?>(null);

        /// <inheritdoc cref="ISyncConnection.ExecuteSync"/>
        public override Response ExecuteSync(ref ValueQuery query) => new();

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            // Nothing to do here
        }

        #endregion

    } // class NullConnection

} // namespace ManagedIrbis
