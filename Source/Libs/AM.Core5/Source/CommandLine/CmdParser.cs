// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CmdParser.cs -- простейший парсер командной строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

#endregion

namespace AM.CommandLine;

/// <summary>
/// Простейший парсер командной строки.
/// </summary>
[PublicAPI]
public sealed class CmdParser
{
    #region Properties

    /// <summary>
    /// Разделитель имени и значения в опциях.
    /// </summary>
    public char Delimiter { get; set; } = '=';

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор аргументов командной строки.
    /// </summary>
    /// <returns></returns>
    public ParsedCmdLine Parse() => Parse (Environment.GetCommandLineArgs().Skip (1));

    /// <summary>
    /// Разбор аргументов командной строки.
    /// </summary>
    public ParsedCmdLine Parse
        (
            IEnumerable<string> arguments
        )
    {
        Sure.NotNull (arguments);

        var result = new ParsedCmdLine();
        foreach (var argument in arguments)
        {
            if (argument.Contains (Delimiter))
            {
                var parts = argument.Split (Delimiter, 2);
                var option = new CmdOption
                {
                    Name = parts[0],
                    Value = parts.SafeAt (1)
                };
                result.Options.Add (option);
            }
            else
            {
                result.PositionalArguments.Add (argument);
            }
        }

        return result;
    }

    #endregion
}
