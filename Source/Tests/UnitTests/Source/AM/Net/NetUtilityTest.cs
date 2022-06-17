// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable StringLiteralTypo

using System;
using System.Net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Net;

#nullable enable

namespace UnitTests.AM.Net;

[TestClass]
public sealed class NetUtilityTest
{
    [TestMethod]
    [Description ("")]
    public void NetUtility_GetLocalAddresses_1()
    {
        var localAddresses = NetUtility.GetLocalAdresses();
        var text = string.Join<IPAddress> (';', localAddresses);
        Console.WriteLine (text);
        Assert.IsNotNull (text);
    }

    [TestMethod]
    [Description ("Получение массива диапазонов адресов локальной сети")]
    public void NetUtility_GetLocalNetwork_1()
    {
        var localNetwork = NetUtility.GetLocalNetwork();
        var text = string.Join<IPRange> (';', localNetwork);
        Assert.IsNotNull (text);
    }
}
