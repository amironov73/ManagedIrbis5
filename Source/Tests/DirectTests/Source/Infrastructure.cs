// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Infrastructure.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.PlatformAbstraction;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace DirectTests;

static class Infrastructure
{
    public static string TestDataPath
    {
        get
        {
            var current = AppContext.BaseDirectory;

            while (!string.IsNullOrEmpty (current))
            {
                var candidate = Path.Combine (current, "TestData");
                if (Directory.Exists (candidate))
                {
                    return candidate;
                }

                current = Path.GetDirectoryName (current);
            }

            throw new Exception();
        }
    }

    public static string Irbis64RootPath => Path.Combine (TestDataPath, "Irbis64");

    static void ShowDifference
        (
            string? expected,
            string? actual
        )
    {
        if (expected is null)
        {
            Console.WriteLine ("EXPECTED is null");
            return;
        }

        if (actual is null)
        {
            Console.WriteLine ("ACTUAL is null");
            return;
        }

        var index = 0;
        while (index < expected.Length && index < actual.Length)
        {
            if (expected[index] != actual[index])
            {
                break;
            }

            ++index;
        }

        if (index == expected.Length && index == actual.Length)
        {
            return;
        }

        Console.WriteLine ($"Difference at index {index}");
        Console.WriteLine ($"Expected: {expected.Substring (index)}");
        Console.WriteLine ($"Actual  : {actual.Substring (index)}");
    }

    public static void AreEqual
        (
            string? expected,
            string? actual
        )
    {
        if (string.Compare (expected, actual, StringComparison.Ordinal) != 0)
        {
            ShowDifference (expected, actual);
        }
        else
        {
            Console.WriteLine ("OK");
        }
    }

    public static void AreEqual<T>
        (
            T expected,
            T actual
        )
        where T : IEquatable<T>
    {
        Console.WriteLine
            (
                expected.Equals (actual) ? "OK" : "FAIL"
            );
    }

    public static ISyncProvider GetProvider()
    {
        var rootPath = Infrastructure.Irbis64RootPath;
        var result = new DirectProvider (rootPath)
        {
            Database = "IBIS",
            PlatformAbstraction = new TestingPlatformAbstraction()
        };

        return result;
    }

    public static void Execute
        (
            string input,
            string expected
        )
    {
        Execute (Infrastructure.GetRecord(), 0, input, expected);
    }

    public static void Execute
        (
            Record? record,
            string input,
            string expected
        )
    {
        Execute (GetRecord(), 0, input, expected);
    }

    public static void Execute
        (
            Record? record,
            int index,
            string input,
            string expected
        )
    {
        Execute (null, record, index, input, expected);
    }

    public static void Execute
        (
            PftGroup? group,
            Record? record,
            int index,
            string input,
            string expected
        )
    {
        using var provider = GetProvider();
        var context = new PftContext (null)
        {
            CurrentGroup = @group,
            Record = record
        };
        context.SetProvider (provider);
        context.Index = index;
        var unifor = new Unifor();
        var expression = input;
        unifor.Execute (context, null, expression);
        var actual = context.Text.DosToUnix();
        AreEqual (expected, actual);
    }


