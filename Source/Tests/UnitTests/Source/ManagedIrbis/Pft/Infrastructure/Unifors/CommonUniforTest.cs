// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

using System;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    public abstract class CommonUniforTest
        : Common.CommonUnitTest
    {
        private static Field Parse
            (
                int tag,
                string body
            )
        {
            var result = new Field (tag);
            result.DecodeBody (body);

            return result;
        }

        protected virtual Record GetRecord()
        {
            var result = new Record();
            result.Fields.Add (Parse (692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            result.Fields.Add (Parse (692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            result.Fields.Add (Parse (692, "^B2008/2009^CV^AЗИ^D25^E0^F0^S0^G20090830"));
            result.Fields.Add (Parse (692, "^B2008/2009^CV^D17^X!COM^K21^E0^M0^G20090830"));
            result.Fields.Add (Parse (692, "^B2008/2009^CV^X!NOFOND^D42^K46^E0^N0^S14.00^G20090830"));
            result.Fields.Add (Parse (692, "^B2007/2008^CV^AЗИ^D25^S0.00^G20080501"));
            result.Fields.Add (Parse (692, "^B2009/2010^CO^AЗИ^D25^E4^F6.25^G20100511"));
            result.Fields.Add (Parse (692, "^B2009/2010^CO^D17^X!COM^K21^E0^M0^G20100511"));
            result.Fields.Add (Parse (692, "^B2009/2010^CO^X!NOFOND^D42^K46^E4^N11.50^G20100511"));
            result.Fields.Add (Parse (692, "^B2007/2008^CV^D17^X!COM^G20080501"));
            result.Fields.Add (Parse (692, "^B2007/2008^CV^X!NOFOND^D42^S10.50^G20080501"));
            result.Fields.Add (Parse (692, "^B2010/2011^CO^AЗИ^D25^E4^F6.25^G20101208"));
            result.Fields.Add (Parse (692, "^B2010/2011^CO^D17^X!COM^K21^E0^M0^G20101208"));
            result.Fields.Add (Parse (692, "^B2010/2011^CO^X!NOFOND^D42^K46^E4^N11.50^G20101208"));
            result.Fields.Add (Parse (692, "^B2007/2008^CO^D42^E4^N10.50^G20080107"));
            result.Fields.Add (Parse (692, "^B2010/2011^CV^AЗИ^D25^E0^F0^S0^G20110408"));
            result.Fields.Add (Parse (692, "^B2010/2011^CV^D17^X!COM^K21^E0^M0^G20110408"));
            result.Fields.Add (Parse (692, "^B2010/2011^CV^X!NOFOND^D42^K46^E0^N0^S11.50^G20110408"));
            result.Fields.Add (Parse (692, "^B2007/2008^CO^D42^E4^Z10.50^G20080107"));
            result.Fields.Add (Parse (692, "^B2011/2012^CO^AЗИ^D25^E4^F6.25^G20120524"));
            result.Fields.Add (Parse (692, "^B2011/2012^CO^D17^X!COM^K21^E0^M0^G20120524"));
            result.Fields.Add (Parse (692, "^B2011/2012^CO^X!NOFOND^D42^K46^E4^N11.50^G20120524"));
            result.Fields.Add (Parse (692, "^B2011/2012^CO^X!NOZO^D46^E4^Z11.50^G20120524"));
            result.Fields.Add (Parse (692, "^B2008/2009^CO^D17^X!COM^G20081218"));
            result.Fields.Add (Parse (692, "^B2012/2013^CO^AЗИ^D25^E4^F1.00^G20130531"));
            result.Fields.Add (Parse (692, "^B2012/2013^CO^X!NOFOND^D42^K46^E4^N1.00^G20130531"));
            result.Fields.Add (Parse (692, "^B2012/2013^CO^X!NOZO^D46^E4^Z11.50^G20130531"));
            result.Fields.Add (Parse (102, "RU"));
            result.Fields.Add (Parse (10, "^a5-7110-0177-9^d300"));
            result.Fields.Add (Parse (675, "37"));
            result.Fields.Add (Parse (675, "37(470.311)(03)"));
            result.Fields.Add (Parse (964, "14"));
            result.Fields.Add (Parse (999, "0000002"));
            result.Fields.Add (Parse (920, "PAZK"));
            result.Fields.Add (Parse (210, "^aМ.^cСП \"Вся Москва\"^d1993"));
            result.Fields.Add (Parse (215, "^A240^Cил^D12^YДА^Z3"));
            result.Fields.Add (Parse (225, "^u2^aВся Москва"));
            result.Fields.Add (Parse (101, "rus"));
            result.Fields.Add (Parse (908, "К88"));
            result.Fields.Add (Parse (903, "37/К88-602720"));
            result.Fields.Add (Parse (690, "^L9.92"));
            result.Fields.Add (Parse (700, "^AАкулова^BЗ.М.^PНИИ ВК"));
            result.Fields.Add (Parse (900, "^B05^Cg^227^3j02"));
            result.Fields.Add (Parse (702, "^4340 ред.^AПавловский^BА.С."));
            result.Fields.Add (Parse (702, "^4340 ред.^AПанасенко^BВ.А."));
            result.Fields.Add (Parse (702, "^4340 ред.^AПанков^BИ."));
            result.Fields.Add (Parse (702, "^4340 ред.^AПетрова^BН.Б."));
            result.Fields.Add (Parse (907, "^A20020530^BДСМ"));
            result.Fields.Add (Parse (907, "^CПРФ^A20060601^BДСМ"));
            result.Fields.Add (Parse (907, "^C^A20060601^BДСМ"));
            result.Fields.Add (Parse (907, "^C^A20070109^B"));
            result.Fields.Add (Parse (907, "^C^A20020530^B"));
            result.Fields.Add (Parse (907, "^C^A20030129^B"));
            result.Fields.Add (Parse (907, "^C^A20050524^B"));
            result.Fields.Add (Parse (907, "^C^A20050525^B"));
            result.Fields.Add (Parse (907, "^C^A20051110^B"));
            result.Fields.Add (Parse (907, "^C^A20070207^B"));
            result.Fields.Add (Parse (907, "^A20071108^BОЛН^C"));
            result.Fields.Add (Parse (907, "^A20071226^BОЛН^C"));
            result.Fields.Add (Parse (907, "^A20080107^B^C"));
            result.Fields.Add (Parse (907, "^A20081101^BОЛН^C"));
            result.Fields.Add (Parse (907, "^A20080501^BОЛН^C"));
            result.Fields.Add (Parse (907, "^A20081218^BОЛН^C"));
            result.Fields.Add (Parse (907, "^CКТ^A20090108^B"));
            result.Fields.Add (Parse (907, "^A20090830^BОЛН^C"));
            result.Fields.Add (Parse (907, "^A20090909^BОЛН^C"));
            result.Fields.Add (Parse (907, "^A20100511^BОЛН^C"));
            result.Fields.Add (Parse (907, "^CКТ^A20100527^B1"));
            result.Fields.Add (Parse (907, "^A20100908^B1^C"));
            result.Fields.Add (Parse (20, "^0 ^! ^aRU^b93-1141"));
            result.Fields.Add (Parse (907, "^A20110328^B1^C"));
            result.Fields.Add (Parse (907, "^A20101208^B1^C"));
            result.Fields.Add (Parse (907, "^A20110408^B1^C"));
            result.Fields.Add (Parse (907, "^A20110908^B1^C"));
            result.Fields.Add (Parse (951,
                "^AПример PDF-файла.PDF^TПример внешнего объекта в виде PDF-файла - с постраничным просмотром^N14"));
            result.Fields.Add (Parse (907, "^CКТ^A20120522^B1"));
            result.Fields.Add (Parse (907, "^A20120524^B1^C"));
            result.Fields.Add (Parse (907, "^CДК^A20130516^B1"));
            result.Fields.Add (Parse (907, "^CКТ^A20130531^B1"));
            result.Fields.Add (Parse (693, "^B2012/2013^CV^AЗИ^D25^E0^F1^S0"));
            result.Fields.Add (Parse (693, "^B2012/2013^CV^X!NOFOND^D42^K46^E0^N1.00^S1.00"));
            result.Fields.Add (Parse (692, "^B2008/2009^CO^X!NOFOND^D42^E3^N14.00^G20081218"));
            result.Fields.Add (Parse (692, "^CV^AЗИ^D25^E0^F1^S0^G20140703"));
            result.Fields.Add (Parse (692, "^CV^X!NOFOND^D42^K46^E0^N1.00^S1.00^G20140703"));
            result.Fields.Add (Parse (691,
                "^! 3^IАКТ^DАктинометрия^SОПД^BФЭиОЭП^KУМО^AЗИ^Vспц^Oд/о^C310700^F3^WАКТ/3^GОсн^0ЗИ310700спцд/о-S3"));
            result.Fields.Add (Parse (907, "^A20140703^B1^C"));
            result.Fields.Add (Parse (701, "^AБабич^BА.М.^U2"));
            result.Fields.Add (Parse (200,
                "^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]"));
            result.Fields.Add (Parse (907, "^CКТ^A20141001^B1"));
            result.Fields.Add (Parse (910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            result.Fields.Add (Parse (910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            result.Fields.Add (Parse (910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            result.Fields.Add (Parse (910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            result.Fields.Add (Parse (910, "^A0^B559^C19990924^DЧЗ^H107256G^=2^U2004/7"));
            result.Fields.Add (Parse (910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            result.Fields.Add (Parse (910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));
            result.Fields.Add (Parse (905, "^F2^21"));
            result.Fields.Add (Parse (941, "^A0^B32^H107206G^DБИНТ^U2004/7^C19930907"));
            result.Fields.Add (Parse (941, "^A0^B33^H107216G^DБИНТ^U2004/7^C19930907"));
            result.Fields.Add (Parse (941, "^AU^BЗИ-1^DЖГ^S20140604^125^TЗИ^!КДИ^C20071226^01"));
            result.Mfn = 1;

            return result;
        }

        protected void Execute
            (
                string input,
                string expected
            )
        {
            Execute (GetRecord(), 0, input, expected);
        }

        protected void Execute
            (
                Record? record,
                string input,
                string expected
            )
        {
            Execute (GetRecord(), 0, input, expected);
        }

        protected void Execute
            (
                Record? record,
                int index,
                string input,
                string expected
            )
        {
            Execute (null, record, index, input, expected);
        }

        protected void Execute
            (
                PftGroup? group,
                Record? record,
                int index,
                string input,
                string expected
            )
        {
            using var provider = GetProvider();
            provider.Connect();

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
            ShowDifference (expected, actual!);
            Assert.AreEqual (expected, actual);
        }
    }
}
