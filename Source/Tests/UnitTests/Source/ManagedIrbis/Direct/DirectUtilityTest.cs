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

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public class DirectUtilityTest
    : Common.CommonUnitTest
{
    private string _GetReadFileName()
    {
        return Path.Combine (TestDataPath, "record.txt");
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
    [Description ("Создание базы данных в формате ИРБИС32")]
    public void DirectUlility_CreateDatabase32_1()
    {
        var random = new Random();
        var directory = Path.Combine
            (
                Path.GetTempPath(),
                random.Next().ToInvariantString() + "_1"
            );
        Directory.CreateDirectory (directory);
        var path = Path.Combine (directory, "database");
        DirectUtility.CreateDatabase32 (path);
        var files = Directory.GetFiles (directory);
        Assert.AreEqual (8, files.Length);
    }

    [TestMethod]
    [Description ("Создание базы данных в формате ИРБИС64")]
    public void DirectUlility_CreateDatabase64_1()
    {
        var random = new Random();
        var directory = Path.Combine
            (
                Path.GetTempPath(),
                random.Next().ToInvariantString() + "_2"
            );
        Directory.CreateDirectory (directory);
        var path = Path.Combine (directory, "database");
        DirectUtility.CreateDatabase64 (path);
        var files = Directory.GetFiles (directory);
        Assert.AreEqual (5, files.Length);
    }

    [TestMethod]
    [Description ("Открытие файла режиме только чтения")]
    public void DirectUtility_OpenFile_1()
    {
        var fileName = _GetReadFileName();
        var stream = DirectUtility.OpenFile
            (
                fileName,
                DirectAccessMode.ReadOnly
            );
        Assert.IsNotNull (stream);
        stream.Dispose();
    }

    [TestMethod]
    [Description ("Открытие файла в совместном режиме")]
    public void DirectUtility_OpenFile_2()
    {
        var fileName = _GetWriteFileName();
        FileUtility.Touch (fileName);
        var stream = DirectUtility.OpenFile
            (
                fileName,
                DirectAccessMode.Shared
            );
        Assert.IsNotNull (stream);
        stream.Dispose();
        File.Delete (fileName);
    }

    [TestMethod]
    [Description ("Открытие файла в эксклюзивном режиме")]
    public void DirectUtility_OpenFile_3()
    {
        var fileName = _GetWriteFileName();
        FileUtility.Touch (fileName);
        var stream = DirectUtility.OpenFile
            (
                fileName,
                DirectAccessMode.Exclusive
            );
        Assert.IsNotNull (stream);
        stream.Dispose();
        File.Delete (fileName);
    }

    [TestMethod]
    [ExpectedException (typeof (IrbisException))]
    [Description ("Открытие файла в неверном формате")]
    public void DirectUtility_OpenFile_4()
    {
        var fileName = _GetWriteFileName();
        FileUtility.Touch (fileName);
        DirectUtility.OpenFile
            (
                fileName,
                (DirectAccessMode) 100
            );
    }

    [TestMethod]
    [Description ("Проецирование файла в память")]
    public void DirectUtility_OpenMemoryMappedFile_1()
    {
        var fileName = _GetReadFileName();
        var mappedFile = DirectUtility.OpenMemoryMappedFile (fileName);
        Assert.IsNotNull (mappedFile);
        mappedFile.Dispose();
    }

    [TestMethod]
    [Description ("Чтение целого числа в сетевом формате")]
    public void DirectUtility_ReadNetworkInt32_1()
    {
        var fileName = _GetReadFileName();
        var mmf = DirectUtility.OpenMemoryMappedFile (fileName);
        var accessor = mmf.CreateViewAccessor (0, 100, MemoryMappedFileAccess.Read);
        try
        {
            const int expected = 0x3639323A;
            var actual = accessor.ReadNetworkInt32 (4);
            Assert.AreEqual (expected, actual);
        }
        finally
        {
            accessor.Dispose();
            mmf.Dispose();
        }
    }

    [TestMethod]
    [Description ("Чтение целого числа в сетевом формате")]
    public void DirectUtility_ReadNetworkInt32_2()
    {
        var fileName = _GetReadFileName();
        var mmf = DirectUtility.OpenMemoryMappedFile (fileName);
        var stream = mmf.CreateViewStream (0, 100, MemoryMappedFileAccess.Read);
        try
        {
            const int expected = unchecked ((int) 0xEFBBBF23);
            var actual = stream.ReadNetworkInt32 ();
            Assert.AreEqual (expected, actual);
        }
        finally
        {
            stream.Dispose();
            mmf.Dispose();
        }
    }

    [TestMethod]
    [Description ("Чтение длинного целого в сетевом формате")]
    public void DirectUtility_ReadNetworkInt64_1()
    {
        var fileName = _GetReadFileName();
        var mappedFile = DirectUtility.OpenMemoryMappedFile (fileName);
        var accessor = mappedFile.CreateViewAccessor (0, 100, MemoryMappedFileAccess.Read);
        try
        {
            const long expected = 0x205E42323639323AL;
            var actual = accessor.ReadNetworkInt64 (4);
            Assert.AreEqual (expected, actual);
        }
        finally
        {
            accessor.Dispose();
            mappedFile.Dispose();
        }
    }

    [TestMethod]
    [Description ("Чтение длинного целого в сетевом формате")]
    public void DirectUtility_ReadNetworkInt64_2()
    {
        var fileName = _GetReadFileName();
        var mappedFile = DirectUtility.OpenMemoryMappedFile (fileName);
        var stream = mappedFile.CreateViewStream (0, 100, MemoryMappedFileAccess.Read);
        try
        {
            const long expected = 0x3639323AEFBBBF23;
            var actual = stream.ReadNetworkInt64 ();
            Assert.AreEqual (expected, actual);
        }
        finally
        {
            stream.Dispose();
            mappedFile.Dispose();
        }
    }
}
