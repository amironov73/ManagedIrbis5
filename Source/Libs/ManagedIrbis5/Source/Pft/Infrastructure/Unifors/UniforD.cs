// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforD.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Форматирование документа из другой БД – &uf('D')
    //
    // Назначение: Форматирование документа из другой БД
    // (REF на другую БД – отношение «от одного к одному»).
    //
    // Формат (передаваемая строка):
    //
    // D<dbn>,<@mfn|/termin/>,<@имя_формата|формат|*>
    //
    // Передаются три параметра, разделенные запятой:
    // Первый – имя БД;
    // Второй – или непосредственно MFN с предшествующим
    // символом @ или термин, ссылающийся на документ
    // (термин – заключается в ограничительные символы);
    // Третий – или имя формата с предшествующим символом
    // @ или непосредственно формат.
    // Если задается *, данные выводятся по прямой ссылке
    // (метка поля, номер повторения).
    //
    // Пример:
    //
    // &unifor('DBOOK,/K=AAA/,v200')
    //

    static class UniforD
    {
        #region Public methods

        public static void FormatDocumentDB
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            /*

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var navigator = new TextNavigator(expression);

            TermLink[] links = null;

            string database = navigator.ReadUntil(',').ToString();
            if (string.IsNullOrEmpty(database))
            {
                database = context.Provider.Database;
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            int[] found;
            if (navigator.PeekChar() == '@')
            {
                // явное указание MFN

                navigator.ReadChar();
                int mfn;
                string mfnText = navigator.ReadInteger().ToString();
                if (!Utility.TryParseInt32(mfnText, out mfn))
                {
                    return;
                }
                found = new[] { mfn };
            }
            else
            {
                var delimiter = navigator.ReadChar();
                string query = navigator.ReadUntil(delimiter).ToString();
                if (string.IsNullOrEmpty(query))
                {
                    return;
                }
                navigator.ReadChar();

                var saveDatabase = context.Provider.Database;
                try
                {
                    context.Provider.Database = database;

                    links = context.Provider.ExactSearchLinks(query);
                    found = TermLink.ToMfn(links);
                }
                finally
                {
                    context.Provider.Database = saveDatabase;
                }
            }

            if (found.Length == 0)
            {
                return;
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            string format = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            if (format == "*")
            {
                if (!ReferenceEquals(links, null) && links.Length != 0)
                {
                    PftUtility.FormatTermLink
                        (
                            context,
                            node,
                            database,
                            links[0]
                        );
                }
            }
            else
            {
                // ibatrak
                // После вызова этого unifor в главном контексте
                // сбрасываются флаги пост обработки
                context.GetRootContext().PostProcessing = PftCleanup.None;

                // TODO some caching

                var program = PftUtility.CompileProgram(format);

                using (var guard = new PftContextGuard(context))
                {
                    var nestedContext = guard.ChildContext;

                    // ibatrak
                    // формат вызывается в контексте без повторений
                    nestedContext.Reset();

                    var saveDatabase = nestedContext.Provider.Database;
                    try
                    {
                        nestedContext.Provider.Database = database;
                        nestedContext.Output = context.Output;
                        var mfn = found[0];
                        var record = nestedContext.Provider.ReadRecord(mfn);
                        if (!ReferenceEquals(record, null))
                        {
                            nestedContext.Record = record;
                            program.Execute(nestedContext);
                        }
                    }
                    finally
                    {
                        nestedContext.Provider.Database = saveDatabase;
                    }
                }
            }

            */
        }

        #endregion
    }
}
