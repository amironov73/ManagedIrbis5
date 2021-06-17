// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncNestedSocket.cs -- синхронный сокет с вложенным сокетом
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Синхронный сокет с вложенным сокетом.
    /// </summary>
    public abstract class SyncNestedSocket
        : ISyncClientSocket
    {
        #region Properties

        /// <inheritdoc cref="ISyncClientSocket.RetryCount"/>
        public int RetryCount
        {
            get => InnerSocket.RetryCount;
            set => InnerSocket.RetryCount = value;
        }

        /// <inheritdoc cref="ISyncClientSocket.RetryDelay"/>
        public int RetryDelay
        {
            get => InnerSocket.RetryDelay;
            set => InnerSocket.RetryDelay = value;
        }

        /// <summary>
        /// Внутренний сокет, который выполняет все операции.
        /// </summary>
        public ISyncClientSocket InnerSocket { get; }

        /// <inheritdoc cref="ISyncClientSocket.Connection"/>
        public ISyncConnection? Connection
        {
            get => InnerSocket.Connection;
            set => InnerSocket.Connection = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected SyncNestedSocket
            (
                ISyncClientSocket innerSocket
            )
        {
            InnerSocket = innerSocket;

        } // constructor

        #endregion

        #region ISyncClientSocket members

        /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
        public virtual Response? TransactSync(SyncQuery query) => InnerSocket.TransactSync(query);

        #endregion

    } // class SyncNestedSocket

} // namespace ManagedIrbis.Infrastructure.Sockets
