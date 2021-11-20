// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

#nullable enable

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public sealed class ConstantInfoTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void ConstantInfo_Constructor_1()
        {
            const string name = "Name";
            const string value = "Value";
            var info = new ConstantInfo<string> (name, value);
            Assert.AreEqual (name, info.Name);
            Assert.AreEqual (value, info.Value);
        }

    }
}
