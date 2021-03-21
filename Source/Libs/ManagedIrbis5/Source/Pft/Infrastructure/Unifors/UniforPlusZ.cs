// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusZ.cs --
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
    // ibatrak неописанный unifor('+Z')
    // Преобразование символов и кодовой страницы ANSI в OEM и обратно
    //

    /// <summary>
    ///
    /// </summary>
    static class UniforPlusZ
    {
        public static void AnsiToOem
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // ibatrak
            // если первый символ 0 вызывает CharToOemA,
            // если первый символ 1 вызывает OemToCharA

            if (!ReferenceEquals(expression, null)
                && expression.Length > 1)
            {
                var command = expression[0];
                var text = expression.Substring(1);

                switch (command)
                {
                    case '0':
                        text = Utility.ChangeEncoding
                            (
                                IrbisEncoding.Ansi,
                                IrbisEncoding.Oem,
                                text
                            );
                        break;

                    case '1':
                        text = Utility.ChangeEncoding
                            (
                                IrbisEncoding.Oem,
                                IrbisEncoding.Ansi,
                                text
                            );
                        break;
                }

                context.WriteAndSetFlag(node, text);
            }
        }
    }
}
