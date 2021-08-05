// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.CommandLine;

using AM.CommandLine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class CommandLineMapperTest
    {
        class HelloClass
        {
            public string? Verb { get; set; }
            public string? Noun { get; set; }
            public override string ToString() =>
                $"{nameof(Verb)}: {Verb}, {nameof(Noun)}: {Noun}";
        }

        private Command GetCommand()
        {
            return new RootCommand("HelloCmd")
            {
                new Argument<string>("input") { Arity = ArgumentArity.ZeroOrOne },
                new Argument<string>("output") { Arity = ArgumentArity.ZeroOrOne },
                new Option<string>("--verb", () => "Hello", "the verb"),
                new Option<string>("--noun", () => "World", "the noun")
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
            var hello = command.MapCommandResult<HelloClass>(arguments);
            Assert.AreEqual("Hola", hello.Verb);
            Assert.AreEqual("Irkutsk", hello.Noun);
        }

        [TestMethod]
        public void CommandLineMapper_MapCommandResult_2()
        {
            var command = GetCommand();
            var arguments = Array.Empty<string>();
            var hello = command.MapCommandResult<HelloClass>(arguments);
            Assert.AreEqual("Hello", hello.Verb);
            Assert.AreEqual("World", hello.Noun);
        }

    }
}
