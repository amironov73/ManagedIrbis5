// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.Scripting.Sharping;

#nullable enable

namespace UnitTests.ManagedIrbis.Scripting.Sharping
{
    [TestClass]
    public sealed class ScriptingContextTest
    {
        [TestMethod]
        public void ScriptingContext_Construction_1()
        {
            var output = TextWriter.Null;
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            Assert.AreSame (output, context.Output);
            Assert.AreSame (provider, context.Provider);
            Assert.IsNull (context.UserData);
        }

        [TestMethod]
        public void ScriptingContext_Count_1()
        {
            var output = TextWriter.Null;
            var provider = new NullProvider();
            var context = new ScriptContext(provider, output)
            {
                Record = new Record()
                    .Add (910, 'b', "N001")
                    .Add (910, 'b', "N002")
                    .Add (910, 'b', "N003")
            };
            Assert.AreEqual (3, context.Count(910));
            Assert.AreEqual (0, context.Count(911));
        }

        [TestMethod]
        public void ScriptingContext_Count_2()
        {
            var output = TextWriter.Null;
            var provider = new NullProvider();
            var context = new ScriptContext(provider, output);
            Assert.AreEqual (0, context.Count(910));
        }

        [TestMethod]
        public void ScriptingContext_FM_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output)
            {
                Record = new Record()
                    .Add (910, 'b', "N001")
                    .Add (910, 'b', "N002")
                    .Add (910, 'b', "N003")
            };

            Assert.IsNull (context.FM (910));
            Assert.IsNull (context.FM (911));
            Assert.IsNull (context.FM (910, 'a'));
            Assert.AreEqual ("N001", context.FM(910, 'b'));
            Assert.IsNull (context.FM (910, 0));
            Assert.IsNull (context.FM (911, 0));
            Assert.IsNull (context.FM (910, 0, 'a'));
            Assert.AreEqual ("N001", context.FM(910, 0, 'b'));
            Assert.IsNull (context.FM (910, 1));
            Assert.IsNull (context.FM (911, 1));
            Assert.IsNull (context.FM (910, 1, 'a'));
            Assert.AreEqual ("N002", context.FM(910, 1, 'b'));
        }

        [TestMethod]
        public void ScriptingContext_FMA_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output)
            {
                Record = new Record()
                    .Add (910, 'b', "N001")
                    .Add (910, 'b', "N002")
                    .Add (910, 'b', "N003")
            };

            Assert.AreEqual (0, context.FMA(910).Length);
            Assert.AreEqual (0, context.FMA(911).Length);
            Assert.AreEqual (0, context.FMA(910, 'a').Length);
            Assert.AreEqual (3, context.FMA(910, 'b').Length);
        }

        [TestMethod]
        public void ScriptingContext_FormatRecord_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            context.BeforeAll();
            context.FormatRecord();
            context.AfterAll();
            var actual = output.ToString();
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void ScriptingContext_HaveField_1()
        {
            var output = TextWriter.Null;
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output)
            {
                Record = new Record()
                    .Add (910, 'b', "N001")
                    .Add (910, 'b', "N002")
                    .Add (910, 'b', "N003")
            };
            Assert.IsTrue (context.HaveField (910));
            Assert.IsFalse (context.HaveField (900));
        }

        [TestMethod]
        public void ScriptingContext_HaveField_2()
        {
            var output = TextWriter.Null;
            var provider = new NullProvider();
            var context = new ScriptContext(provider, output);
            Assert.IsFalse (context.HaveField (910));
            Assert.IsFalse (context.HaveField (900));
        }

        [TestMethod]
        public void ScriptingContext_V_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output)
            {
                Record = new Record()
                    .Add (910, 'b', "N001")
                    .Add (910, 'b', "N002")
                    .Add (910, 'b', "N003")
            };
            var result = context.V (910, 'b', after: ", ", skipLast: true);
            var actual = output.ToString();
            Assert.IsTrue (result);
            Assert.AreEqual ("N001, N002, N003", actual);
        }

        [TestMethod]
        public void ScriptingContext_V_2()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            var result = context.V (910, 'b', after: ", ", skipLast: true);
            var actual = output.ToString();
            Assert.IsFalse (result);
            Assert.AreEqual (string.Empty, actual);
        }

        [TestMethod]
        public void ScriptingContext_UserData_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            var userData = new object();
            context.UserData = userData;
            Assert.AreSame (userData, context.UserData);
        }

        [TestMethod]
        public void ScriptingContext_Write_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            var expected = "This is a test";
            context.Write (expected);
            var actual = output.ToString();
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        public void ScriptingContext_WriteLine_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            context.WriteLine ();
            var actual = output.ToString();
            Assert.AreEqual (Environment.NewLine, actual);
        }

        [TestMethod]
        public void ScriptingContext_WriteLine2_1()
        {
            var output = new StringWriter();
            var provider = new NullProvider();
            var context = new ScriptContext (provider, output);
            var expected = "This is a test";
            context.WriteLine (expected);
            var actual = output.ToString();
            Assert.AreEqual (expected + Environment.NewLine, actual);
        }

    }
}