    public static Record GetRecord()
    {
        var result = new Record();
        result.Fields.Add (new Field (692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
        result.Fields.Add (new Field (692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
        result.Fields.Add (new Field (692, "^B2008/2009^CV^AЗИ^D25^E0^F0^S0^G20090830"));
        result.Fields.Add (new Field (692, "^B2008/2009^CV^D17^X!COM^K21^E0^M0^G20090830"));
        result.Fields.Add (new Field (692, "^B2008/2009^CV^X!NOFOND^D42^K46^E0^N0^S14.00^G20090830"));
        result.Fields.Add (new Field (692, "^B2007/2008^CV^AЗИ^D25^S0.00^G20080501"));
        result.Fields.Add (new Field (692, "^B2009/2010^CO^AЗИ^D25^E4^F6.25^G20100511"));
        result.Fields.Add (new Field (692, "^B2009/2010^CO^D17^X!COM^K21^E0^M0^G20100511"));
        result.Fields.Add (new Field (692, "^B2009/2010^CO^X!NOFOND^D42^K46^E4^N11.50^G20100511"));
        result.Fields.Add (new Field (692, "^B2007/2008^CV^D17^X!COM^G20080501"));
        result.Fields.Add (new Field (692, "^B2007/2008^CV^X!NOFOND^D42^S10.50^G20080501"));
        result.Fields.Add (new Field (692, "^B2010/2011^CO^AЗИ^D25^E4^F6.25^G20101208"));
        result.Fields.Add (new Field (692, "^B2010/2011^CO^D17^X!COM^K21^E0^M0^G20101208"));
        result.Fields.Add (new Field (692, "^B2010/2011^CO^X!NOFOND^D42^K46^E4^N11.50^G20101208"));
        result.Fields.Add (new Field (692, "^B2007/2008^CO^D42^E4^N10.50^G20080107"));
        result.Fields.Add (new Field (692, "^B2010/2011^CV^AЗИ^D25^E0^F0^S0^G20110408"));
        result.Fields.Add (new Field (692, "^B2010/2011^CV^D17^X!COM^K21^E0^M0^G20110408"));
        result.Fields.Add (new Field (692, "^B2010/2011^CV^X!NOFOND^D42^K46^E0^N0^S11.50^G20110408"));
        result.Fields.Add (new Field (692, "^B2007/2008^CO^D42^E4^Z10.50^G20080107"));
        result.Fields.Add (new Field (692, "^B2011/2012^CO^AЗИ^D25^E4^F6.25^G20120524"));
        result.Fields.Add (new Field (692, "^B2011/2012^CO^D17^X!COM^K21^E0^M0^G20120524"));
        result.Fields.Add (new Field (692, "^B2011/2012^CO^X!NOFOND^D42^K46^E4^N11.50^G20120524"));
        result.Fields.Add (new Field (692, "^B2011/2012^CO^X!NOZO^D46^E4^Z11.50^G20120524"));
        result.Fields.Add (new Field (692, "^B2008/2009^CO^D17^X!COM^G20081218"));
        result.Fields.Add (new Field (692, "^B2012/2013^CO^AЗИ^D25^E4^F1.00^G20130531"));
        result.Fields.Add (new Field (692, "^B2012/2013^CO^X!NOFOND^D42^K46^E4^N1.00^G20130531"));
        result.Fields.Add (new Field (692, "^B2012/2013^CO^X!NOZO^D46^E4^Z11.50^G20130531"));
        result.Fields.Add (new Field (102, "RU"));
        result.Fields.Add (new Field (10, "^a5-7110-0177-9^d300"));
        result.Fields.Add (new Field (675, "37"));
        result.Fields.Add (new Field (675, "37(470.311)(03)"));
        result.Fields.Add (new Field (964, "14"));
        result.Fields.Add (new Field (999, "0000002"));
        result.Fields.Add (new Field (920, "PAZK"));
        result.Fields.Add (new Field (210, "^aМ.^cСП \"Вся Москва\"^d1993"));
        result.Fields.Add (new Field (215, "^A240^Cил^D12^YДА^Z3"));
        result.Fields.Add (new Field (225, "^u2^aВся Москва"));
        result.Fields.Add (new Field (101, "rus"));
        result.Fields.Add (new Field (908, "К88"));
        result.Fields.Add (new Field (903, "37/К88-602720"));
        result.Fields.Add (new Field (690, "^L9.92"));
        result.Fields.Add (new Field (700, "^AАкулова^BЗ.М.^PНИИ ВК"));
        result.Fields.Add (new Field (900, "^B05^Cg^227^3j02"));
        result.Fields.Add (new Field (702, "^4340 ред.^AПавловский^BА.С."));
        result.Fields.Add (new Field (702, "^4340 ред.^AПанасенко^BВ.А."));
        result.Fields.Add (new Field (702, "^4340 ред.^AПанков^BИ."));
        result.Fields.Add (new Field (702, "^4340 ред.^AПетрова^BН.Б."));
        result.Fields.Add (new Field (907, "^A20020530^BДСМ"));
        result.Fields.Add (new Field (907, "^CПРФ^A20060601^BДСМ"));
        result.Fields.Add (new Field (907, "^C^A20060601^BДСМ"));
        result.Fields.Add (new Field (907, "^C^A20070109^B"));
        result.Fields.Add (new Field (907, "^C^A20020530^B"));
        result.Fields.Add (new Field (907, "^C^A20030129^B"));
        result.Fields.Add (new Field (907, "^C^A20050524^B"));
        result.Fields.Add (new Field (907, "^C^A20050525^B"));
        result.Fields.Add (new Field (907, "^C^A20051110^B"));
        result.Fields.Add (new Field (907, "^C^A20070207^B"));
        result.Fields.Add (new Field (907, "^A20071108^BОЛН^C"));
        result.Fields.Add (new Field (907, "^A20071226^BОЛН^C"));
        result.Fields.Add (new Field (907, "^A20080107^B^C"));
        result.Fields.Add (new Field (907, "^A20081101^BОЛН^C"));
        result.Fields.Add (new Field (907, "^A20080501^BОЛН^C"));
        result.Fields.Add (new Field (907, "^A20081218^BОЛН^C"));
        result.Fields.Add (new Field (907, "^CКТ^A20090108^B"));
        result.Fields.Add (new Field (907, "^A20090830^BОЛН^C"));
        result.Fields.Add (new Field (907, "^A20090909^BОЛН^C"));
        result.Fields.Add (new Field (907, "^A20100511^BОЛН^C"));
        result.Fields.Add (new Field (907, "^CКТ^A20100527^B1"));
        result.Fields.Add (new Field (907, "^A20100908^B1^C"));
        result.Fields.Add (new Field (20, "^0 ^! ^aRU^b93-1141"));
        result.Fields.Add (new Field (907, "^A20110328^B1^C"));
        result.Fields.Add (new Field (907, "^A20101208^B1^C"));
        result.Fields.Add (new Field (907, "^A20110408^B1^C"));
        result.Fields.Add (new Field (907, "^A20110908^B1^C"));
        result.Fields.Add (new Field (951, "^AПример PDF-файла.PDF^TПример внешнего объекта в виде PDF-файла - с постраничным просмотром^N14"));
        result.Fields.Add (new Field (907, "^CКТ^A20120522^B1"));
        result.Fields.Add (new Field (907, "^A20120524^B1^C"));
        result.Fields.Add (new Field (907, "^CДК^A20130516^B1"));
        result.Fields.Add (new Field (907, "^CКТ^A20130531^B1"));
        result.Fields.Add (new Field (693, "^B2012/2013^CV^AЗИ^D25^E0^F1^S0"));
        result.Fields.Add (new Field (693, "^B2012/2013^CV^X!NOFOND^D42^K46^E0^N1.00^S1.00"));
        result.Fields.Add (new Field (692, "^B2008/2009^CO^X!NOFOND^D42^E3^N14.00^G20081218"));
        result.Fields.Add (new Field (692, "^CV^AЗИ^D25^E0^F1^S0^G20140703"));
        result.Fields.Add (new Field (692, "^CV^X!NOFOND^D42^K46^E0^N1.00^S1.00^G20140703"));
        result.Fields.Add (new Field (691, "^! 3^IАКТ^DАктинометрия^SОПД^BФЭиОЭП^KУМО^AЗИ^Vспц^Oд/о^C310700^F3^WАКТ/3^GОсн^0ЗИ310700спцд/о-S3"));
        result.Fields.Add (new Field (907, "^A20140703^B1^C"));
        result.Fields.Add (new Field (701, "^AБабич^BА.М.^U2"));
        result.Fields.Add (new Field (200, "^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]"));
        result.Fields.Add (new Field (907, "^CКТ^A20141001^B1"));
        result.Fields.Add (new Field (910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
        result.Fields.Add (new Field (910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
        result.Fields.Add (new Field (910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
        result.Fields.Add (new Field (910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
        result.Fields.Add (new Field (910, "^A0^B559^C19990924^DЧЗ^H107256G^=2^U2004/7"));
        result.Fields.Add (new Field (910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
        result.Fields.Add (new Field (910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));
        result.Fields.Add (new Field (905, "^F2^21"));
        result.Fields.Add (new Field (941, "^A0^B32^H107206G^DБИНТ^U2004/7^C19930907"));
        result.Fields.Add (new Field (941, "^A0^B33^H107216G^DБИНТ^U2004/7^C19930907"));
        result.Fields.Add (new Field (941, "^AU^BЗИ-1^DЖГ^S20140604^125^TЗИ^!КДИ^C20071226^01"));
        result.Mfn = 1;

        return result;
    }
}
