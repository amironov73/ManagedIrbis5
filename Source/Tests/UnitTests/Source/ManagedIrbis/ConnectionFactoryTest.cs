// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MustUseReturnValue

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ConnectionFactoryTest
    {
        [TestMethod]
        public void ConnectionFactory_CreateConnection_1()
        {
            Assert.IsNotNull(ConnectionFactory.Shared);

            var actual = ConnectionFactory.Shared.CreateConnection();
            Assert.IsNotNull(actual);
        }
    }
}
