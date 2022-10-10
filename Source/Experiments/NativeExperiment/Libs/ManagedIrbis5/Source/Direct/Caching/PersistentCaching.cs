// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PersistentCaching.cs -- постоянное кеширование
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Постоянное кеширование.
    /// </summary>
    public sealed class PersistentCaching
        : IContextCachingStrategy
    {
        #region Private members

        private static readonly ConcurrentDictionary<string, string> _dictionary =
            new (StringComparer.OrdinalIgnoreCase);

        #endregion

        #region IContextCachingStrategy

        /// <inheritdoc cref="IContextCachingStrategy.ClearFileCache"/>
        public void ClearFileCache() => _dictionary.Clear();

        /// <inheritdoc cref="IContextCachingStrategy.GetCachedFile"/>
        public string? GetCachedFile(ISyncProvider provider, string fileName) =>
            _dictionary.TryGetValue(fileName, out var content) ? content : default;

        /// <inheritdoc cref="IContextCachingStrategy.StoreFile"/>
        public void StoreFile (string fileName, string content) =>
            _dictionary[fileName] = content;

        /// <inheritdoc cref="IContextCachingStrategy.ForgetFile"/>
        public void ForgetFile(string fileName) => _dictionary.TryRemove(fileName, out _);

        #endregion

    } // class PersistentCaching

} // namespace ManagedIrbis.Direct
