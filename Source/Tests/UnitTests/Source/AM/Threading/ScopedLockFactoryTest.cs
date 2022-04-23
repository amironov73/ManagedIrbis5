// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace

using AM.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public sealed class ScopedLockFactoryTest
{
    [TestMethod]
    public void ScopedLockFactory_Construction_1()
    {
        using var factory = new ScopedLockFactory();
        using (var lock1 = factory.CreateLock())
        {
            Assert.IsNotNull (lock1);
        }

        using (var lock2 = factory.CreateLock())
        {
            Assert.IsNotNull (lock2);
        }
    }
}
