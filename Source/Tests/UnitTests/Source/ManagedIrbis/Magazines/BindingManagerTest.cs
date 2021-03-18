using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Magazines;

using Moq;

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class BindingManagerTest
        : CommonMagazineTest
    {
        [TestMethod]
        public void BindingManager_Construction_1()
        {
            var mock = new Mock<IIrbisConnection>();
            var connection = mock.Object;

            var manager = new BindingManager(connection);
            Assert.AreSame(connection, manager.Connection);
        }
    }
}
