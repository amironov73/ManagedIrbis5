// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforBang.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Команда постредактуры: очистить результат расформатирования
    // от двойных разделителей – &uf('!')
    // Вид функции: !.
    // Назначение: Команда постредактуры: очистить результат
    // расформатирования от двойных разделителей (двойных точек
    // или двойных конструкций <. – >). Имеет смысл использовать
    // один раз в любом месте формата.
    // Присутствует в версиях ИРБИС с 2004.1.
    // Формат(передаваемая строка):
    // !
    //
    // Пример:
    //
    // &unifor('!')
    //

    static class UniforBang
    {
        #region Public methods

        /// <summary>
        /// Post processing: cleanup double text.
        /// </summary>
        public static void CleanDoubleText
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            context.GetRootContext().PostProcessing |= PftCleanup.DoubleText;
        }

        #endregion
    }
}
