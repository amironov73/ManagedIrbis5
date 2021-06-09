// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* LngUtility.cs -- методы для работы с лингвистическими файлами
 * Ars Magna project, http://arsmagna.ru
 * TODO use case-sensitive dictionary?
 */

#region Using directives

using System.IO;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Lng
{
    /// <summary>
    /// Методы для работы с лингвистическими файлами.
    /// </summary>
    public static class LngUtility
    {
        #region Public methods

        /// <summary>
        /// Load from the server.
        /// </summary>
        public static LngFile? ReadLngFile
            (
                this ISyncProvider provider,
                FileSpecification specification
            )
        {
            var content = provider.ReadTextFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            using var reader = new StringReader(content);
            var result = new LngFile();
            result.ParseText(reader);

            return result;
        } // method ReadLngFile

        /// <summary>
        /// Load from the server.
        /// </summary>
        public static async Task<LngFile?> ReadOptFileAsync
            (
                this IAsyncProvider provider,
                FileSpecification specification
            )
        {
            var content = await provider.ReadTextFileAsync(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            using var reader = new StringReader(content);
            var result = new LngFile();
            result.ParseText(reader);

            return result;
        } // method ReadLngFileAsync

        #endregion

    } // class LngUtility

} // namespace ManagedIrbis.Lng
