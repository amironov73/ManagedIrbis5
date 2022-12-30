// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.IO.Compression;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Resource loader class.
    /// </summary>
    public static partial class ResourceLoader
    {
        /// <summary>
        /// Gets a stream from specified assembly resource.
        /// </summary>
        /// <param name="assembly">Assembly name.</param>
        /// <param name="resource">Resource name.</param>
        /// <returns>Stream object.</returns>
        public static Stream GetStream (string assembly, string resource)
        {
            var assembly_full_name = assembly;
#if MONO
	  assembly_full_name += ".Mono";
#endif
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var name = new AssemblyName (a.FullName);
                if (name.Name == assembly_full_name)
                {
                    return a.GetManifestResourceStream (assembly + ".Resources." + resource);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a stream from AM.Reporting assembly resource.
        /// </summary>
        /// <param name="resource">Resource name.</param>
        /// <returns>Stream object.</returns>
        public static Stream GetStream (string resource)
        {
            return GetStream ("AM.Reporting", resource);
        }

        /// <summary>
        /// Gets a stream from specified assembly resource and unpacks it.
        /// </summary>
        /// <param name="assembly">Assembly name.</param>
        /// <param name="resource">Resource name.</param>
        /// <returns>Stream object.</returns>
        public static Stream UnpackStream (string assembly, string resource)
        {
            using (var packedStream = GetStream (assembly, resource))
            using (Stream gzipStream = new GZipStream (packedStream, CompressionMode.Decompress, true))
            {
                var result = new MemoryStream();

                const int BUFFER_SIZE = 4096;
                gzipStream.CopyTo (result, BUFFER_SIZE);

                result.Position = 0;
                return result;
            }
        }

        /// <summary>
        /// Gets a stream from specified AM.Reporting assembly resource and unpacks it.
        /// </summary>
        /// <param name="resource">Resource name.</param>
        /// <returns>Stream object.</returns>
        public static Stream UnpackStream (string resource)
        {
            return UnpackStream ("AM.Reporting", resource);
        }
    }
}
