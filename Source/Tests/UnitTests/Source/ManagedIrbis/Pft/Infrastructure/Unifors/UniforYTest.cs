﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforYTest
    {
        private Record _GetRecord()
        {
            var record = new Record();
            record.Fields.Add(new Field(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            record.Fields.Add(new Field(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            record.Fields.Add(new Field(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            record.Fields.Add(new Field(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            record.Fields.Add(new Field(910, "^A0^B559^C19990924^H107256G^=2^U2004/7"));
            record.Fields.Add(new Field(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60^22"));
            record.Fields.Add(new Field(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));

            return record;
        }

        [TestMethod]
        public void UniforY_FreeExemplars_1()
        {
            var context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            var unifor = new Unifor();
            var expression = "Y";
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual("(1), БИНТ(2), ЖГ(25), ХР(10), ЧЗ(2)", actual);
        }
    }
}
