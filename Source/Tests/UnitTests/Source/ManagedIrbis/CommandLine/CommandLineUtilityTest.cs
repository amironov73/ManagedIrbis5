﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

using ManagedIrbis;
using ManagedIrbis.CommandLine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.CommandLine
{
    [TestClass]
    public class CommandLineUtilityTest
    {
        [TestMethod]
        public void CommandLineUtility_GetRootCommand_1()
        {
            var rootCommand = CommandLineUtility.GetRootCommand();
            rootCommand.Handler = CommandHandler.Create
                (
                    (ConnectionSettings settings) =>
                    {
                        Assert.AreEqual("testHost", settings.Host);
                        Assert.AreEqual((ushort)5555, settings.Port);
                        Assert.AreEqual("librarian", settings.Username);
                        Assert.AreEqual("secret", settings.Password);
                        Assert.AreEqual("PERIO", settings.Database);
                        Assert.AreEqual("A", settings.Workstation);
                    }
                );

            var parser = new CommandLineBuilder(rootCommand).Build();
            var arguments = new[]
            {
                "--host", "testHost",
                "--port", "5555",
                "--username", "librarian",
                "--password", "secret",
                "--database", "PERIO",
                "--workstation", "A"
            };
            parser.Invoke(arguments);
        }

        [TestMethod]
        public void CommandLineUtility_GetRootCommand_2()
        {
            var rootCommand = CommandLineUtility.GetRootCommand();
            rootCommand.Handler = CommandHandler.Create
                (
                    (ConnectionSettings settings) =>
                    {
                        Assert.AreEqual("127.0.0.1", settings.Host);
                        Assert.AreEqual((ushort)6666, settings.Port);
                        Assert.AreEqual(string.Empty, settings.Username);
                        Assert.AreEqual(string.Empty, settings.Password);
                        Assert.IsNull(settings.Database);
                        Assert.IsNull(settings.Workstation);
                    }
                );

            var parser = new CommandLineBuilder(rootCommand).Build();
            var arguments = Array.Empty<string>();
            parser.Invoke(arguments);
        }

        [TestMethod]
        public void CommandLineUtility_GetRootCommand_3()
        {
            var rootCommand = CommandLineUtility.GetRootCommand();
            rootCommand.Handler = CommandHandler.Create
            (
                (ConnectionSettings settings) =>
                {
                    Assert.AreEqual("testHost", settings.Host);
                    Assert.AreEqual((ushort)5555, settings.Port);
                    Assert.AreEqual("librarian", settings.Username);
                    Assert.AreEqual("secret", settings.Password);
                    Assert.AreEqual("PERIO", settings.Database);
                    Assert.AreEqual("A", settings.Workstation);
                }
            );

            var parser = new CommandLineBuilder(rootCommand).Build();
            var arguments = new[]
            {
                "someCommand",  // добавляем посторонние аргументы
                "--use", "full", // и опции, чтобы посмотреть, что получится
                "--host", "testHost",
                "--port", "5555",
                "--username", "librarian",
                "--password", "secret",
                "--database", "PERIO",
                "--workstation", "A"
            };
            parser.Invoke(arguments);
        }

        [TestMethod]
        public void CommandLineUtility_GetRootCommand_4()
        {
            var rootCommand = CommandLineUtility.GetRootCommand();
            var arguments = new[]
            {
                "--host", "testHost",
                "--port", "5555",
                "--username", "librarian",
                "--password", "secret",
                "--database", "PERIO",
                "--workstation", "A"
            };
            var parsed = rootCommand.Parse(arguments);

            Assert.AreEqual("testHost", parsed.ValueForOption("--host"));
            Assert.AreEqual((ushort)5555, parsed.ValueForOption<ushort>("--port"));
            Assert.AreEqual("librarian", parsed.ValueForOption("--username"));
            Assert.AreEqual("secret", parsed.ValueForOption("--password"));
            Assert.AreEqual("PERIO", parsed.ValueForOption("--database"));
            Assert.AreEqual("A", parsed.ValueForOption("--workstation"));
        }

        [TestMethod]
        public void CommandLineUtility_GetRootCommand_5()
        {
            var rootCommand = CommandLineUtility.GetRootCommand();
            var arguments = Array.Empty<string>();
            var parsed = rootCommand.Parse(arguments);

            Assert.IsNull(parsed.ValueForOption("--host"));
            Assert.AreEqual((ushort)0, parsed.ValueForOption<ushort>("--port"));
            Assert.IsNull(parsed.ValueForOption("--username"));
            Assert.IsNull(parsed.ValueForOption("--password"));
            Assert.IsNull(parsed.ValueForOption("--database"));
            Assert.IsNull(parsed.ValueForOption("--workstation"));
        }

        [TestMethod]
        public void CommandLineUtility_ConfigureConnectionFromCommandLine_1()
        {
            var arguments = new[]
            {
                "--host", "testHost",
                "--port", "5555",
                "--username", "librarian",
                "--password", "secret",
                "--database", "PERIO",
                "--workstation", "A"
            };

            var mock = new Mock<IConnection>();
            mock.SetupAllProperties();
            var connection = mock.Object;
            CommandLineUtility.ConfigureConnectionFromCommandLine
                (
                    connection,
                    arguments
                );

            // TODO: придумать, как замочить Busy
            // Assert.IsFalse(connection.Busy);
            Assert.IsFalse(connection.Connected);
            Assert.AreEqual("testHost", connection.Host);
            Assert.AreEqual((ushort)5555, connection.Port);
            Assert.AreEqual("librarian", connection.Username);
            Assert.AreEqual("secret", connection.Password);
            Assert.AreEqual("PERIO", connection.Database);
            Assert.AreEqual("A", connection.Workstation);
        }

        [TestMethod]
        public void CommandLineUtility_ConfigureConnectionFromCommandLine_2()
        {
            var arguments = Array.Empty<string>();
            var mock = new Mock<IConnection>();
            mock.SetupAllProperties();
            var connection = mock.Object;
            CommandLineUtility.ConfigureConnectionFromCommandLine
                (
                    connection,
                    arguments
                );

            // TODO: придумать, как замочить Busy
            // Assert.IsFalse(connection.Busy);
            Assert.IsFalse(connection.Connected);
            Assert.AreEqual("127.0.0.1", connection.Host);
            Assert.AreEqual((ushort)6666, connection.Port);
            Assert.IsNull(connection.Username);
            Assert.IsNull(connection.Password);
            Assert.IsNull(connection.Database);
            Assert.IsNull(connection.Workstation);
        }

        [TestMethod]
        public void CommandLineUtility_ConfigureConnectionFromCommandLine_3()
        {
            var arguments = new[]
            {
                "someCommand",  // добавляем посторонние аргументы
                "--use", "full", // и опции, чтобы посмотреть, что получится
                "--host", "testHost",
                "--port", "5555",
                "--username", "librarian",
                "--password", "secret",
                "--database", "PERIO",
                "--workstation", "A"
            };

            var mock = new Mock<IConnection>();
            mock.SetupAllProperties();
            var connection = mock.Object;
            CommandLineUtility.ConfigureConnectionFromCommandLine
                (
                    connection,
                    arguments
                );

            // TODO: придумать, как замочить Busy
            // Assert.IsFalse(connection.Busy);
            Assert.IsFalse(connection.Connected);
            Assert.AreEqual("testHost", connection.Host);
            Assert.AreEqual((ushort)5555, connection.Port);
            Assert.AreEqual("librarian", connection.Username);
            Assert.AreEqual("secret", connection.Password);
            Assert.AreEqual("PERIO", connection.Database);
            Assert.AreEqual("A", connection.Workstation);
        }

        [TestMethod]
        public void CommandLineUtility_ParseEnvironment_1()
        {
            var rootCommand = CommandLineUtility.GetRootCommand();
            var parser = new CommandLineBuilder(rootCommand).Build();
            var environment = "--host:testHost --port:5555 --username:librarian "
                              + "--password:secret --database:PERIO --workstation:A";
            var parsed = parser.Parse(environment);

            Assert.AreEqual("testHost", parsed.ValueForOption("--host"));
            Assert.AreEqual((ushort)5555, parsed.ValueForOption<ushort>("--port"));
            Assert.AreEqual("librarian", parsed.ValueForOption("--username"));
            Assert.AreEqual("secret", parsed.ValueForOption("--password"));
            Assert.AreEqual("PERIO", parsed.ValueForOption("--database"));
            Assert.AreEqual("A", parsed.ValueForOption("--workstation"));
        }

        [TestMethod]
        public void CommandLineUtility_ParseEnvironment_2()
        {
            var environment = "--host:testHost --port:5555 --username:librarian "
                              + "--password:secret --database:PERIO --workstation:A";
            var settings = new ConnectionSettings();
            CommandLineUtility.ParseEnvironment(settings, environment);

            Assert.AreEqual("testHost", settings.Host);
            Assert.AreEqual((ushort)5555, settings.Port);
            Assert.AreEqual("librarian", settings.Username);
            Assert.AreEqual("secret", settings.Password);
            Assert.AreEqual("PERIO", settings.Database);
            Assert.AreEqual("A", settings.Workstation);
        }

        [TestMethod]
        public void CommandLineUtility_ConfigureConnectionFromEnvironment_1()
        {
            var environmentValue = "--host:testHost --port:5555 --username:librarian "
                              + "--password:secret --database:PERIO --workstation:A";

            Environment.SetEnvironmentVariable("IRBIS64_CONNECTION", environmentValue);

            var mock = new Mock<IConnection>();
            mock.SetupAllProperties();
            var connection = mock.Object;
            CommandLineUtility.ConfigureConnectionFromEnvironment(connection);

            Environment.SetEnvironmentVariable("IRBIS64_CONNECTION", null);

            // TODO: придумать, как замочить Busy
            // Assert.IsFalse(connection.Busy);
            Assert.IsFalse(connection.Connected);
            Assert.AreEqual("testHost", connection.Host);
            Assert.AreEqual((ushort)5555, connection.Port);
            Assert.AreEqual("librarian", connection.Username);
            Assert.AreEqual("secret", connection.Password);
            Assert.AreEqual("PERIO", connection.Database);
            Assert.AreEqual("A", connection.Workstation);
        }

        [TestMethod]
        public void CommandLineUtility_ConfigureConnectionFromEnvironment_2()
        {
            var mock = new Mock<IConnection>();
            mock.SetupAllProperties();
            var connection = mock.Object;
            CommandLineUtility.ConfigureConnectionFromEnvironment(connection);

            // TODO: придумать, как замочить Busy
            // Assert.IsFalse(connection.Busy);
            Assert.IsFalse(connection.Connected);
            Assert.IsNull(connection.Host);
            Assert.AreEqual(0, connection.Port);
            Assert.IsNull(connection.Username);
            Assert.IsNull(connection.Password);
            Assert.IsNull(connection.Database);
            Assert.IsNull(connection.Workstation);
        }
    }
}
