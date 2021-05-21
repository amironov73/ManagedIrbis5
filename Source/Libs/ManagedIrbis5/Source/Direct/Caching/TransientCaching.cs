// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TransientCaching.cs -- создание кеша "на каждый чих"
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
    /// Создание кеша "на каждый чих".
    /// </summary>
    public sealed class TransientCaching
        : IContextCachingStrategy
    {
        #region Private members

        private readonly ConcurrentDictionary<string, string> _dictionary
            = new (StringComparer.OrdinalIgnoreCase);

        #endregion

        #region IContextCachingStrategy

        /// <inheritdoc cref="IContextCachingStrategy.ClearFileCache"/>
        public void ClearFileCache() => _dictionary.Clear();

        /// <inheritdoc cref="IContextCachingStrategy.GetCachedFile"/>
        public string? GetCachedFile (ISyncProvider provider, string fileName) =>
            _dictionary.TryGetValue(fileName, out var content) ? content : default;

        /// <inheritdoc cref="IContextCachingStrategy.StoreFile"/>
        public void StoreFile (string fileName, string content) =>
            _dictionary[fileName] = content;

        /// <inheritdoc cref="IContextCachingStrategy.ForgetFile"/>
        public void ForgetFile(string fileName) => _dictionary.TryRemove(fileName, out _);

        #endregion

    } // class TransientCaching

} // namespace ManagedIrbis.Direct
