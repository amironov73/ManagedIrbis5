// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* RecflectionExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Updating.Internal.Extensions;

internal static class ReflectionExtensions
{
    public static async Task ExtractManifestResourceAsync (this Assembly assembly, string resourceName,
        string destFilePath)
    {
        var input =
            assembly.GetManifestResourceStream (resourceName) ??
            throw new MissingManifestResourceException ($"Could not find resource '{resourceName}'.");

        using var output = File.Create (destFilePath);
        await input.CopyToAsync (output);
    }
}
