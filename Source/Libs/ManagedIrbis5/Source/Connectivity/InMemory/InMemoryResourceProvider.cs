// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryResourceProvider.cs -- провайдер ресурсов, расположенных в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// Провайдер ресурсов, расположенных в оперативной памяти.
    /// </summary>
    public sealed class InMemoryResourceProvider
        : IResourceProvider
    {
        #region Properties

        /// <summary>
        /// Провайдеру запрещено перезаписывать ресурсы?
        /// </summary>
        public bool ReadOnly { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InMemoryResourceProvider
            (
                bool readOnly = false
            )
        {
            ReadOnly = readOnly;
            _directories = new();
            _files = new();
        }

        #endregion

        #region Private members

        private readonly CaseInsensitiveDictionary<InMemoryResourceProvider> _directories;
        private readonly CaseInsensitiveDictionary<string> _files;

        internal (string fileName, InMemoryResourceProvider provider) FindFile
            (
                string path
            )
        {
            var current = this;
            var fileName = path;
            if (path.Contains(Path.DirectorySeparatorChar))
            {
                var parts = path.Split(Path.DirectorySeparatorChar);
                var subdirs = parts[..^1];
                fileName = parts[^1];
                foreach (var subdir in subdirs)
                {
                    if (!current._directories.TryGetValue(subdir, out var inner))
                    {
                        return default;
                    }

                    current = inner;
                }
            }

            return (fileName, current);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Восстановление состояния провайдера из файловой системы.
        /// </summary>
        public void RestoreFrom
            (
                string path
            )
        {
            _directories.Clear();
            _files.Clear();

            foreach (var fileName in Directory.EnumerateFiles(path))
            {
                var nameWithExtension = Path.GetFileName(fileName);
                var content = File.ReadAllText(fileName, IrbisEncoding.Ansi);
                _files.Add(nameWithExtension, content);
            }

            foreach (var subdir in Directory.EnumerateDirectories(path))
            {
                var nameWithExtension = Path.GetFileName(subdir);
                var subitem = new InMemoryResourceProvider(ReadOnly);
                _directories.Add(nameWithExtension, subitem);
                subitem.RestoreFrom(subdir);
            }
        }

        #endregion

        #region IResourceProvider members

        /// <inheritdoc cref="IResourceProvider.Dump"/>
        public void Dump
            (
                TextWriter output
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IResourceProvider.ListResources"/>
        public string[] ListResources
            (
                string path
            )
        {
            return _files.Values.ToArray();
        }

        /// <inheritdoc cref="IResourceProvider.ReadResource"/>
        public string? ReadResource
            (
                string fileName
            )
        {
            InMemoryResourceProvider provider;
            (fileName, provider) = FindFile(fileName);

            if (provider._files.TryGetValue(fileName, out var content))
            {
                return content;
            }

            return default;
        }

        /// <inheritdoc cref="IResourceProvider.ResourceExists"/>
        public bool ResourceExists
            (
                string fileName
            )
        {
            InMemoryResourceProvider provider;
            (fileName, provider) = FindFile(fileName);

            return provider._files.ContainsKey(fileName);
        }

        /// <inheritdoc cref="IResourceProvider.WriteResource"/>
        public bool WriteResource
            (
                string fileName,
                string? content
            )
        {
            if (ReadOnly)
            {
                return false;
            }

            InMemoryResourceProvider provider;
            (fileName, provider) = FindFile(fileName);
            if (content is null)
            {
                return provider._files.Remove(fileName);
            }

            provider._files[fileName] = content;

            return true;
        }

        #endregion

    } // class InMemoryResourceProvider

} // namespace ManagedIrbis.InMemory
