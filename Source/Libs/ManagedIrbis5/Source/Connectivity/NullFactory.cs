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

namespace ManagedIrbis
{
    /// <summary>
    /// Фабрика, создающая нулевые подключения
    /// для целей тестирования.
    /// </summary>
    public sealed class NullFactory
        : ConnectionFactory
    {
        #region ConnectionFactory members

        /// <inheritdoc cref="ConnectionFactory.CreateConnection"/>
        public override IIrbisConnection CreateConnection() => new NullConnection();

        #endregion

    } // class NullFactory

} // namespace ManagedIrbis
