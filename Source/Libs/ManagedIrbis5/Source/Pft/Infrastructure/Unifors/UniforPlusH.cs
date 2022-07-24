// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusH.cs -- неописанная функция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors;

//
// ibatrak
//
// Неописанная функция unifor('+H')
// Очень странная функция.
// Перебирает строку как массив однобайтовых символов.
// Выкидывает каждый четвертый, в начало строки помещает
// количество таких групп.
//

internal static class UniforPlusH
{
    #region Public methods

    /// <summary>
    /// Take every 3 of four bytes
    /// </summary>
    public static void Take3Of4
        (
            PftContext context,
            PftNode? node,
            string? expression
        )
    {
        Sure.NotNull (context);

        if (string.IsNullOrEmpty (expression))
        {
            return;
        }

        var encoding = IrbisEncoding.Utf8;
        var bytes = encoding.GetBytes (expression);
        var list = new List<byte>();
        var length = bytes.Length < 3
            ? 0
            : unchecked ((bytes.Length + 3) / 4);
        list.Add ((byte)('0' + length));
        for (var i = 0; i < bytes.Length; i++)
        {
            if (i % 4 != 3)
            {
                list.Add (bytes[i]);
            }
        }

        try
        {
            var bytes2 = list.ToArray();
            var output = encoding.GetString (bytes2, 0, bytes2.Length);
            context.WriteAndSetFlag (node, output);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (UniforPlusH) + "::" + nameof (Take3Of4)
                );
        }
    }

    #endregion
}
