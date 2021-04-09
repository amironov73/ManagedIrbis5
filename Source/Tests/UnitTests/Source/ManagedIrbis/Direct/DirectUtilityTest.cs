// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.IO.MemoryMappedFiles;

using AM;
using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class DirectUtilityTest
        : Common.CommonUnitTest
    {
        private string _GetReadFileName()
        {
            return Path.Combine(TestDataPath, "record.txt");
        }

        private string _GetWriteFileName()
        {
            var random = new Random();
            var result = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToInvariantString()
                );

            return result;
        }

        [TestMethod]
        public void DirectUlility_CreateDatabase32_1()
        {
            var random = new Random();
            var directory = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToInvariantString() + "_1"
                );
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase32(path);
            var files = Directory.GetFiles(directory);
            Assert.AreEqual(8, files.Length);
        }

        [TestMethod]
        public void DirectUlility_CreateDatabase64_1()
        {
            var random = new Random();
            var directory = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToInvariantString() + "_2"
                );
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase64(path);
            var files = Directory.GetFiles(directory);
            Assert.AreEqual(5, files.Length);
        }

        [TestMethod]
        public void DirectUtility_OpenFile_1()
        {
            var fileName = _GetReadFileName();
            var stream = DirectUtility.OpenFile
                (
                    fileName,
                    DirectAccessMode.ReadOnly
                );
            Assert.IsNotNull(stream);
            stream.Dispose();
        }

        [TestMethod]
        public void DirectUtility_OpenFile_2()
        {
            var fileName = _GetWriteFileName();
            FileUtility.Touch(fileName);
            var stream = DirectUtility.OpenFile
                (
                    fileName,
                    DirectAccessMode.Shared
                );
            Assert.IsNotNull(stream);
            stream.Dispose();
            File.Delete(fileName);
        }

        [TestMethod]
        public void DirectUtility_OpenFile_3()
        {
            var fileName = _GetWriteFileName();
            FileUtility.Touch(fileName);
            var stream = DirectUtility.OpenFile
                (
                    fileName,
                    DirectAccessMode.Exclusive
                );
            Assert.IsNotNull(stream);
            stream.Dispose();
            File.Delete(fileName);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void DirectUtility_OpenFile_4()
        {
            var fileName = _GetWriteFileName();
            FileUtility.Touch(fileName);
            DirectUtility.OpenFile
                (
                    fileName,
                    (DirectAccessMode)100
                );
        }

        [TestMethod]
        public void DirectUtility_OpenMemoryMappedFile_1()
        {
            var fileName = _GetReadFileName();
            var mmf = DirectUtility.OpenMemoryMappedFile(fileName);
            Assert.IsNotNull(mmf);
            mmf.Dispose();
        }

        [TestMethod]
        public void DirectUtility_ReadNetworkInt32_1()
        {
            var fileName = _GetReadFileName();
            var mmf = DirectUtility.OpenMemoryMappedFile(fileName);
            var accessor
                = mmf.CreateViewAccessor(0, 100, MemoryMappedFileAccess.Read);
            try
            {
                var expected = 0x3639323A;
                var actual = DirectUtility.ReadNetworkInt32(accessor, 4);
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                accessor.Dispose();
                mmf.Dispose();
            }
        }

        [TestMethod]
        public void DirectUtility_ReadNetworkInt32_2()
        {
            var fileName = _GetReadFileName();
            var mmf = DirectUtility.OpenMemoryMappedFile(fileName);
            var stream
                = mmf.CreateViewStream(0, 100, MemoryMappedFileAccess.Read);
            try
            {
                var expected = unchecked((int)0xEFBBBF23);
                var actual = DirectUtility.ReadNetworkInt32(stream);
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                stream.Dispose();
                mmf.Dispose();
            }
        }

        [TestMethod]
        public void DirectUtility_ReadNetworkInt64_1()
        {
            var fileName = _GetReadFileName();
            var mmf = DirectUtility.OpenMemoryMappedFile(fileName);
            var accessor
                = mmf.CreateViewAccessor(0, 100, MemoryMappedFileAccess.Read);
            try
            {
                var expected = 0x205E42323639323AL;
                var actual = DirectUtility.ReadNetworkInt64(accessor, 4);
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                accessor.Dispose();
                mmf.Dispose();
            }
        }

        [TestMethod]
        public void DirectUtility_ReadNetworkInt64_2()
        {
            var fileName = _GetReadFileName();
            var mmf = DirectUtility.OpenMemoryMappedFile(fileName);
            var stream
                = mmf.CreateViewStream(0, 100, MemoryMappedFileAccess.Read);
            try
            {
                var expected = 0x3639323AEFBBBF23;
                var actual = DirectUtility.ReadNetworkInt64(stream);
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                stream.Dispose();
                mmf.Dispose();
            }
        }
    }
}
