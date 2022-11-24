// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM;

using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands;

[TestClass]
public sealed class BangCommandTest
    : CommonMxCommandTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BangCommand_Construction_1()
    {
        var command = new BangCommand();
        Assert.AreEqual ("!", command.Name);
    }

    [TestMethod]
    [Description ("Выполнение команды")]
    public void BangCommand_Execute_1()
    {
        using var executive = GetExecutive();
        using var command = new BangCommand();
        command.Initialize (executive);

        const string source = "1 + 2";
        var arguments = new []{ new MxArgument { Text = source }};
        command.Execute (executive, arguments);
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void BangCommand_ToString_1()
    {
        var command = new BangCommand();
        Assert.AreEqual ("!", command.ToString());
    }

}
