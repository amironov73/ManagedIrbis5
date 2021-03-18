﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExternalPftCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using AM.IO;
using AM.Reflection;



using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;


#endregion

// ReSharper disable InconsistentNaming

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>

    public sealed class ExternalPftCache
    {
        #region Constants

        /// <summary>
        /// Serialized AST file extension.
        /// </summary>
        public const string AST = ".ast";

        /// <summary>
        /// DLL file extension.
        /// </summary>
        public const string DLL = ".dll";

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
            _hasp = new object();
            SetRootDirectory(GetDefaultRootDirectory());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalPftCache
            (
                string rootDirectory
            )
        {
            Code.NotNullNorEmpty(rootDirectory, "rootDirectory");

            _hasp = new object();
            SetRootDirectory(rootDirectory);
        }

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
            Code.NotNull(scriptText, "scriptText");
            Code.NotNull(image, "image");

            lock (_hasp)
            {
                string path = ComputePath(scriptText) + DLL;
                FileUtility.WriteAllBytes(path, image);
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
            Code.NotNull(scriptText, "scriptText");
            Code.NotNull(image, "image");

            lock (_hasp)
            {
                string path = ComputePath(scriptText) + AST;
                FileUtility.WriteAllBytes(path, image);
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            lock (_hasp)
            {
                string[] files = Directory.GetFiles
                    (
                        RootDirectory,
                        "*.*"
#if !WINMOBILE && !PocketPC
                        , SearchOption.AllDirectories
#endif
                    );

                foreach (string file in files)
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
            Code.NotNullNorEmpty(scriptText, "scriptText");

            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = IrbisEncoding.Utf8.GetBytes(scriptText);
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder result = new StringBuilder(hash.Length * 2);
                foreach (byte one in hash)
                {
                    result.AppendFormat(one.ToString("X2"));
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Compute full file name for given script.
        /// </summary>
        public string ComputePath
            (
                string scriptText
            )
        {
            Code.NotNull(scriptText, scriptText);

            string fileName = ComputeFileName(scriptText);
            string result = Path.Combine
                (
                    RootDirectory,
                    fileName
                );

            return result;
        }

        /// <summary>
        /// </summary>
        /// Get path of default cache root directory.
        public string GetDefaultRootDirectory()
        {
#if UAP

            // TODO Implement properly

            return ".";

#else

            string result = Path.Combine
                (
                    Path.Combine
                    (
                        Environment.GetFolderPath
                            (
                                Environment.SpecialFolder.ApplicationData
                            ),
                        "ManagedIrbis"
                    ),
                    "PftCache"
                );

            return result;

#endif
        }

        /// <summary>
        /// Get DLL for specified script.
        /// </summary>
        [CanBeNull]
        public Func<PftContext, PftPacket> GetDll
            (
                string scriptText
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");

#if CLASSIC || NETCORE || ANDROID

            lock (_hasp)
            {
                string path = GetPath(scriptText, DLL);
                if (ReferenceEquals(path, null))
                {
                    return null;
                }

                // TODO choose the right method

                Assembly assembly = AssemblyUtility.LoadFile(path);
                //Assembly assembly = Assembly.LoadFrom(path);

                Func<PftContext, PftPacket> result =
                    CompilerUtility.GetEntryPoint(assembly);

                return result;

            }

#else

                return null;

#endif
        }

        /// <summary>
        /// Get supposed path for specified script.
        /// </summary>
        [CanBeNull]
        public string GetPath
            (
                string scriptText,
                string extension
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");
            Code.NotNull(extension, "extension");

            lock (_hasp)
            {
                string result = ComputePath(scriptText) + extension;
                if (File.Exists(result))
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Get serialized PFT for the script.
        /// </summary>
        [CanBeNull]
        public PftNode GetSerializedPft
            (
                string scriptText
            )
        {
            Code.NotNullNorEmpty(scriptText, "scriptText");

            lock (_hasp)
            {
                string path = GetPath(scriptText, AST);
                if (ReferenceEquals(path, null))
                {
                    return null;
                }

                PftNode result = PftSerializer.Read(path);
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
            Code.NotNullNorEmpty(path, "path");

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
        public override string ToString()
        {
            return RootDirectory;
        }

        #endregion
    }
}
