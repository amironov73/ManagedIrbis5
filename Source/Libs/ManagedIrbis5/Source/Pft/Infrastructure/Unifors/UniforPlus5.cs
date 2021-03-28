// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus5.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдача элемента списка/справочника в соответствии
    // с индексом (номером повторения) повторяющейся группы – &uf('+5')
    // Вид функции: +5.
    // Назначение: Выдача элемента списка/справочника в соответствии
    // с индексом (номером повторения) повторяющейся группы.
    // Присутствует в версиях ИРБИС с 2005.2.
    // Формат (передаваемая строка):
    // +5Х<имя_справочника/списка>
    // где Х принимает значения: Т – выдать значение;
    // F – выдать пояснение(имеет смысл, если задается справочник,
    // т. е. файл с расширением MNU).
    //
    // Пример:
    //
    // (&unifor('+5Tfield.mnu'),' – ',&unifor('+5Ffield.mnu'))
    //

    //
    // ibatrak
    // unifor +5 парсит *.MNU как меню,
    // а если встречается любой другой файл, выдает просто текстовую строку.
    // Пустая строка означает конец разбора.
    //

    static class UniforPlus5
    {
        #region Public methods

        public static void GetMenuEntry
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            /*

            // ibatrak
            // минимальная длина выражения команда + .mnu (имя файла - только расширение) = 5
            if (string.IsNullOrEmpty(expression) || expression.Length < 5)
            {
                return;
            }

            var navigator = new TextNavigator(expression);
            char command = char.ToUpperInvariant(navigator.ReadChar());
            if (command != 'T' && command != 'F')
            {
                Magna.Warning
                    (
                        "UniforPlus5::GetMenuEntry: "
                        + "unknown command="
                        + command
                    );

                return;
            }

            string fileName = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileSpecification specification;
            string? output = null;
            var index = context.Index;

            var extension = Path.GetExtension(fileName).ToUpperInvariant();
            if (extension != ".MNU")
            {
                specification = new FileSpecification
                    {
                        Path = IrbisPath.System,
                        FileName = fileName
                    };
                var text = context.Provider.ReadTextFile(specification);
                if (!string.IsNullOrEmpty(text))
                {
                    var lines = text.SplitLines();
                    for (var i = 0; i < lines.Length; i++)
                    {
                        var value = lines[i];
                        if (string.IsNullOrEmpty(value))
                        {
                            break;
                        }

                        if (i == index)
                        {
                            output = value;
                            break;
                        }
                    }
                }
            }
            else
            {
                // .MNU
                specification = new FileSpecification
                    {
                        Path = IrbisPath.MasterFile,
                        Database = context.Provider.Database,
                        FileName = fileName
                    };
                var menu = context.Provider.ReadMenuFile(specification);
                var entry = menu?.Entries.SafeAt(index);
                if (entry is null)
                {
                    return;
                }

                switch (command)
                {
                    case 'T':
                        output = entry.Code;
                        break;

                    case 'F':
                        output = entry.Comment;
                        break;
                }
            }

            context.WriteAndSetFlag(node, output);

            */
        }

        #endregion
    }
}
