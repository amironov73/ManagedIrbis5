// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading;

namespace UnitTests.AM.Threading;

[TestClass]
public sealed class BusyGuardTest
{
    //[Ignore]
    [TestMethod]
    public void BusyGuard_Construction_1()
    {
        var done = false;
        using var busy = new BusyState();

        var task = Task.Factory.StartNew
            (
                () =>
                {
                    using (new BusyGuard (busy))
                    {
                        done = true;
                    }
                }
            );

        busy.SetState (false);

        task.Wait();

        Assert.IsTrue (done);
    }
}
