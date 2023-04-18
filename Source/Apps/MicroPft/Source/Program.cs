// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;
using AM.Kotik;

using ManagedIrbis;

#endregion

#nullable enable

namespace MicroPft;

internal static class Program
{
    public static void Main (string[] args)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var record = CreateRecord();
        var microTest = new MicroTest();

        RunForSuccess (microTest, record);
        RunForException (microTest, record);

        Console.WriteLine (new string ('=', 70));
        var error = microTest.Total - microTest.Success;
        Console.WriteLine ($"Total={microTest.Total}, success={microTest.Success}, error={error}");
        Console.WriteLine ($"Elapsed={stopwatch.Elapsed.ToAutoString()}");
    }

    private static Record CreateRecord() => new ()
        {
            new Field (200)
            {
                new ('a', "Заглавие"),
                new ('e', "подзаголовочные"),
                new ('f', "первые сведения"),
                new ('g', "последующие"),
            },
            new Field (300, "Первое повторение"),
            new Field (300, "Второе повторение"),
            new Field (300, "Третье повторение"),
            new Field (301, "1"),
            new Field (301, "2"),
            new Field (301, "3"),
            new Field (302, 'a', "1"),
            new Field (302, 'a', "2"),
            new Field (302, 'a', "3"),
            new Field (303, "1234567890")
        };

    private static void RunForSuccess
        (
            MicroTest microTest,
            Record record
        )
    {
        UnconditionalLiteral (microTest, record);
        SimpleField (microTest, record);
        ConditionalLiteral (microTest, record);
        RepeatingLiteral (microTest, record);
        Spacing (microTest, record);
        OutputMode (microTest, record);
        SimpleGroup (microTest, record);
    }

    private static void UnconditionalLiteral
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                "'Hello world'",
                record,
                "Hello world"
            );

        microTest.RunForSuccess
            (
                "'Hello ' 'world'",
                record,
                "Hello world"
            );

        microTest.RunForSuccess
            (
                "'Hello ''world'",
                record,
                "Hello world"
            );

        microTest.RunForSuccess
            (
                "'Hello ', 'world'",
                record,
                "Hello world"
            );
    }

    public static void SimpleField
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                "v200^a",
                record,
                record.FM (200, 'a')!
            );

        microTest.RunForSuccess
            (
                "v301",
                record,
                "123"
            );

        microTest.RunForSuccess
            (
                "v302^a",
                record,
                "123"
            );

        microTest.RunForSuccess
            (
                "v303.5",
                record,
                "12345"
            );

        microTest.RunForSuccess
            (
                "v303*5",
                record,
                "67890"
            );
    }

    public static void ConditionalLiteral
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                """ "o" v301 """,
                record,
                "o123"
            );

        microTest.RunForSuccess
            (
                """ v301 "o" """,
                record,
                "123o"
            );

        microTest.RunForSuccess
            (
                """v200^a, ": "v200^e, " / "v200^f, "; "v200^g""",
                record,
                "Заглавие: подзаголовочные / первые сведения; последующие"
            );
    }

    public static void RepeatingLiteral
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                "v301|,|",
                record,
                "1,2,3,"
            );

        microTest.RunForSuccess
            (
                "v301+|,|",
                record,
                "1,2,3"
            );

        microTest.RunForSuccess
            (
                "|o|v301",
                record,
                "o1o2o3"
            );

        microTest.RunForSuccess
            (
                "|o|+v301",
                record,
                "1o2o3"
            );

        microTest.RunForSuccess
            (
                """ "o" |x|+ v301 """,
                record,
                "o1x2x3"
            );

        microTest.RunForSuccess
            (
                """ v301 +|x| "o" """,
                record,
                "1x2x3o"
            );
    }

    private static void Spacing
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                "c5 v200^a",
                record,
                "     Заглавие"
            );

        microTest.RunForSuccess
            (
                "x5 v200^a",
                record,
                "     Заглавие"
            );

        microTest.RunForSuccess
            (
                "'Hello'#'world'",
                record,
                "Hello\nworld"
            );

        microTest.RunForSuccess
            (
                "'Hello'/'world'",
                record,
                "Hello\nworld"
            );
    }

    private static void OutputMode
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                "mpl, v200^e",
                record,
                "подзаголовочные"
            );

        microTest.RunForSuccess
            (
                "mhl, v200^e",
                record,
                "подзаголовочные"
            );

        microTest.RunForSuccess
            (
                "mdl, v200^e",
                record,
                "подзаголовочные.  "
            );

        microTest.RunForSuccess
            (
                "mpu, v200^e",
                record,
                "ПОДЗАГОЛОВОЧНЫЕ"
            );

        microTest.RunForSuccess
            (
                "mhl, v200",
                record,
                "Заглавие; подзаголовочные, первые сведения, последующие"
            );
        
        microTest.RunForSuccess
            (
                "mdl, v200",
                record,
                "Заглавие; подзаголовочные, первые сведения, последующие.  "
            );

        microTest.RunForSuccess
            (
                "mdu, v200",
                record,
                "ЗАГЛАВИЕ; ПОДЗАГОЛОВОЧНЫЕ, ПЕРВЫЕ СВЕДЕНИЯ, ПОСЛЕДУЮЩИЕ.  "
            );
    }

    private static void SimpleGroup
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForSuccess
            (
                "(v200^a)",
                record,
                record.FM (200, 'a')!
            );

        microTest.RunForSuccess
            (
                """(v200^a, ": "v200^e)""",
                record,
                "Заглавие: подзаголовочные"
            );

        microTest.RunForSuccess
            (
                "(v300, ', ')",
                record,
                "Первое повторение, Второе повторение, Третье повторение, "
            );
    }

    private static void RunForException
        (
            MicroTest microTest,
            Record record
        )
    {
        microTest.RunForException<SyntaxException>
            (
                "v200^",
                record
            );

        microTest.RunForException<SyntaxException>
            (
                "|nothing",
                record
            );

        microTest.RunForException<SyntaxException>
            (
                "'nothing",
                record
            );
        
        microTest.RunForException<SyntaxException>
            (
                """ "nothing""",
                record
            );
    }
}
