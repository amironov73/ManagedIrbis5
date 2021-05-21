// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* NullFactory.cs -- фабрика, создающая нулевые подключения
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Providers
{
    // TODO: вернуть наследование от ConnectionFactory

    /// <summary>
    /// Фабрика, создающая нулевые подключения
    /// для целей тестирования.
    /// </summary>
    public sealed class NullFactory
        // : ConnectionFactory
    {
        #region ConnectionFactory members

        /// <summary>
        /// Создание нулевого подключения, работающего в синхронном режиме.
        /// </summary>
        public ISyncProvider CreateSyncConnection() =>
            new NullProvider();

        /// <summary>
        /// Создание нулевого подключения, работающего в асинхронном режиме.
        /// </summary>
        /// <returns></returns>
        public IAsyncProvider CreateAsyncConnection() =>
            new NullProvider();

        #endregion

    } // class NullFactory

} // namespace ManagedIrbis.Providers
