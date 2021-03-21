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

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Раскодировка через справочник – &uf('K')
    // Вид функции: K.
    // Назначение: Возвращает значение из справочника,
    // соответствующее переданному коду
    // (иными словами, осуществляется раскодировка).
    // Формат (передаваемая строка):
    // K<имя_справочника><разделитель><исх_значение>
    // <разделитель> между<имя_справочника>
    // и <исх_значение> может быть двух видов:
    // \ - раскодировка с учетом регистра,
    // ! - раскодировка без учета регистра.
    //
    // Примеры:
    //
    // &unifor("Kjz.mnu\"v101)
    // &uf('kFIO_SF.MNU!'&uf('av907^b#1'))
    //
    // Вместо разделителя \ может использоваться
    // недокументированный разделитель |, он означает то же самое
    //

    static class UniforK
    {
        #region Public methods

        /// <summary>
        /// Get MNU-file entry.
        /// </summary>
        public static void GetMenuEntry
            (
                PftContext context,
                PftNode node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                string menuName = navigator.ReadUntil('\\', '!', '|').ToString();
                if (string.IsNullOrEmpty(menuName))
                {
                    return;
                }

                var separator = navigator.ReadChar();
                if (separator != '\\'
                    && separator != '!'
                    && separator != '|')
                {
                    return;
                }

                string key = navigator.GetRemainingText().ToString();
                if (string.IsNullOrEmpty(key))
                {
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
                if (ReferenceEquals(menu, null))
                {
                    Magna.Warning
                        (
                            "Missing menu file: " + specification
                        );
                }
                else
                {
                    string? output = null;

                    switch (separator)
                    {
                        case '\\':
                        case '|': // nondocumented but used in scripts
                            output = menu.GetStringSensitive(key);
                            break;

                        case '!':
                            output = menu.GetString(key);
                            break;
                    }

                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
