// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionBase.cs -- абстрактная база для подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Абстрактная база для подключения к серверу ИРБИС64.
    /// </summary>
    public abstract class ConnectionBase
        : IIrbisConnection
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public virtual string Host { get; set; } = "127.0.0.1";

        /// <summary>
        ///
        /// </summary>
        public virtual ushort Port { get; set; } = 6666;

        /// <summary>
        ///
        /// </summary>
        public virtual string Username { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public virtual string Password { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public virtual string Database { get; set; } = "IBIS";

        /// <summary>
        ///
        /// </summary>
        public virtual string Workstation { get; set; } = "C";

        /// <summary>
        ///
        /// </summary>
        public virtual int ClientId { get; protected internal set; }

        /// <summary>
        ///
        /// </summary>
        public virtual int QueryId { get; protected internal set; }

        /// <summary>
        ///
        /// </summary>
        public virtual bool Connected { get; protected internal set; } = false;

        /// <summary>
        /// Busy?
        /// </summary>
        public virtual bool Busy { get; protected internal set; } = false;

        /// <summary>
        /// Last error code.
        /// </summary>
        public virtual int LastError { get; protected internal set; } = 0;

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка, установлено ли соединение.
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckConnection()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;
        } // method CheckConnection

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public abstract Task<Response?> ExecuteAsync(Query query);

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public abstract Response? ExecuteSync(ref ValueQuery query);

        /// <summary>
        ///
        /// </summary>
        public abstract void Dispose();

        #endregion

    } // class ConnectionBase

} // namespace ManagedIrbis
