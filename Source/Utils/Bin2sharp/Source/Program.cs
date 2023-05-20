// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/*

  Простая утилита, конвертирующая бинарный файл в исходный код на C#.

  Bin2sharp <input-file>

  Результат выводится в стандартный выходной поток.

 */

/* Program.cs -- вся логика программы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace Bin2sharp;

/// <summary>
/// Вся логика программы.
/// </summary>
public static class Program
{
    /// <summary>
    /// Convert array of bytes to C# source code.
    /// </summary>
    private static string ToSourceCode
        (
            IReadOnlyList<byte> array
        )
    {
        var result = new StringBuilder ("{\n");
        for (var i = 0; i < array.Count; i++)
        {
            if (i == 0)
            {
                result.Append ("  ");
            }
            else
            {
                result.Append(", ");
                if (i % 10 == 0)
                {
                    result.AppendLine();
                    result.Append("  ");
                }
            }
            result.AppendFormat
                (
                    CultureInfo.InvariantCulture,
                    "0x{0:X2}",
                    array [i]
                );
        }

        result.Append ("\n}");

        return result.ToString();
    }

    public static int Main
        (
            string[] args
        )
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine ("Usage: Bin2sharp <file>");

            return 1;
        }

        try
        {
            var array = File.ReadAllBytes (args[0]);
            Console.Out.WriteLine (ToSourceCode (array));
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);

            return 1;
        }

        return 0;
    }
}
