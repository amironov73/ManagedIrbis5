// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Net;

namespace UnitTests.AM.Net;

[TestClass]
public sealed class NetUtilityTest
{
    [TestMethod]
    public void NetUtility_GetLocalNetwork_1()
    {
        var localNetwork = NetUtility.GetLocalNetwork();
        var text = string.Join<IPRange> (';', localNetwork);
        Console.WriteLine (text);
        Assert.IsNotNull (text);
    }
}
