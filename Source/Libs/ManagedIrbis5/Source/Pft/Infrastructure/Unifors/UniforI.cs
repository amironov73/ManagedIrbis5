// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforI.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.IO;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть параметр из INI-файла - &uf('I')
    // Вид функции: I.
    // Назначение: Вернуть параметр из INI-файла.
    // Формат (передаваемая строка):
    // I<SECTION>,<PAR_NAME>,<DE-FAULT_VALUE>
    //
    // Пример:
    //
    // &unifor('IPRIVATE,NAME,NONAME')
    //

    static class UniforI
    {
        #region Public methods

        /// <summary>
        /// Get INI-file entry.
        /// </summary>
        public static void GetIniFileEntry
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split
                    (
                        CommonSeparators.Comma,
                        3
                    );

                if (parts.Length >= 2)
                {
                    var section = parts[0];
                    var parameter = parts[1];
                    var defaultValue = parts.Length > 2
                        ? parts[2]
                        : null;

                    if (!string.IsNullOrEmpty(section)
                        && !string.IsNullOrEmpty(parameter))
                    {
                        var iniFile = context.Provider.GetUserIniFile();
                        var result = iniFile.GetValue
                            (
                                section,
                                parameter,
                                defaultValue
                            );

                        context.WriteAndSetFlag(node, result);
                    }
                }
            }
        }

        #endregion
    }
}
