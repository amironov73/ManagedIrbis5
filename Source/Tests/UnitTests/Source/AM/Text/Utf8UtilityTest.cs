// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public sealed class Utf8UtilityTest
    : Common.CommonUnitTest
{
    [TestMethod]
    [Description ("Подсчет числа байтов: пустая строка")]
    public unsafe void Utf8Utility_CountBytes_1()
    {
        var data = stackalloc char[10];
        Assert.AreEqual (0u, Utf8Utility.CountBytes (data, 0));
    }

    [TestMethod]
    [Description ("Подсчет числа байтов: строка с латиницей")]
    public unsafe void Utf8Utility_CountBytes_2()
    {
        var data = stackalloc char[] { 'H', 'e', 'l', 'l', 'o' };
        Assert.AreEqual (5u, Utf8Utility.CountBytes (data, 5));
    }

    [TestMethod]
    [Description ("Подсчет числа байтов: строка с кириллицей")]
    public unsafe void Utf8Utility_CountBytes_3()
    {
        var data = stackalloc char[] { 'П', 'р', 'и', 'в', 'е', 'т' };
        Assert.AreEqual (12u, Utf8Utility.CountBytes (data, 6));
    }

    [TestMethod]
    [Description ("Подсчет числа байтов: строка с девангари")]
    public unsafe void Utf8Utility_CountBytes_4()
    {
        var data = stackalloc char[] { 'द', 'े', 'व', 'न', 'ा', 'ग', 'र', 'ी' };
        Assert.AreEqual (24u, Utf8Utility.CountBytes (data, 8));
    }

    [TestMethod]
    [Description ("Подсчет числа байтов: строка с грузинским")]
    public unsafe void Utf8Utility_CountBytes_5()
    {
        var data = stackalloc char[] { 'კ', 'ა', 'რ', 'გ', 'ი', 'დ', 'ღ', 'ე' };
        Assert.AreEqual (24u, Utf8Utility.CountBytes (data, 8));
    }

    [TestMethod]
    [Description ("Подсчет числа байтов: строки")]
    public void Utf8Utility_CountBytes_7()
    {
        Assert.AreEqual (0u, Utf8Utility.CountBytes (string.Empty));
        Assert.AreEqual (5u, Utf8Utility.CountBytes ("Hello"));
        Assert.AreEqual (12u, Utf8Utility.CountBytes ("Привет"));
    }

    [TestMethod]
    [Description ("Подсчет числа символов: пустая строка")]
    public unsafe void Utf8Utility_CountChars_1()
    {
        var data = stackalloc byte[10];
        Assert.AreEqual (0, Utf8Utility.CountChars (data, 0));
    }
    [TestMethod]
    [Description ("Подсчет числа символов: латиница")]
    public unsafe void Utf8Utility_CountChars_2()
    {
        var data = stackalloc byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        Assert.AreEqual (5, Utf8Utility.CountChars (data, 5));
    }

    [TestMethod]
    [Description ("Подсчет числа символов: кириллица")]
    public unsafe void Utf8Utility_CountChars_3()
    {
        var data = stackalloc byte[] { 0xD0, 0x9F, 0xD1, 0x80, 0xD0,
            0xB8, 0xD0, 0xB2, 0xD0, 0xB5, 0xD1, 0x82 };
        Assert.AreEqual (6, Utf8Utility.CountChars (data, 12));
    }

    [TestMethod]
    [Description ("Подсчет числа символов: девангари")]
    public unsafe void Utf8Utility_CountChars_4()
    {
        var data = stackalloc byte[] { 0xE0, 0xA4, 0xA6, 0xE0, 0xA5,
            0x87, 0xE0, 0xA4, 0xB5, 0xE0, 0xA4, 0xA8, 0xE0, 0xA4, 0xBE,
            0xE0, 0xA4, 0x97, 0xE0, 0xA4, 0xB0, 0xE0, 0xA5, 0x80 };
        Assert.AreEqual (8, Utf8Utility.CountChars (data, 24));
    }

    [TestMethod]
    [Description ("Посимвольное чтение: пустая строка")]
    public void Utf8Utility_ReadChar_1()
    {
        using var stream = new MemoryStream (Array.Empty<byte>());
        Assert.AreEqual ((char) 0, Utf8Utility.ReadChar (stream));
    }

    [TestMethod]
    [Description ("Посимвольное чтение: латиница")]
    public void Utf8Utility_ReadChar_2()
    {
        var data = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        using var stream = new MemoryStream (data);
        Assert.AreEqual ('H', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('e', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('l', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('l', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('o', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ((char) 0, Utf8Utility.ReadChar (stream));
    }

    [TestMethod]
    [Description ("Посимвольное чтение: кириллица")]
    public void Utf8Utility_ReadChar_3()
    {
        var data = new byte[] { 0xD0, 0x9F, 0xD1, 0x80, 0xD0,
            0xB8, 0xD0, 0xB2, 0xD0, 0xB5, 0xD1, 0x82 };
        using var stream = new MemoryStream (data);
        Assert.AreEqual ('П', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('р', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('и', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('в', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('е', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('т', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ((char) 0, Utf8Utility.ReadChar (stream));
    }

    [TestMethod]
    [Description ("Посимвольное чтение: девангари")]
    public void Utf8Utility_ReadChar_4()
    {
        var data = new byte[] { 0xE0, 0xA4, 0xA6, 0xE0, 0xA5,
            0x87, 0xE0, 0xA4, 0xB5, 0xE0, 0xA4, 0xA8, 0xE0, 0xA4, 0xBE,
            0xE0, 0xA4, 0x97, 0xE0, 0xA4, 0xB0, 0xE0, 0xA5, 0x80 };
        using var stream = new MemoryStream (data);
        Assert.AreEqual ('द', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('े', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('व', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('न', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('ा', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('ग', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('र', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ('ी', Utf8Utility.ReadChar (stream));
        Assert.AreEqual ((char) 0, Utf8Utility.ReadChar (stream));
    }

    [TestMethod]
    public void Utf8Utility_Validate_1()
    {
        Assert.IsTrue (Utf8Utility.Validate (ReadOnlySpan<byte>.Empty));
        Assert.IsTrue (Utf8Utility.Validate (Encoding.UTF8.GetBytes ("Hello")));
        Assert.IsTrue (Utf8Utility.Validate (Encoding.UTF8.GetBytes ("Привет")));
    }

    private bool ValidateFile (string fileName)
    {
        var fullName = Path.Combine (TestDataPath, "Utf8", fileName);
        var bytes = File.ReadAllBytes (fullName);
        return Utf8Utility.Validate (bytes);
    }

    [TestMethod]
    public void Utf8Utility_Validate_2()
    {
        Assert.IsTrue (ValidateFile ("utf8.html"));
        Assert.IsTrue (ValidateFile ("ru.sql"));
        Assert.IsFalse (ValidateFile ("cyr.txt"));
        Assert.IsTrue (ValidateFile ("UTF-8-demo.txt"));
        Assert.IsTrue (ValidateFile ("utf8BOM.txt"));
        Assert.IsTrue (ValidateFile ("UTF-8-test.txt"));
        Assert.IsFalse (ValidateFile ("UTF-8-test-illegal-311.txt"));
        Assert.IsFalse (ValidateFile ("UTF-8-test-illegal-312.txt"));
    }

    [TestMethod]
    [Description ("Валидация потока байтов: пустой поток")]
    public void Utf8Utility_ValidateStream_1()
    {
        using var stream = new MemoryStream (Array.Empty<byte>());
        Assert.IsTrue (Utf8Utility.Validate (stream));
    }

    [TestMethod]
    [Description ("Валидация потока байтов: латиница")]
    public void Utf8Utility_ValidateStream_2()
    {
        var data = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        using var stream = new MemoryStream (data);
        Assert.IsTrue (Utf8Utility.Validate (stream));
    }

    [TestMethod]
    [Description ("Валидация потока байтов: кириллица")]
    public void Utf8Utility_ValidateStream_3()
    {
        var data = new byte[] { 0xD0, 0x9F, 0xD1, 0x80, 0xD0,
            0xB8, 0xD0, 0xB2, 0xD0, 0xB5, 0xD1, 0x82 };
        using var stream = new MemoryStream (data);
        Assert.IsTrue (Utf8Utility.Validate (stream));
    }

    [TestMethod]
    [Description ("Валидация потока байтов: девангари")]
    public void Utf8Utility_ValidateStream_4()
    {
        var data = new byte[] { 0xE0, 0xA4, 0xA6, 0xE0, 0xA5,
            0x87, 0xE0, 0xA4, 0xB5, 0xE0, 0xA4, 0xA8, 0xE0, 0xA4, 0xBE,
            0xE0, 0xA4, 0x97, 0xE0, 0xA4, 0xB0, 0xE0, 0xA5, 0x80 };
        using var stream = new MemoryStream (data);
        Assert.IsTrue (Utf8Utility.Validate (stream));
    }

    [TestMethod]
    [Description ("Посимвольная запись: латиница")]
    public void Utf8Utility_WriteChar_1()
    {
        using var stream = new MemoryStream();
        Utf8Utility.WriteChar (stream, 'H');
        Utf8Utility.WriteChar (stream, 'e');
        Utf8Utility.WriteChar (stream, 'l');
        Utf8Utility.WriteChar (stream, 'l');
        Utf8Utility.WriteChar (stream, 'o');
        stream.Flush();
        var actual = stream.ToArray();
        var expected = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
        CollectionAssert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Посимвольная запись: кириллица")]
    public void Utf8Utility_WriteChar_2()
    {
        using var stream = new MemoryStream();
        Utf8Utility.WriteChar (stream, 'П');
        Utf8Utility.WriteChar (stream, 'р');
        Utf8Utility.WriteChar (stream, 'и');
        Utf8Utility.WriteChar (stream, 'в');
        Utf8Utility.WriteChar (stream, 'е');
        Utf8Utility.WriteChar (stream, 'т');
        stream.Flush();
        var actual = stream.ToArray();
        var expected = new byte[] { 0xD0, 0x9F, 0xD1, 0x80, 0xD0,
            0xB8, 0xD0, 0xB2, 0xD0, 0xB5, 0xD1, 0x82 };
        CollectionAssert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Посимвольная запись: девангари")]
    public void Utf8Utility_WriteChar_3()
    {
        using var stream = new MemoryStream();
        Utf8Utility.WriteChar (stream, 'द');
        Utf8Utility.WriteChar (stream, 'े');
        Utf8Utility.WriteChar (stream, 'व');
        Utf8Utility.WriteChar (stream, 'न');
        Utf8Utility.WriteChar (stream, 'ा');
        Utf8Utility.WriteChar (stream, 'ग');
        Utf8Utility.WriteChar (stream, 'र');
        Utf8Utility.WriteChar (stream, 'ी');
        stream.Flush();
        var actual = stream.ToArray();
        var expected = new byte[] { 0xE0, 0xA4, 0xA6, 0xE0, 0xA5,
            0x87, 0xE0, 0xA4, 0xB5, 0xE0, 0xA4, 0xA8, 0xE0, 0xA4, 0xBE,
            0xE0, 0xA4, 0x97, 0xE0, 0xA4, 0xB0, 0xE0, 0xA5, 0x80 };
        CollectionAssert.AreEqual (expected, actual);
    }
}
