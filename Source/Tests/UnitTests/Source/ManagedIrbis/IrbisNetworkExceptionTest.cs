// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#nullable enable

using System;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis;

[TestClass]
public sealed class IrbisNetworkExceptionTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void IrbisNetworkException_Constructor_1()
    {
        var exception = new IrbisNetworkException();
        Assert.AreEqual (0, exception.ErrorCode);
    }

    [TestMethod]
    [Description ("Конструктор с кодом ошибки")]
    public void IrbisNetworkException_Constructor_2()
    {
        var exception = new IrbisNetworkException (-2222);
        Assert.AreEqual (-2222, exception.ErrorCode);
    }

    [TestMethod]
    [Description ("Конструктор с сообщением об ошибке")]
    public void IrbisNetworkException_Constructor_3()
    {
        const string expected = "internal error";
        var exception = new IrbisNetworkException (expected);
        Assert.AreEqual (0, exception.ErrorCode);
        Assert.AreEqual (expected, exception.Message);
    }

    [TestMethod]
    [Description ("Конструктор с вложенным исключением")]
    public void IrbisNetworkException_Constructor_4()
    {
        const string expected = "internal error";
        var innerException = new Exception();
        var exception = new IrbisNetworkException (expected, innerException);
        Assert.AreEqual (0, exception.ErrorCode);
        Assert.AreEqual (expected, exception.Message);
        Assert.AreSame (innerException, exception.InnerException);
    }
}
