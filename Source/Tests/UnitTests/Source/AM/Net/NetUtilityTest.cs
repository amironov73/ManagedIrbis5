// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.Net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Net;

#endregion

#nullable enable

namespace UnitTests.AM.Net;

[TestClass]
public sealed class NetUtilityTest
{
    [TestMethod]
    [Description ("Получение локальных адресов")]
    public void NetUtility_GetLocalAddresses_1()
    {
        var localAddresses = NetUtility.GetLocalAdresses();
        var text = string.Join<IPAddress> (';', localAddresses);
        // System.Console.WriteLine (text);
        Assert.IsNotNull (text);
    }

    [TestMethod]
    [Description ("Получение массива диапазонов адресов локальной сети")]
    public void NetUtility_GetLocalNetwork_1()
    {
        var localNetwork = NetUtility.GetLocalNetwork();
        var text = string.Join<IPRange> (';', localNetwork);
        // System.Console.WriteLine (text);
        Assert.IsNotNull (text);
    }
}
