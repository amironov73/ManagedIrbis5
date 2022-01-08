// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ScriptTransformer.cs -- при необходимости трансформирует скрипт
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting.Sharping
{
    /*
        Эвристика такова:

        1. Если текст скрипта не содержит строк вида "^<.+>$",
           значит, это либо полный скрипт (не нуждается в трансформации),
           либо в нем содержится только секция <format>.
           Если есть хоть одна такая строка, переходим к п. 4.

        2. Эти два случая (полный скрипт и <format>) различаем по наличию
           в тексте строки вида "class\s+\w+\s+:\s+ScriptContext". Если
           такая строка есть, то это полный скрипт - отдаем его "как есть".

        3. Если вышеописанной строки нет, добавляем обвязку до полного скрипта.

        4. Весь текст разбивается на секции, затем формируется скрипт такого вида:

           ```c#
           <содержимое секции using>
           ```

           sealed class ComposedScript : ScriptContext
           {
                public ComposedScript (ISyncProvider provider, TextWriter output)
                    : base (provider, output) {}

                // при наличии секции <after>
                public override void AfterAll()
                {
                    <содержимое секции after>
                }

                // при наличии секции <before>
                public override void BeforeAll()
                {
                    <сожержимое секции before>
                }

                // всегда
                public override void FormatRecord()
                {
                    <содержимое секции format>
                }
           }

           5. При наличии секции <references> перечисленные в ней
              ссылки на сборки передаются Roslyn.

     */

    /// <summary>
    /// При необходимости трансформирует скрипт
    /// из секционного представления в полноценный исходный код.
    /// </summary>
    public sealed class ScriptTransformer
    {
        #region Properties

        /// <summary>
        /// Директивы <c>using</c>.
        /// </summary>
        public List<string> Usings { get; } = new ()
        {
            // начальный (стандартный) набор директив

            "using System;",
            "using System.IO;",
            "using ManagedIrbis;",
            "using ManagedIrbis.Scripting;"
        };

        /// <summary>
        /// Дополнительные ссылки на сборки (не на NuGet-пакеты!).
        /// </summary>
        public List<string> References { get; } = new();

        #endregion

        #region Private members

        /// <summary>
        /// Проверка существования секций в скрипте.
        /// </summary>
        private static bool ContainsSections (string[] lines) =>
            lines.Any (line => Regex.IsMatch (line, "^<.+?>$", RegexOptions.Singleline));

        /// <summary>
        /// Извлечение строк секции с указанным именем.
        /// </summary>
        private static string[] ExtractSection
            (
                string[] lines,
                string sectionName
            )
        {
            var begin = $"<{sectionName}>"; // признак начала секции
            var end = $"</{sectionName}>"; // признак конца секции
            var result = new List<string>();
            using var enumerator = ((IEnumerable<string>) lines).GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Contains (begin))
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.Contains (end))
                        {
                            break;
                        }

                        result.Add (enumerator.Current);

                    } // while

                    break;

                } // if

            } // while

            return result.ToArray();

        } // method ExtractSection

        /// <summary>
        /// Добавление в результирующий скрипт секции.
        /// </summary>
        private static void AppendSection
            (
                StringBuilder builder,
                string[] lines
            )
        {
            foreach (var line in lines)
            {
                builder.AppendLine (line);
            }

            if (!lines.IsNullOrEmpty())
            {
                builder.AppendLine();
            }

        } // method AppendSection

        /// <summary>
        /// Добавление метода с указанным именем и набором строк.
        /// Если набор строк пуст, метод не добавляется.
        /// </summary>
        private static void AppendMethod
            (
                StringBuilder builder,
                string methodName,
                string[] lines
            )
        {
            if (!lines.IsNullOrEmpty())
            {
                builder.AppendLine ($"public override void {methodName}()");
                builder.AppendLine ("{");
                foreach (var line in lines)
                {
                    builder.AppendLine (line);
                }

                builder.AppendLine ("}");
                builder.AppendLine();
            }

        } // method AppendMethod

        /// <summary>
        /// Сборка скрипта из секций.
        /// </summary>
        private string BuildScript
            (
                string[] usings,
                string[] before,
                string[] after,
                string[] format
            )
        {
            var builder = new StringBuilder();

            AppendSection (builder, Usings.Union (usings).ToArray());
            builder.AppendLine ("sealed class ComposedScript : ScriptContext");
            builder.AppendLine ("{");
            builder.AppendLine ("public ComposedScript (ISyncProvider provider, TextWriter output)");
            builder.AppendLine (": base (provider, output) {}");
            builder.AppendLine ();
            AppendMethod (builder, "AfterAll", after);
            AppendMethod (builder, "BeforeAll", before);
            AppendMethod (builder, "FormatRecord", format);
            builder.AppendLine ("}");

            return builder.ToString();

        } // method BuildScript

        #endregion

        #region Public methods

        /// <summary>
        /// Трансформация скрипта в полную форму.
        /// Если скрипт уже в полной форме, ничего не происходит.
        /// </summary>
        public string TransformScript
            (
                string originalSource
            )
        {
            var lines = originalSource.SplitLines();

            if (ContainsSections (lines))
            {
                // это скрипт в формате секций, собираем его из них

                References.AddRange (ExtractSection (lines, "references"));
                var usings = ExtractSection (lines, "using");
                var before = ExtractSection (lines, "before");
                var after  = ExtractSection (lines, "after");
                var format = ExtractSection (lines, "format");

                return BuildScript (usings, before, after, format);
            }

            if (!Regex.IsMatch (originalSource, @"class\s+\w+\s*:\s*ScriptContext"))
            {
                // это упрощенный скрипт с единственной (неявной) секцией <format>

                var none = Array.Empty<string>();
                return BuildScript (Usings.ToArray(), before: none, after: none, format: lines);
            }

            // скрипт не требует трансформации
            return originalSource;

        } // method TransformScript

        #endregion

    } // class ScriptTransformer

} // namespace ManagedIrbis.Scripting
