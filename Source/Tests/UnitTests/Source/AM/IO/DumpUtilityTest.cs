﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

// ReSharper disable CheckNamespace

namespace UnitTests.AM.IO
{
    [TestClass]
    public class DumpUtilityTest
    {
        [TestMethod]
        public void DumpUtility_DumpToTextByte()
        {
            byte[] data = {1, 2, 5, 10, 25, 50, 100};

            string actual = DumpUtility.DumpToText(data)
                .TrimEnd();
            const string expected = "000000>   01 02 05 0A  19 32 64";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DumpUtility_DumpToTextInt32()
        {
            int[] data = { 1, 2, 5, 10, 25, 50, 100 };

            string actual = DumpUtility.DumpToText(data)
                .TrimEnd();
            const string expected = "000000>   00000001 00000002 00000005 0000000A  00000019 00000032 00000064";

            Assert.AreEqual(expected, actual);
        }
    }
}
