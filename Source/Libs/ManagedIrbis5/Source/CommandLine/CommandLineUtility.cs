// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CommandLineUtility.cs -- утилиты для работы с командной строкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.CommandLine;

#endregion

#nullable enable

namespace ManagedIrbis.CommandLine
{
    /// <summary>
    /// Утилиты для работы с командной строкой,
    /// специфичные для ИРБИС.
    /// </summary>
    public static class CommandLineUtility
    {
        #region Public methods

        /// <summary>
        /// Настройки для подключения к серверу.
        /// </summary>
        public static RootCommand GetRootCommand()
        {
            var result = new RootCommand
            {
                new Option<string>
                    (
                        "--host",
                        "host address"
                    ),

                new Option<int>
                    (
                        "--port",
                        () => 6666,
                        "port number"
                    ),

                new Option<string>
                    (
                        new [] { "--user", "--username", "--login" },
                        "user name"
                    ),

                new Option<string>
                    (
                        "--password",
                        "user password"
                    ),

                new Option<string>
                    (
                        "--arm",
                        () => "C",
                        "workstation kind"
                    ),

                new Option<string>
                    (
                        new[] { "--database", "--db", "--catalog" },
                        () => "IBIS",
                        "initial catalog"
                    )
            };

            return result;
        } // method GetRootCommand

        #endregion
    } // class CommandLineUtility

} // namespace ManagedIrbis.CommandLine
