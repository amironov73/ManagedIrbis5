// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LiteHandler.cs -- обработчик языка форматирования ISIS
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Scripting.Barsik;

using ManagedIrbis.Scripting.Barsik;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Обработчик языка форматирования ISIS для интерпретатора Барсик.
/// </summary>
public static class LiteHandler
{
    #region Private members

    private static readonly IMemoryCache _cache = new MemoryCache (new MemoryCacheOptions());

    #endregion

    #region Public methods

    /// <summary>
    /// Собственно обработчик.
    /// </summary>
    public static void ExternalCodeHandler
        (
            Context context,
            ExternalNode node
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return;
        }

        var format = node.Code;
        if (string.IsNullOrWhiteSpace (format))
        {
            return;
        }

        try
        {
            if (!_cache.TryGetValue (format, out LiteFormatter formatter))
            {
                formatter = new LiteFormatter();
                formatter.SetFormat (format.Trim());
                _cache.Set (format, formatter, TimeSpan.FromMinutes (5));
            }

            var result = formatter.FormatRecord (record);
            context.Output.Write (result);

        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }
    }

    #endregion
}
