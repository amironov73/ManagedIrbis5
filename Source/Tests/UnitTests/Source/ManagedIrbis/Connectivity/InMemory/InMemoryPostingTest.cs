// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory
{
    [TestClass]
    public sealed class InMemoryPostingTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void InMemoryPosting_Construction_1()
        {
            var posting = new InMemoryPosting();
            Assert.AreEqual (0, posting.Mfn);
            Assert.AreEqual (0, posting.Tag);
            Assert.AreEqual (0, posting.Occurrence);
            Assert.AreEqual (0, posting.Position);
        }

    }
}
