// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforK.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak неописанный unifor('+K')
    // Похож на unifor K, перебирает файл mnu.
    // В отличие от unifor K ищет по значениям элементов
    // максимально близкое значение и возвращает ключ.
    // В качестве разделителя может быть символ вертикальной черты | и обратный слэш \
    // Оба ведут себя одинаково.
    // Функция корректно работает только с упорядоченным списком.
    // В документе ftp://www.library.tomsk.ru/pub/IRBIS/unifor+.doc
    // утверждается, что unifor +K для авторского знака.
    // В принципе, возможно и так,
    // если подсунуть mnu с авторской таблицей, то будет работать.
    //

    static class UniforPlusK
    {
        #region Public methods

        /// <summary>
        /// Get Author Sign
        /// </summary>
        public static void GetAuthorSign
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                string menuName = navigator.ReadUntil('\\', '!').ToString();
                if (string.IsNullOrEmpty(menuName))
                {
                    return;
                }

                var separator = navigator.ReadChar();
                if (separator != '\\'
                    && separator != '!')
                {
                    return;
                }

                string text = navigator.GetRemainingText().ToString();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                // Немного магии от разработчиков ИРБИС
                if (text.StartsWith("Ы") || text.StartsWith("ы"))
                {
                    context.WriteAndSetFlag(node, "Ы");

                    return;
                }

                var specification = new FileSpecification
                    {
                        Path = IrbisPath.MasterFile,
                        Database = context.Provider.Database,
                        FileName = menuName
                    };
                var menu = context.Provider.ReadMenuFile
                    (
                        specification
                    );
                if (!ReferenceEquals(menu, null))
                {
                    // Алгоритм подсмотрен в раскомпилированном коде
                    // Код гуляет по пунктам меню,
                    // как только результат сравнения строки меньше нуля,
                    // остановка цикла.
                    // Сравнение без учета регистра
                    // Точнее в irbis64.dll делается приведение
                    // к верхнему регистру, но в C# можно сравнивать и так

                    string? output = null;
                    MenuEntry? entry = null;
                    var comparer = StringComparer.OrdinalIgnoreCase;
                    var entries = menu.Entries;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        if (comparer.Compare(text, entries[i].Comment) < 0)
                        {
                            break;
                        }

                        entry = entries[i];
                    }

                    if (!ReferenceEquals(entry, null))
                    {
                        output = entry.Code;
                    }

                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
