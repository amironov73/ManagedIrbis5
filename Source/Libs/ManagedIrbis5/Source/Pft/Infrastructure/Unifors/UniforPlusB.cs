﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusB.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+B').
    // Суммирует байты входной строки.
    //

    static class UniforPlusB
    {
        #region Public methods

        /// <summary>
        /// Sum of input string bytes.
        /// </summary>
        public static void ByteSum
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!ReferenceEquals(expression, null))
            {
                var bytes = IrbisEncoding.Utf8.GetBytes(expression);
                long sum = 0;
                unchecked
                {
                    for (var i = 0; i < bytes.Length; i++)
                    {
                        sum += bytes[i];
                    }
                }

                var output = sum.ToInvariantString();
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
