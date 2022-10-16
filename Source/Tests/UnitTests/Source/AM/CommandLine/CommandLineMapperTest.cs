// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.CommandLine;

using AM.CommandLine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.CommandLine;

[TestClass]
public sealed class CommandLineMapperTest
{
    private sealed class HelloClass
    {
        public string? Verb { get; set; }
        public string? Noun { get; set; }

        public override string ToString() =>
            $"{nameof (Verb)}: {Verb}, {nameof (Noun)}: {Noun}";
    }

    private Command GetCommand()
    {
        return new RootCommand ("HelloCmd")
        {
            new Argument<string> ("input") { Arity = ArgumentArity.ZeroOrOne },
            new Argument<string> ("output") { Arity = ArgumentArity.ZeroOrOne },
            new Option<string> ("--verb", () => "Hello", "the verb"),
            new Option<string> ("--noun", () => "World", "the noun")
        };
    }

    private string[] GetArguments()
    {
        return new[] { "input.txt", "output.txt", "--verb", "Hola", "--noun", "Irkutsk", "--unknown", "value" };
    }

    [TestMethod]
    public void CommandLineMapper_MapCommandResult_1()
    {
        var command = GetCommand();
        var arguments = GetArguments();
        var hello = command.MapCommandResult<HelloClass> (arguments);
        Assert.AreEqual ("Hola", hello.Verb);
        Assert.AreEqual ("Irkutsk", hello.Noun);
    }

    [TestMethod]
    public void CommandLineMapper_MapCommandResult_2()
    {
        var command = GetCommand();
        var arguments = Array.Empty<string>();
        var hello = command.MapCommandResult<HelloClass> (arguments);
        Assert.AreEqual ("Hello", hello.Verb);
        Assert.AreEqual ("World", hello.Noun);
    }
}
