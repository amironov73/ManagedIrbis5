// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OfType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TR"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<TR> OfType<TR>
        (
            this IPoolingEnumerable source
        )
    {
        return source is IPoolingEnumerable<TR> res
            ? res
            : Pool<OfTypeExprEnumerable<TR>>.Get().Init (source);
    }
}
