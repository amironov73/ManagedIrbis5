// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforEqual.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Форматный выход: Сравнить заданное значение с маской
    // (сравнение по маске)
    // &uf('=!<маска>!<значение>')
    // ! – уникальный символ двухстороннего ограничения
    // (может быть любым символом).
    // Маска может содержать принятые символы маскирования * и ?.
    // В общем случае маска может содержать несколько масок,
    // отделенных друг от друга символом вертикальной черты (|).
    // Форматный выход возвращает: 1 – в случае положительного
    // результата сравнения; 0 – в случае отрицательного.
    //
    // Examples
    //
    // ----------------------------------
    // | Mask      | Value     | Result |
    // | ----------|-----------|--------|
    // | *         |           | 1      |
    // | ?         |           | 0      |
    // | Hello     | Hello     | 1      |
    // | Hello     | hello     | 0      |
    // | Hello*    | Hello     | 1      |
    // | Hello?    | Hello     | 0      |
    // | Hel*      | Hello     | 1      |
    // | He??o     | Hello     | 1      |
    // | He??o     | Hell      | 0      |
    // ----------------------------------
    //

    static class UniforEqual
    {
        #region Public methods

        public static void CompareWithMask
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var navigator = new TextNavigator(expression);
            var delimiter = navigator.ReadChar();
            if (delimiter == TextNavigator.EOF)
            {
                return;
            }

            var maskSpecifiaction = navigator.ReadUntil(delimiter).ToString();
            if (ReferenceEquals(maskSpecifiaction, null)
                || navigator.ReadChar() == TextNavigator.EOF)
            {
                return;
            }

            var text = navigator.GetRemainingText().ToString();
            var mask = new PftMask(maskSpecifiaction);
            var result = mask.Match(text);
            var output = result ? "1" : "0";
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
