// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforH.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанный &unifor('H').
    // Извлекает текст в пределах угловых скобок.
    // Eсли скобки дублируются, unifor H выводит задвоенные строки
    // Логику восстановить даже не пытался
    //
    // Вот примеры
    // 123<+++>321<<....>>!!!        <+++><<...><...>
    // 123<+++>321<<...>>!!!         <+++><<..><...>
    //

    //
    // amironov73
    //
    // Похоже, unifor рассчитан на простейшие случаи вроде
    //
    // &uf('H', 'abc<def>ijk')
    //
    // &uf('H', 'abc<def>hik<lmn>opq')
    //
    // Более сложные сценарии он зажевывает
    //

    static class UniforH
    {
        #region Public methods

        /// <summary>
        /// Extract text within angle brackets (with brackets).
        /// </summary>
        public static void ExtractAngleBrackets
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                while (navigator.SkipTo('<'))
                {
                    string chunk = navigator.ReadTo('>').ToString();

                    if (chunk.LastChar() == '>')
                    {
                        context.WriteAndSetFlag(node, chunk);
                    }
                }
            }
        }

        #endregion
    }
}
