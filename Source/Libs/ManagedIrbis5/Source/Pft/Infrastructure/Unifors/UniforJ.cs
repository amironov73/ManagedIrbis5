// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforJ.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть кол-во ссылок для заданного термина – &uf('J')
    // Вид функции: J.
    // Назначение: Вернуть кол-во ссылок для заданного термина.
    // Формат (передаваемая строка):
    // J<dbn>,<термин>
    // <dbn> – имя БД; по умолчанию используется текущая.
    //
    // Пример:
    //
    // &unifor('JBOOK,',"A="v200^a)
    //

    static class UniforJ
    {
        #region Public methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="node"></param>
        /// <param name="expression"></param>
        public static void GetTermRecordCountDB
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

            var database = navigator.ReadUntil(',').ToString();
            if (string.IsNullOrEmpty(database))
            {
                database = context.Provider.Database.ThrowIfNull("context.Provider.Database");
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            var term = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(term))
            {
                return;
            }

            var saveDatabase = context.Provider.Database;
            try
            {
                context.Provider.Database = database;

                var found = context.Provider.Search(term);
                var output = found.Length.ToInvariantString();
                context.WriteAndSetFlag(node, output);
            }
            finally
            {
                context.Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
