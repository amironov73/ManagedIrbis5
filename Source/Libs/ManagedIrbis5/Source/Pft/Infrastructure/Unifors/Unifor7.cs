// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Unifor7.cs --
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
    // Расформатирование группы связанных документов из другой БД
    // Вид функции: 7.
    // Назначение: Расформатирование группы связанных документов
    // из другой БД(отношение «от одного к многим»).
    // Функция обеспечивает возможность связать запись
    // с рядом других записей по какому бы то ни было общему признаку.
    // К примеру, можно отобрать все записи с определенным заглавие,
    // индексом УДК/ББК, ключевым словом.
    // Присутствует в версиях ИРБИС с 2004.1.
    // Формат (передаваемая строка):
    // 7<имя_БД>,</termin/>,<@имя_формата|формат|*>
    // где:
    // имя_БД – имя базы данных, из которой будут браться
    // связанные документы; по умолчанию используется текущая БД.
    // /termin/ – ключевой термин, на основе которого отбираются
    // связанные документы; термин заключается в уникальные
    // ограничители (например. /), в качестве которых используется
    // символ, не входящий(гарантированно) в термин.
    // @имя_формата|формат|* – имя формата или формат в явном виде,
    // в соответствии с которым будут расформатироваться связанные
    // документы. Если задается имя формата, то он берется
    // из директории БД, заданной параметром <имя_БД>.
    // Если задается *, данные выводятся по прямой ссылке
    // (метка поля, номер повторения).
    // Примеры:
    // &unifor('7TEST,',"/T="v200^a"/",',v903"\par "')
    //
    // &uf(|7EK,!FAK= 23.01!,&uf('av907^A#1'),&uf('6brief')/|d90),
    //

    static class Unifor7
    {
        #region Private members

        private static TermLink[] ExtractLinks
            (
                ISyncIrbisProvider provider,
                string term
            )
        {
            /*

            TermLink[] result;

            if (term.EndsWith("$"))
            {
                var start = term.Substring(0, term.Length - 1);
                result = provider.ExactSearchTrimLinks(start, 100);
            }
            else
            {
                result = provider.ExactSearchLinks(term);
            }

            return result;

            */

            throw new NotImplementedException();
        }

        #endregion

        #region Public methods

        public static void FormatDocuments
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
            if (navigator.ReadChar() == TextNavigator.EOF)
            {
                return;
            }
            var provider = context.Provider;
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database.ThrowIfNull("provider.Database");
            }

            var delimiter = navigator.ReadChar();
            if (delimiter == TextNavigator.EOF)
            {
                return;
            }
            var term = navigator.ReadUntil(delimiter).ToString();
            if (string.IsNullOrEmpty(term))
            {
                return;
            }
            if (navigator.ReadChar() != delimiter
                || navigator.ReadChar() != ',')
            {
                return;
            }
            var format = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            // ibatrak
            // После вызова этого unifor в главном контексте
            // сбрасываются флаги постобработки
            context.GetRootContext().PostProcessing = PftCleanup.None;

            if (format == "*")
            {
                provider.Database = database;
                var links = ExtractLinks(provider, term);
                foreach (var link in links)
                {
                    if (PftUtility.FormatTermLink
                        (
                            context,
                            node,
                            database,
                            link
                        ))
                    {
                        context.WriteLine(node);
                    }
                }

                return;
            }

            var previousDatabase = provider.Database;
            try
            {
                provider.Database = database;
                var links = ExtractLinks(provider, term);
                var found = TermLink.ToMfn(links);
                if (found.Length != 0)
                {
                    // TODO some caching

                    var program = PftUtility.CompileProgram(format);

                    using (var guard = new PftContextGuard(context))
                    {
                        var nestedContext = guard.ChildContext;

                        // ibatrak
                        // формат вызывается в контексте без повторений
                        nestedContext.Reset();

                        nestedContext.Output = context.Output;
                        foreach (var mfn in found)
                        {
                            var record = nestedContext.Provider.ReadRecord(mfn);
                            if (!ReferenceEquals(record, null))
                            {
                                nestedContext.Record = record;
                                program.Execute(nestedContext);
                            }
                        }
                    }
                }
            }
            finally
            {
                provider.Database = previousDatabase;
            }
        }

        #endregion
    }
}
