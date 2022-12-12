// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EnumerableEx.cs -- полезные методы для работы с перечисляемыми коллекциями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Memory.Collections.Linq;

#endregion

#nullable enable

namespace AM.Memory.Collections;

/// <summary>
/// Полезные методы для работы с перечисляемыми коллекциями.
/// </summary>
public static class EnumerableEx
{
    #region Public methods

    /// <summary>
    /// Преобразование в пул-версию.
    /// </summary>
    public static IPoolingEnumerable<T> AsPooling<T>
        (
            this IEnumerable<T> source
        )
    {
        Sure.NotNull (source);

        return Pool<GenericPoolingEnumerable<T>>.Get().Init (source);
    }

    /// <summary>
    /// Преобразование в обычную версию.
    /// </summary>
    public static IEnumerable<T> AsEnumerable<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        Sure.NotNull (source);

        return Pool<GenericEnumerable<T>>.Get().Init (source);
    }

    #endregion
}
