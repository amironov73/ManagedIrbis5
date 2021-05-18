// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* DirectAccessProxy64.cs -- прокси для отслеживания "убивания" акцессора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Прокси для отслеживания "убивания" ацессора в соответствии
    /// со  стратегией.
    /// </summary>
    public sealed class DirectAccessProxy64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Собственно акцессор.
        /// </summary>
        public DirectAccess64 Accessor => _accessor;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DirectAccessProxy64
            (
                IDirectAccess64Strategy strategy,
                DirectProvider provider,
                DirectAccess64 accessor
            )
        {
            _strategy = strategy;
            _provider = provider;
            _accessor = accessor;
        }

        #endregion

        #region Private members

        private readonly IDirectAccess64Strategy _strategy;
        private readonly DirectProvider _provider;
        private readonly DirectAccess64 _accessor;

        #endregion

        #region Public methods

        /// <summary>
        /// Неявное преобразование к акцессору.
        /// </summary>
        public static implicit operator DirectAccess64(DirectAccessProxy64 proxy) => proxy._accessor;

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => _strategy.ReleaseAccessor(_provider, _accessor);

        #endregion

    } // class DirectAccessProxy64

} // namespace ManagedIrbis.Direct
