// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Text;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforZTest
    {
        private void _Z
            (
                string input,
                string expected
            )
        {
            var context = new PftContext(null);
            var record = new Record();
            context.Record = record;
            var field = new Field(910);
            field.DecodeBody(input);
            record.Fields.Add(field);

            var unifor = new Unifor();
            unifor.Execute(context, null, "Z");

            var builder = new StringBuilder();
            foreach (var oneField in record.Fields)
            {
                builder.AppendLine(oneField.ToText());
            }
            var actual = builder.ToString().TrimEnd().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [Ignore]
        [TestMethod]
        public void UniforZ_GenerateExemplars_1()
        {
            _Z("^AR^Qwert", "^a0^qwert");
            _Z("^AR^B0/111", string.Empty);
            _Z("^AR^B-1/111", string.Empty);
            _Z("^AR^B5/111", "^a0^b111\n^a0^b112\n^a0^b113\n^a0^b114\n^a0^b115");
            _Z("^AR^B5", "^a0^b5");
            _Z("^AR^B5/", "^a0^b1\n^a0^b2\n^a0^b3\n^a0^b4\n^a0^b5");
            _Z("^AR^B5/111^H222", "^a0^b111^h222\n^a0^b112^h223\n^a0^b113^h224\n^a0^b114^h225\n^a0^b115^h226");
            _Z("^AR^B5/111^H222^Qwert", "^a0^b111^h222^qwert\n^a0^b112^h223^qwert\n^a0^b113^h224^qwert\n^a0^b114^h225^qwert\n^a0^b115^h226^qwert");
            _Z("^AR^B/5", "^a0^b5");
        }
    }
}
