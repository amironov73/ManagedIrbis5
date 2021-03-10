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

        /// <inheritdoc cref="ISyncConnection.Connect"/>
        public override bool Connect() => true;

        /// <inheritdoc cref="IAsyncConnection.ConnectAsync"/>
        public override Task<bool> ConnectAsync() => Task.FromResult(true);

        /// <inheritdoc cref="IAsyncConnection.ExecuteAsync"/>
        public override Task<Response?> ExecuteAsync(Query query) =>
            Task.FromResult<Response?>(null);

        /// <inheritdoc cref="ISyncConnection.ExecuteSync"/>
        public override Response ExecuteSync(ref ValueQuery query) => new();

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            // Nothing to do here
        }

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public override ValueTask DisposeAsync() => ValueTask.CompletedTask;

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public override object? GetService(Type serviceType) => null;

        #endregion

    } // class NullConnection

} // namespace ManagedIrbis
