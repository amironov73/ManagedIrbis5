// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MustUseReturnValue

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis;

[TestClass]
public class ConnectionFactoryTest
{
    [TestMethod]
    [Description ("Создание синхронного подключения")]
    public void ConnectionFactory_CreateSyncConnection_1()
    {
        Assert.IsNotNull (ConnectionFactory.Shared);

        var actual = ConnectionFactory.Shared.CreateSyncConnection();
        Assert.IsNotNull (actual);
    }

    [TestMethod]
    [Description ("Создание асинхронного подключения")]
    public void ConnectionFactory_CreateAsyncConnection_1()
    {
        Assert.IsNotNull (ConnectionFactory.Shared);

        var actual = ConnectionFactory.Shared.CreateAsyncConnection();
        Assert.IsNotNull (actual);
    }

    [TestMethod]
    [Description ("Замена общей фабрики")]
    public void ConnectionFactory_Replace_1()
    {
        Assert.IsNotNull (ConnectionFactory.Shared);

        var replacement = new ConnectionFactory();

        var backup = ConnectionFactory.Replace (replacement);
        Assert.IsNotNull (backup);
        Assert.AreSame (replacement, ConnectionFactory.Shared);

        var previous = ConnectionFactory.Replace (backup);
        Assert.AreSame (backup, ConnectionFactory.Shared);
        Assert.AreSame (replacement, previous);
    }
}
