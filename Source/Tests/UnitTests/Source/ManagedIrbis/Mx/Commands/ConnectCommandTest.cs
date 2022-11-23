// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis;
using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands;

[TestClass]
public sealed class ConnectCommandTest
    : CommonMxCommandTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ConnectCommand_Construction_1()
    {
        var command = new ConnectCommand();
        Assert.AreEqual ("connect", command.Name);
    }

    [TestMethod]
    [Description ("Выполнение команды")]
    [ExpectedException (typeof (IrbisException))]
    public void ConnectCommand_Execute_1()
    {
        using var executive = GetExecutive();
        using var command = new ConnectCommand();
        command.Initialize (executive);

        var arguments = Array.Empty<MxArgument>();
        command.Execute (executive, arguments);
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void ConnectCommand_ToString_1()
    {
        var command = new ConnectCommand();
        Assert.AreEqual ("connect", command.ToString());
    }
}
