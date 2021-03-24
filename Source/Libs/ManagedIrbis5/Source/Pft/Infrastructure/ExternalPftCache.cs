// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ExternalPftCache.cs -- дисковый кэш для скомпилированных PFT-скриптов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Дисковый кэш для скомпилированных PFT-скриптов
    /// </summary>
    public sealed class ExternalPftCache
    {
        #region Constants

        /// <summary>
        /// Serialized AST file extension.
        /// </summary>
        public const string Ast = ".ast";

        /// <summary>
        /// DLL file extension.
        /// </summary>
        public const string Dll = ".dll";

        #endregion

        #region Properties

        /// <summary>
        /// Root directory.
        /// </summary>
        public string RootDirectory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public ExternalPftCache()
        {
            RootDirectory = string.Empty; // to make compiler happy
            _hasp = new object();
            SetRootDirectory(GetDefaultRootDirectory());
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalPftCache
            (
                string rootDirectory
            )
        {
            RootDirectory = rootDirectory; // to make compiler happy
            _hasp = new object();
            SetRootDirectory(rootDirectory);
        } // constructor

        #endregion

        #region Private members

        private readonly object _hasp;

        #endregion

        #region Public methods

        /// <summary>
        /// Add DLL with compiled PFT.
        /// </summary>
        public void AddDll
            (
                string scriptText,
                byte[] image
            )
        {
            lock (_hasp)
            {
                var path = ComputePath(scriptText) + Dll;
                File.WriteAllBytes(path, image);
            }
        }

        /// <summary>
        /// Add serialized PFT.
        /// </summary>
        public void AddSerializedPft
            (
                string scriptText,
                byte[] image
            )
        {
            lock (_hasp)
            {
                var path = ComputePath(scriptText) + Ast;
                File.WriteAllBytes(path, image);
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            lock (_hasp)
            {
                var files = Directory.EnumerateFiles
                    (
                        RootDirectory,
                        "*.*",
                        SearchOption.AllDirectories
                    );

                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Compute file name for given script.
        /// </summary>
        public string ComputeFileName
            (
                string scriptText
            )
        {
            using var md5 = MD5.Create();
            var bytes = IrbisEncoding.Utf8.GetBytes(scriptText);
            var hash = md5.ComputeHash(bytes);
            var result = new StringBuilder(hash.Length * 2);
            foreach (var one in hash)
            {
                result.AppendFormat(one.ToString("X2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// Compute full file name for given script.
        /// </summary>
        public string ComputePath(string scriptText) => Path.Combine
            (
                RootDirectory,
                ComputeFileName(scriptText)
            );

        /// <summary>
        /// </summary>
        /// Get path of default cache root directory.
        public string GetDefaultRootDirectory() => Path.Combine
            (
                Path.Combine
                (
                    Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
                    "ManagedIrbis"
                ),
                "PftCache"
            );

        /// <summary>
        /// Get DLL for specified script.
        /// </summary>
        public Func<PftContext, PftPacket>? GetDll
            (
                string scriptText
            )
        {
            lock (_hasp)
            {
                var path = GetPath(scriptText, Dll);
                if (ReferenceEquals(path, null))
                {
                    return null;
                }

                // TODO choose the right method

                var assembly = Assembly.LoadFile(path);
                var result = CompilerUtility.GetEntryPoint(assembly);

                return result;

            }
        } // method GetDll

        /// <summary>
        /// Get supposed path for specified script.
        /// </summary>
        public string? GetPath
            (
                string scriptText,
                string extension
            )
        {
            lock (_hasp)
            {
                var result = ComputePath(scriptText) + extension;
                if (File.Exists(result))
                {
                    return result;
                }
            }

            return null;
        } // method GetPath

        /// <summary>
        /// Get serialized PFT for the script.
        /// </summary>
        public PftNode? GetSerializedPft
            (
                string scriptText
            )
        {
            lock (_hasp)
            {
                var path = GetPath(scriptText, Ast);
                if (ReferenceEquals(path, null))
                {
                    return null;
                }

                var result = PftSerializer.Read(path);
                return result;
            }
        }

        /// <summary>
        /// Set root directory.
        /// </summary>
        public void SetRootDirectory
            (
                string path
            )
        {
            lock (_hasp)
            {
                path = Path.GetFullPath(path);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                RootDirectory = path;
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => RootDirectory;

        #endregion

    } // class ExternalPftCache

} // namespace ManagedIrbis.Pft.Infrastructure
