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
            Assert.IsNotNull(ConnectionFactory.Default);

            var actual = ConnectionFactory.Default.CreateConnection();
            Assert.IsNotNull(actual);
        }
    }
}
