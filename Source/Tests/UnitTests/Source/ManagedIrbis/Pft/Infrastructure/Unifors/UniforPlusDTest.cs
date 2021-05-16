// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusDTest
    {
        [TestMethod]
        public void UniforPlusD_GetDatabaseName_1()
        {
            const string DatabaseName = "IBIS";

            var context = new PftContext(null)
            {
                Provider =
                {
                    Database = DatabaseName
                }
            };
            var unifor = new Unifor();
            var expression = "+D";
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(DatabaseName, actual);
        }
    }
}
