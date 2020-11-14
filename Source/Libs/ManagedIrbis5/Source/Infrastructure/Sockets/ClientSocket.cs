// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ClientSocket.cs -- абстрактный клиентский сокет
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Абстрактный клиентский сокет.
    /// Занимается общением с сервером в самом широком смысле.
    /// Чаще всего - обычный BSD-сокет для TCP v4.
    /// </summary>
    public abstract class ClientSocket
    {
        #region Properties

        /// <summary>
        /// Используемое подключение (для нотификаций).
        /// </summary>
        protected internal Connection? Connection { get; internal set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Собственно общение с сервером -- в асинхронном режиме.
        /// </summary>
        public abstract Task<Response?> TransactAsync
            (
                Query query
            );

        #endregion
    }
}
