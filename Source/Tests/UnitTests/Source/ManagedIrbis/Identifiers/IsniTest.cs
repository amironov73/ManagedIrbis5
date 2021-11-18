// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class IsniTest
    {
        [TestMethod]
        public void Isni_GenerateCheckDigit_1()
        {
            Assert.AreEqual ("X", Isni.GenerateCheckDigit ("000000029534656"));
            Assert.AreEqual ("7", Isni.GenerateCheckDigit ("000000021825009"));
            Assert.AreEqual ("0", Isni.GenerateCheckDigit ("000000015109370"));
            Assert.AreEqual ("X", Isni.GenerateCheckDigit ("000000021694233"));
        }
    }
}
