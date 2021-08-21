// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* TransientDirectAccess64.cs -- создание акцессора "на каждый чих"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Создание акцессора "на каждый чих".
    /// </summary>
    public sealed class TransientDirectAccess64
        : IDirectAccess64Strategy
    {
        #region Private members

        private readonly List<Pair<DirectProvider, DirectAccess64>> _active = new();

        #endregion

        #region IDirectAccess64Strategy members

        /// <inheritdoc cref="IDirectAccess64Strategy.CreateAccessor"/>
        public DirectAccessProxy64 CreateAccessor
            (
                DirectProvider provider,
                string? databaseName,
                IServiceProvider? serviceProvider
            )
        {
            var result = DirectUtility.CreateAccessor(provider, databaseName, serviceProvider);
            var pair = new Pair<DirectProvider, DirectAccess64>(provider, result);
            _active.Add(pair);

            return new DirectAccessProxy64(this, provider, result);

        } // method CreateAccessor

        /// <inheritdoc cref="IDirectAccess64Strategy.ReleaseAccessor"/>
        public void ReleaseAccessor
            (
                DirectProvider? provider,
                DirectAccess64 accessor
            )
        {
            var found = _active.FirstOrDefault
                (
                    pair => ReferenceEquals(pair.Second, accessor)
                );

            if (found is not null)
            {
                _active.Remove(found);
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            foreach (var pair in _active)
            {
                var accessor = pair.Second;
                accessor!.Dispose();
            }

            _active.Clear();

        } // method Dispose

        #endregion

    } // class TransientDirectAccess64

} // namespace ManagedIrbis.Direct
