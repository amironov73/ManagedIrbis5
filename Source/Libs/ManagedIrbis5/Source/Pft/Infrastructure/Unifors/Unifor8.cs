// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Unifor8.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    // Неописанный unifor 8
    // Формат (передаваемая строка):
    //
    // 8<dbn>,<@mfn|/termin/>,<fst>,<tag>,<teq>
    //
    // Передаются пять параметров, разделенные запятой:
    // Первый – имя БД;
    // Второй – или непосредственно MFN с предшествующим
    // символом @ или термин, ссылающийся на документ
    // (термин – заключается в ограничительные символы);
    // Третий – имя FST (IFS не поддерживается)
    // Четвертый - тег из FST
    // Пятый метод индексирования
    // Читает FST, ищет строки с указанным тегом и методом,
    // расформатирует найденную запись
    //

    static class Unifor8
    {
        #region Public methods

        public static void FormatWithFst
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

            var provider = context.Provider;
            string[] parts = expression.Split
                (
                    CommonSeparators.Comma,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            var database = parts[0];
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            if (string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            var mfn = 0;
            string? query = null;
            if (parts[1].StartsWith("@"))
            {
                parts = parts[1].Split
                    (
                        CommonSeparators.Comma,
                        4
                    );
                var mfnText = parts[0].Substring(1);
                if (!Utility.TryParseInt32(mfnText, out mfn)
                    || mfn <= 0)
                {
                    return;
                }
            }
            else
            {
                var separator = parts[1].Substring(0, 1);
                if (string.IsNullOrEmpty(separator))
                {
                    return;
                }
                var index = parts[1].IndexOf
                    (
                        separator,
                        1,
                        StringComparison.InvariantCulture
                    );
                if (index < 0)
                {
                    return;
                }

                query = parts[1].Substring(1, index - 1); //-V3057
                parts = parts[1].Substring(index + 1).Split
                    (
                        CommonSeparators.Comma,
                        4
                    );
            }

            if (parts.Length != 4)
            {
                return;
            }

            var fstName = parts[1];
            // если FST не задана, берем имя БД
            if (string.IsNullOrEmpty(fstName))
            {
                fstName = provider.Database;
            }
            var tagStr = parts[2];
            var methodStr = parts[3];
            // тег может быть пустым, не числом или 0, значит по тегу не фильтровать
            var tag = tagStr.SafeToInt32();
            int method;

            // метод может быть 0
            if (!Utility.TryParseInt32(methodStr, out method)
                || method < 0)
            {
                return;
            }

            var saveDatabase = provider.Database;
            provider.Database = database;

            try
            {
                if (mfn == 0)
                {
                    var parameters = new TermParameters
                    {
                        StartTerm = query.TrimEnd('$'),
                        NumberOfTerms = 1
                    };
                    Term[] terms = provider.ReadTerms(parameters);
                    if (terms.Length == 0)
                    {
                        return;
                    }

                    var postings = provider.ExactSearchLinks(terms[0].Text);
                    if (postings.Length == 0)
                    {
                        return;
                    }

                    mfn = postings[0].Mfn;
                }

                if (!fstName.Contains("."))
                {
                    fstName += ".FST";
                }

                var record = provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    return;
                }

                var specification = new FileSpecification
                {
                    Database = database,
                    Path = IrbisPath.InternalResource,
                    FileName = fstName
                };
                var fstContent = provider.ReadFile(specification);
                if (string.IsNullOrEmpty(fstContent))
                {
                    return;
                }

                // разбор FST напрямую без чтения вложенных файлов и поддержки формата IFS
                var lines = fstContent.SplitLines();
                var formatLines = new List<string>();
                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    parts = line.TrimStart().Split
                        (
                            CommonSeparators.SpaceOrTab,
                            3
                        );
                    if (parts.Length != 3)
                    {
                        continue;
                    }

                    // ищем строки, соответствующие указанным тегу и технике индексирования
                    if (!Utility.TryParseInt32(parts[0], out var lineTag)
                        || !Utility.TryParseInt32(parts[1], out var lineMethod))
                    {
                        continue;
                    }

                    if (tag > 0 && lineTag != tag || lineMethod != method)
                    {
                        continue;
                    }
                    formatLines.Add(parts[2]);
                }

                if (formatLines.Count == 0)
                {
                    return;
                }

                // после вызова этого unifor в главном контексте сбрасываются флаги постобработки
                context.GetRootContext().PostProcessing = PftCleanup.None;

                var builder = new StringBuilder();
                var seen = new List<string>();
                for (var i = 0; i < formatLines.Count; i++)
                {
                    using (var guard = new PftContextGuard(context))
                    {
                        // формат вызывается в контексте без повторений
                        // делаем аналогично RepGroup
                        // создаем копию контекста со ссылкой на тот же буфер
                        // в копии сбрасываем состояние повторяющейся группы и работаем через него
                        // текстовый буфер восстанавливаем, так как он один и тот же
                        var nestedContext = guard.ChildContext;
                        nestedContext.Record = record;
                        nestedContext.Reset();
                        var format = formatLines[i];
                        var program = PftUtility.CompileProgram(format);
                        program.Execute(nestedContext);
                        var formatted = nestedContext.Text;
                        formatted = formatted.Trim(CommonSeparators.NewLineAndPercent);
                        string[] subLines = formatted.Split
                            (
                                CommonSeparators.NewLineAndPercent,
                                StringSplitOptions.RemoveEmptyEntries
                            );
                        foreach (var subLine in subLines)
                        {
                            if (seen.Contains(subLine))
                            {
                                continue;
                            }

                            builder.AppendLine();
                            seen.Add(subLine);
                            builder.Append(subLine);
                        }
                    }
                }

                context.WriteAndSetFlag(node, builder.ToString());

            }
            finally
            {
                context.Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
