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
    public class UniforPlus6Test
    {
        private void _6
            (
                bool deleted,
                string input,
                string expected
            )
        {
            var record = new Record
            {
                Status = deleted ? RecordStatus.LogicallyDeleted : RecordStatus.None
            };
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
        public void UniforPlus6_GetRecordStatus_1()
        {
            _6(false, "+6", "1");
            _6(true, "+6", "0");
        }
    }
}
