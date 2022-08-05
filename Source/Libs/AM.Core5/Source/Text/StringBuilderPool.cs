// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StringBuilderPool.cs -- пул для StringBuilder
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Пул для <see cref="StringBuilder"/>.
/// </summary>
public static class StringBuilderPool
{
    #region Properties

    /// <summary>
    /// Общий экземпляр пула.
    /// </summary>
    public static ObjectPool<StringBuilder> Shared { get; } = CreateNew();

    #endregion

    #region Public methods

    /// <summary>
    /// Создание нового пула со стандартными настройками.
    /// </summary>
    public static ObjectPool<StringBuilder> CreateNew()
    {
        return new DefaultObjectPoolProvider().CreateStringBuilderPool();
    }

    /// <summary>
    /// Создание нового пула с нестандартными настройками.
    /// </summary>
    public static ObjectPool<StringBuilder> CreateNew
        (
            int initialCapacity,
            int maximumRetainedCapacity
        )
    {
        return new DefaultObjectPoolProvider().CreateStringBuilderPool
            (
                initialCapacity,
                maximumRetainedCapacity
            );
    }

    /// <summary>
    /// Возврат <see cref="StringBuilder"/> в общий пул.
    /// </summary>
    public static string ReturnShared
        (
            this StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        var result = builder.ToString();
        Shared.Return (builder);

        return result;
    }

    #endregion
}
