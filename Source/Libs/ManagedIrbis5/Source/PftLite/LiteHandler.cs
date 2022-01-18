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

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Обработчик языка форматирования ISIS для интерпретатора Барсик.
/// </summary>
public static class LiteHandler
{
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

        // TODO кешировать форматы

        var formatter = new LiteFormatter();
        try
        {
            formatter.SetFormat (format.Trim());
            var result = formatter.FormatRecord (record);
            context.Output.Write (result);

        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }
    }
}
