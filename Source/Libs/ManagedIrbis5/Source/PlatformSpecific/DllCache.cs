// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DllCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// DLL cache.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DllCache
    {
        #region Properties

        /// <summary>
        /// DLL registry.
        /// </summary>
        public static Dictionary<string, DynamicLibrary> DllRegistry { get; private set; }

        /// <summary>
        /// Delegate registry.
        /// </summary>
        public static Dictionary<Pair<string, string>, Delegate> DelegateRegistry
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        static DllCache()
        {
            DllRegistry = new Dictionary<string, DynamicLibrary>
                (
                    StringComparer.InvariantCultureIgnoreCase
                );
            DelegateRegistry = new Dictionary<Pair<string, string>, Delegate>();
            _sync = new object();
        }

        #endregion

        #region Private members

        private static object _sync;

        #endregion

        #region Public methods

        /// <summary>
        /// Free dynamic library.
        /// </summary>
        public static void FreeLibrary
            (
                string libraryName
            )
        {
            lock (_sync)
            {
                DynamicLibrary library;

                if (DllRegistry.TryGetValue(libraryName, out library))
                {
                    DllRegistry.Remove(libraryName);
                    library.Dispose();
                }
            }
        }

        /// <summary>
        /// Get delegate for given function from dynamic library.
        /// </summary>
        public static Delegate CreateDelegate
            (
                string libraryName,
                string functionName,
                Type type
            )
        {
            var library = LoadLibrary(libraryName);
            var key = new Pair<string, string>
                (
                    libraryName.ToUpperInvariant(),
                    functionName.ToUpperInvariant()
                );
            if (!DelegateRegistry.TryGetValue(key, out var result))
            {
                result = library.CreateDelegate
                    (
                        functionName,
                        type
                    );
                DelegateRegistry.Add(key, result);
            }

            return result;
        }

        /// <summary>
        /// Load dynamic library.
        /// </summary>
        public static DynamicLibrary LoadLibrary
            (
                string libraryName
            )
        {
            DynamicLibrary result;

            lock (_sync)
            {
                if (!DllRegistry.TryGetValue(libraryName, out result))
                {
                    result = new DynamicLibrary(libraryName);
                    DllRegistry.Add(libraryName, result);
                }
            }

            return result;
        }

        #endregion
    }
}
