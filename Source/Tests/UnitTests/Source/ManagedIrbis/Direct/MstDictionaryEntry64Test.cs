// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstDictionaryEntry64Test
    {
        [TestMethod]
        public void MstDictionaryEntry64_Construction_1()
        {
            var entry = new MstDictionaryEntry64();
            Assert.AreEqual(0, entry.Tag);
            Assert.AreEqual(0, entry.Position);
            Assert.AreEqual(0, entry.Length);
            Assert.IsNull(entry.Bytes);
            Assert.IsNull(entry.Text);
        }

        [TestMethod]
        public void MstDictionaryEntry64_ToString_1()
        {
            var entry = new MstDictionaryEntry64
            {
                Tag = 123,
                Position = 345,
                Length = 5,
                Bytes = new byte[] { 72, 101, 108, 108, 111 },
                Text = "Hello"
            };
            Assert.AreEqual("Tag: 123, Position: 345, Length: 5, Text: Hello", entry.ToString().DosToUnix());
        }
    }
}
