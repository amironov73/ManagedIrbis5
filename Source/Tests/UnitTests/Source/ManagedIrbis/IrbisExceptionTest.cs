// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#nullable enable

using System;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis;

[TestClass]
public class IrbisExceptionTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void IrbisException_Constructor_1()
    {
        var exception = new IrbisException();
        Assert.AreEqual (0, exception.ErrorCode);
    }

    [TestMethod]
    [Description ("Конструктор с кодом ошибки")]
    public void IrbisException_Constructor_2()
    {
        var exception = new IrbisException (-2222);
        Assert.AreEqual (-2222, exception.ErrorCode);
    }

    [TestMethod]
    [Description ("Конструктор с сообщением об ошибке")]
    public void IrbisException_Constructor_3()
    {
        const string expected = "internal error";
        var exception = new IrbisException (expected);
        Assert.AreEqual (0, exception.ErrorCode);
        Assert.AreEqual (expected, exception.Message);
    }

    [TestMethod]
    [Description ("Конструктор с вложенным исключением")]
    public void IrbisException_Constructor_4()
    {
        const string expected = "internal error";
        var innerException = new Exception();
        var exception = new IrbisException (expected, innerException);
        Assert.AreEqual (0, exception.ErrorCode);
        Assert.AreEqual (expected, exception.Message);
        Assert.AreSame (innerException, exception.InnerException);
    }

    [TestMethod]
    [Description ("Получение сообщения об ошибке по ее коду")]
    public void IrbisException_GetErrorDescription_1()
    {
        //using (UICultureSaver.ForTesting())
        //{
        Assert.AreEqual
            (
                "No error",
                IrbisException.GetErrorDescription (100)
            );
        Assert.AreEqual
            (
                "Normal return",
                IrbisException.GetErrorDescription (0)
            );
        Assert.AreEqual
            (
                "The field is absent",
                IrbisException.GetErrorDescription (-200)
            );

        //}
    }

    [TestMethod]
    [Description ("Получение сообщения об ошибке по ее коду")]
    public void IrbisException_GetErrorDescription_2()
    {
        //using (UICultureSaver.ForTesting())
        //{
        Assert.AreEqual
            (
                "Normal return",
                IrbisException.GetErrorDescription (ReturnCode.NoError)
            );
        Assert.AreEqual
            (
                "File not exist",
                IrbisException.GetErrorDescription (ReturnCode.FileNotExist)
            );

        //}
    }

    [TestMethod]
    [Description ("Получение сообщения об ошибке из исключения")]
    public void IrbisException_GetErrorDescription_3()
    {
        //using (UICultureSaver.ForTesting())
        //{
        Assert.AreEqual
            (
                "No error",
                IrbisException.GetErrorDescription (new IrbisException (100))
            );
        Assert.AreEqual
            (
                "The field is absent",
                IrbisException.GetErrorDescription (new IrbisException (-200))
            );

        //}
    }

    // [TestMethod]
    // public void IrbisException_ToString_1()
    // {
    //     using (UICultureSaver.ForTesting())
    //     {
    //         Assert.AreEqual
    //             (
    //                 "ErrorCode: -200\nDescription: The field is absent\nManagedIrbis.IrbisException: The field is absent",
    //                 new IrbisException (-200).ToString().DosToUnix()
    //             );
    //
    //         Assert.AreEqual
    //             (
    //                 "ErrorCode: 0\nDescription: network error\nManagedIrbis.IrbisException: network error",
    //                 new IrbisException ("network error").ToString().DosToUnix()
    //             );
    //     }
    // }
}
