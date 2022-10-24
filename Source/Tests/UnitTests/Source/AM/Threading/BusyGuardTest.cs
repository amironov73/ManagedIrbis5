// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace

#region Using directives

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading;

#endregion

#nullable enable

namespace UnitTests.AM.Threading;

[TestClass]
public sealed class BusyGuardTest
{
    [TestMethod]
    [Description ("Захват и освобождение BusyState")]
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
