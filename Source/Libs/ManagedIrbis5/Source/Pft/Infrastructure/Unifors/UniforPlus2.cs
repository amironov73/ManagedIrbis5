// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus2.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выполнить внешнее приложение – &uf('+2')
    // Вид функции: +2.
    // Назначение: Выполняет внешнее приложение.
    // Всё, что после +2 – параметры командной строки.
    // Формат (передаваемая строка):
    //
    // Пример:
    //
    // &unifor('+2cmd')
    //

    static class UniforPlus2
    {
        #region Public methods

        public static void System
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // TODO some Android support?

            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split(new[] {' ', '\t'}, 2);
                string fileName = parts[0];
                string arguments = parts.Length == 2
                    ? parts[1]
                    : string.Empty;

                ProcessStartInfo startInfo = new ProcessStartInfo
                    (
                        fileName,
                        arguments
                    )
                {
                    CreateNoWindow = false,
                    UseShellExecute = false
                };

                Process process = Process.Start(startInfo);
                if (!ReferenceEquals(process, null))
                {
                    process.Dispose();
                }
            }
        }

        #endregion
    }
}
