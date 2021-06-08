// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#nullable enable

namespace UnitTests.AM.Text
{
    [TestClass]
    public class Utf8UtilityTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public unsafe void Utf8Utility_CountBytes_1()
        {
            char* data = stackalloc char[10];
            Assert.AreEqual(0u, Utf8Utility.CountBytes(data, 0));
        }

        [TestMethod]
        public unsafe void Utf8Utility_CountBytes_2()
        {
            char* data = stackalloc char[] { 'H', 'e', 'l', 'l', 'o' };
            Assert.AreEqual(5u, Utf8Utility.CountBytes(data, 5));
        }

        [TestMethod]
        public unsafe void Utf8Utility_CountBytes_3()
        {
            char* data = stackalloc char[] { 'П', 'р', 'и', 'в', 'е', 'т' };
            Assert.AreEqual(12u, Utf8Utility.CountBytes(data, 6));
        }

        [TestMethod]
        public void Utf8Utility_CountBytes_4()
        {
            Assert.AreEqual(0u, Utf8Utility.CountBytes(string.Empty));
            Assert.AreEqual(5u, Utf8Utility.CountBytes("Hello"));
            Assert.AreEqual(12u, Utf8Utility.CountBytes("Привет"));
        }

        [TestMethod]
        public void Utf8Utility_Validate_1()
        {
            Assert.IsTrue(Utf8Utility.Validate(ReadOnlySpan<byte>.Empty));
            Assert.IsTrue(Utf8Utility.Validate(Encoding.UTF8.GetBytes("Hello")));
            Assert.IsTrue(Utf8Utility.Validate(Encoding.UTF8.GetBytes("Привет")));
        }

        private bool ValidateFile(string fileName)
        {
            var fullName = Path.Combine(TestDataPath, "Utf8", fileName);
            var bytes = File.ReadAllBytes(fullName);
            return Utf8Utility.Validate(bytes);
        }

        [TestMethod]
        public void Utf8Utility_Validate_2()
        {
            Assert.IsTrue(ValidateFile("utf8.html"));
            Assert.IsTrue(ValidateFile("ru.sql"));
            Assert.IsFalse(ValidateFile("cyr.txt"));
            Assert.IsTrue(ValidateFile("UTF-8-demo.txt"));
            Assert.IsTrue(ValidateFile("utf8BOM.txt"));
            Assert.IsTrue(ValidateFile("UTF-8-test.txt"));
            Assert.IsFalse(ValidateFile("UTF-8-test-illegal-311.txt"));
            Assert.IsFalse(ValidateFile("UTF-8-test-illegal-312.txt"));
        }
    }
}
