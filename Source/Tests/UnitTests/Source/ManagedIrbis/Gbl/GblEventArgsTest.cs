// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblEventArgsTest
    {
        [TestMethod]
        public void GblEventArgs_Construction()
        {
            var mock = new Mock<ISyncProvider>();
            mock.SetupAllProperties();
            var connection = mock.Object;
            connection.Database = "IBIS";
            var corrector = new GlobalCorrector(connection);
            var args = new GblEventArgs(corrector);

            Assert.AreSame(corrector, args.Corrector);
            Assert.AreEqual(false, args.Cancel);
        }
    }
}
