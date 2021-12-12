// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforDollar.cs -- MD5 заданной строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Security.Cryptography;

using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Создан новый форматный выход, который возвращает
    // хеш-сумму (MD5) заданной строки:
    //
    // &uf('$....)
    //

    /// <summary>
    /// MD5 заданной строки.
    /// </summary>
    static class UniforDollar
    {
        #region Public methods

        public static void Md5Hash
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            expression ??= string.Empty;

            var bytes = IrbisEncoding.Utf8.GetBytes (expression);
            var md5 = MD5.HashData (bytes);
            var builder = StringBuilderPool.Shared.Get();

            foreach (var one in md5)
            {
                builder.AppendFormat ("{0:X2}", one);
            }

            var output = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            context.WriteAndSetFlag (node, output);
        }

        #endregion
    }
}
