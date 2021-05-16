// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusNTest
    {
        private void _N
            (
                Record record,
                string input,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            var unifor = new Unifor();
            var expression = input;
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforPlusN_GetFieldCount_1()
        {
            var record = new Record();
            _N(record, "+N692", "0");
            _N(record, "+N", "0");
            _N(record, "+N0", "0");
            _N(record, "+N-1", "0");
            _N(record, "+N910", "0");

            record.Fields.Add(new Field(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            _N(record, "+N692", "1");
            _N(record, "+N", "0");
            _N(record, "+N910", "0");

            record.Fields.Add(new Field(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            _N(record, "+N692", "2");

            record.Fields.Add(new Field(692, "^B2008/2009^CV^AЗИ^D25^E0^F0^S0^G20090830"));
            _N(record, "+N692", "3");

            record.Fields.Add(new Field(692, "^B2008/2009^CV^D17^X!COM^K21^E0^M0^G20090830"));
            _N(record, "+N692", "4");

            record.Fields.Clear();
            _N(record, "+N", "0");
        }
    }
}
