// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CubeHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Reporting.Data;

internal static class CubeHelper
{
    public static CubeSourceBase? GetCubeSource
        (
            Dictionary dictionary,
            string complexName
        )
    {
        Sure.NotNull (dictionary);

        if (string.IsNullOrEmpty (complexName))
        {
            return null;
        }

        var names = complexName.Split ('.');
        return dictionary.FindByAlias (names[0]) as CubeSourceBase;
    }
}
