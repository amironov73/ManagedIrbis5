// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* UniforPlusD.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать имя базы данных – &uf('+D')
    // Вид функции: +D.
    // Назначение: Возвращает имя текущей базы данных.
    // Формат (передаваемая строка):
    // +D
    //
    // Примеры:
    //
    // &unifor('+D')
    //

    static class UniforPlusD
    {
        #region Public methods

        /// <summary>
        /// Get current database name.
        /// </summary>
        public static void GetDatabaseName
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var output = context.Provider.Database;
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
