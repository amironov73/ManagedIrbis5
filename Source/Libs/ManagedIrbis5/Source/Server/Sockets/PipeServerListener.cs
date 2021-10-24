// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* PipeServerListener.cs -- серверный слушатель для System.IO.Pipes
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Серверный слушатель для System.IO.Pipes.
    /// </summary>
    public sealed class PipeServerListener
        : IAsyncServerListener
    {
        #region Properties

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Максимальное количество подключаемых клиентов.
        /// </summary>
        public int InstanceCount { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PipeServerListener
            (
                string name,
                int instanceCount,
                CancellationToken cancellationToken
            )
        {
            Sure.NotNullNorEmpty (name);
            Sure.Positive (instanceCount);

            Name = name;
            InstanceCount = instanceCount;
            _cancellationToken = cancellationToken;
            _cancellationToken.Register (_StopListener);

        } // constructor

        #endregion

        #region Private members

        private NamedPipeServerStream? _stream;
        private readonly CancellationToken _cancellationToken;

        private void _StopListener()
        {
            _stream?.Dispose();

        } // method _StopListener

        #endregion

        #region IAsyncServerListener members

        /// <inheritdoc cref="IAsyncServerListener.AcceptClientAsync"/>
        public async Task<IAsyncServerSocket?> AcceptClientAsync()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            Sure.NotNull (_stream);

            await _stream!.WaitForConnectionAsync (_cancellationToken);
            if (_cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var result = new PipeServerSocket (_stream, _cancellationToken);

            return result;

        } // method AcceptClientAsync

        /// <inheritdoc cref="IAsyncServerListener.GetLocalAddress"/>
        public string GetLocalAddress() => Name;

        /// <inheritdoc cref="IAsyncServerListener.StartAsync"/>
        public Task StartAsync()
        {
            _stream ??= new NamedPipeServerStream
                (
                    Name,
                    PipeDirection.InOut,
                    InstanceCount,
#pragma warning disable CA1416

                    // supported only on Windows
                    PipeTransmissionMode.Message,
#pragma warning restore CA1416
                    PipeOptions.Asynchronous
                );

            return Task.CompletedTask;

        } // method StartAsync

        /// <inheritdoc cref="IAsyncServerListener.StopAsync"/>
        public async Task StopAsync()
        {
            if (_stream is not null)
            {
                await _stream.DisposeAsync();
                _stream = null;
            }

        } // method StopAsync

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public async ValueTask DisposeAsync() => await StopAsync();

        #endregion

    } // class Tcp46erverListener

} // namespace ManagedIrbis.Server.Sockets
