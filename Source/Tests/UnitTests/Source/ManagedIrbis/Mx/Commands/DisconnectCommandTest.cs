// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands;

[TestClass]
public sealed class DisconnectCommandTest
    : CommonMxCommandTest
{
    [TestMethod]
    [Description ("Констуктор по умолчанию")]
    public void DisconnectCommand_Construction_1()
    {
        var command = new DisconnectCommand();
        Assert.AreEqual ("disconnect", command.Name);
    }

    [TestMethod]
    [Description ("Выполнение команды")]
    public void DisconnectCommand_Execute_1()
    {
        using var executive = GetExecutive();
        using var command = new DisconnectCommand();
        command.Initialize (executive);

        var arguments = Array.Empty<MxArgument>();
        command.Execute (executive, arguments);
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void DisconnectCommand_ToString_1()
    {
        var command = new DisconnectCommand();
        Assert.AreEqual ("disconnect", command.ToString());
    }
}
