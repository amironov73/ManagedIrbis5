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
public sealed class ListUsersCommandTest
    : CommonMxCommandTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ListUsersCommand_Construction_1()
    {
        var command = new ListUsersCommand();
        Assert.AreEqual ("listusers", command.Name);
    }

    [TestMethod]
    [Description ("Выполнение команды")]
    public void ListUsersCommand_Execute_1()
    {
        using var executive = GetExecutive();
        using var command = new ListUsersCommand();
        command.Initialize (executive);

        var arguments = Array.Empty<MxArgument>();
        command.Execute (executive, arguments);
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void ListUsersCommand_ToString_1()
    {
        var command = new ListUsersCommand();
        Assert.AreEqual ("listusers", command.ToString());
    }
}
